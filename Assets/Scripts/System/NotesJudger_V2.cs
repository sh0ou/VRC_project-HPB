//-----------------------------------------------------------------------------
// Script Adviser:@mezum
// Thank you for your advice!
//-----------------------------------------------------------------------------
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    public class NotesJudger_V2 : UdonSharpBehaviour
    {
        [SerializeField, Tooltip("譜面データ変換スクリプト")]
        private TextFileConverter textFileConverter;

        [SerializeField, Tooltip("ゲームマネージャ")]
        private GameManager gameManager;

        [SerializeField, Tooltip("プレイマネージャ")]
        private PlayManager playManager;

        [SerializeField, Tooltip("設定マネージャ")]
        private SettingsManager settingsMng;

        [SerializeField, Tooltip("同期マネージャ")]
        private SyncManager syncManager;

        private const int NoteJudgeNone = 0;
        private const int NoteJudgeHappy = 1;
        private const int NoteJudgeGood = 2;
        private const int NoteJudgeSad = 3;

        private int[] NoteJudge = new int[]
        {
        NoteJudgeNone,
        NoteJudgeHappy,
        NoteJudgeGood,
        NoteJudgeSad,
        };

        /// <summary>None を含めた判定の種類の数です。</summary>
        private const int NoteJudgeKindCount = 3 + 1;  // None の為に +1

        [Tooltip("レーンの個数")]
        public readonly int LaneCount = 6;

        [Tooltip("各ノートの判定位置")]
        public float[][] noteTimeList;

        [Tooltip("各判定の閾値")]
        public float[] noteJudgeTimeSpanList;

        [Tooltip("各レーン毎の総ノート数")]
        public int[] totalNotesPerLaneList;

        [Tooltip("全てのレーンの総ノート数")]
        public int totalPlacedNotes;

        [Tooltip("各レーンごとの判定済みノート数")]
        public int[] judgedNotesCountPerLaneList;

        [Tooltip("各ノートごとの判定")]
        public int[][] noteJudgeResultsList;

        [UdonSynced, Tooltip("全てのレーンの判定済みノート数")]
        public int totalJudgedNotes;

        [Tooltip("各判定の総和\n*要素の順番や個数は NoteJudge と同じ順番と個数になるようにしてください。")]
        public int[] judgesCountList;

        [Tooltip("ノーツ数カウント補助用")]
        public int[] noteCountList;

        // -----------------------------------------------------------------------------
        // 初期化系統
        // -----------------------------------------------------------------------------

        /// <summary>
        /// 初期化します。
        /// </summary>
        public void Setup()
        {
            InitializeFields();
            CountNotesPerLane();
            InitializeFieldsRelatedNotesCount();
            CalculateNoteTimings();
        }

        /// <summary>
        /// フィールド (変数) を初期化します。
        /// </summary>
        private void InitializeFields()
        {
            noteJudgeTimeSpanList = new float[]
            {
		        // NoteJudge の順番に合わせます。
		        // None は念のため各条件式に当てはまらないように NaN を設定しておきます。
		        float.NaN,
                //絶対値を参照するため2で割る
                0.05f,
                0.1f,
                0.2f,
            };

            noteTimeList = new float[LaneCount][];
            totalNotesPerLaneList = new int[LaneCount];
            judgedNotesCountPerLaneList = new int[LaneCount];
            noteJudgeResultsList = new int[LaneCount][];
            totalJudgedNotes = 0;
            RequestSerialization();
            judgesCountList = new int[NoteJudgeKindCount];
            noteCountList = new int[LaneCount];
        }

        /// <summary>
        /// 各レーン毎のノート数を譜面データから算出します。
        /// </summary>
        private void CountNotesPerLane()
        {
            totalPlacedNotes = textFileConverter.textDB[1].Length;

            // 先に各ノート毎のノート数だけ得るために全体を読み込みます。
            for (var globalNoteIndex = 0; globalNoteIndex < totalPlacedNotes; globalNoteIndex++)
            {
                var laneIndex = int.Parse(textFileConverter.textDB[3][globalNoteIndex]);
                totalNotesPerLaneList[laneIndex]++;
            }
        }

        /// <summary>
        /// ノートカウントに依存するフィールドを初期化します。
        /// </summary>
        private void InitializeFieldsRelatedNotesCount()
        {
            for (var laneIndex = 0; laneIndex < LaneCount; laneIndex++)
            {
                var totalNotesPerLane = totalNotesPerLaneList[laneIndex];
                noteTimeList[laneIndex] = new float[totalNotesPerLane];
                noteJudgeResultsList[laneIndex] = new int[totalNotesPerLaneList[laneIndex]];
            }
        }

        /// <summary>
        /// 各ノートの判定タイミングを算出します。
        /// </summary>
        private void CalculateNoteTimings()
        {
            for (var globalNoteIndex = 0; globalNoteIndex < totalPlacedNotes; globalNoteIndex++)
            {
                // レーン番号と時刻を取得します。
                var laneIndex = int.Parse(textFileConverter.textDB[3][globalNoteIndex]);
                var time = float.Parse(textFileConverter.textDB[1][globalNoteIndex]);

                noteTimeList[laneIndex][noteCountList[laneIndex]] = time + (settingsMng.timingAdjust * 0.01f);
                noteCountList[laneIndex]++;

            }
        }

        // -----------------------------------------------------------------------------
        // 判定系統
        // -----------------------------------------------------------------------------

        /// <summary>
        /// 指定したレーンに対して判定を行います。
        /// </summary>
        public int Judge(int laneIndex)
        {
            //Debug.Log("[<color=yellow>NotesJudger_V2</color>]開始:Judge");
            // すでに全てのノートが判定済みであれば、None にします。
            //Debug.Log("[<color=yellow>NotesJudger_V2</color>]レーン" + laneIndex);
            //Debug.Log("[<color=yellow>NotesJudger_V2</color>]判定済みノーツ:" + judgedNotesCountPerLaneList[laneIndex]);
            var judgedNotesCount = judgedNotesCountPerLaneList[laneIndex];
            if (judgedNotesCount >= totalNotesPerLaneList[laneIndex])
            {
                return NoteJudge[0];
            }

            // 現在時刻と Happy 判定時刻から、時間の差を求めます。
            var currentTime = playManager.playTime;
            var noteTime = noteTimeList[laneIndex][judgedNotesCount];
            var distance = currentTime - noteTime;
            //Debug.Log("[<color=yellow>NotesJudger_V2</color>]時間差:" + (currentTime - noteTime));

            // 各判定に対して、判定幅が小さい順に試行します。
            // NoteJudge.None を 0 番にしているので、judgeKindIndex は 1 から始めます。
            var judgeResult = NoteJudge[0];
            for (var judgeKindIndex = 1; judgeKindIndex < NoteJudgeKindCount; judgeKindIndex++)
            {
                var span = noteJudgeTimeSpanList[judgeKindIndex];
                if (Mathf.Abs(distance) <= span)
                {
                    judgeResult = NoteJudge[judgeKindIndex];
                    break;
                }
            }

            // 何かしらの判定が起きた場合は各種カウンターの値を増やします。
            if (judgeResult != NoteJudge[0])
            {
                syncManager.targetlane = laneIndex;
                syncManager.targetid_a = judgedNotesCount;
                syncManager.targetid_b = judgeResult;
                //noteJudgeResultsList[laneIndex][judgedNotesCount] = judgeResult;
                RequestSerialization();
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetResultsList");
                judgedNotesCountPerLaneList[laneIndex]++;
                judgesCountList[judgeResult]++;
                totalJudgedNotes++;
                RequestSerialization();
            }
            //Debug.Log("[<color=yellow>NotesJudger_V2</color>]完了:Judge");
            return judgeResult;
        }

        /// <summary>
        /// 全てのノートが判定済みかを確認します。
        /// </summary>
        public bool IsAllNotesJudged()
        {
            return totalJudgedNotes >= totalPlacedNotes;
        }

        /// <summary>
        /// ミス判定処理
        /// </summary>
        public void Judge_miss(int laneIndex, int noteNum)
        {
            syncManager.targetlane = laneIndex;
            syncManager.targetid_a = noteNum;
            syncManager.targetid_b = 3;
            RequestSerialization();
            //noteJudgeResultsList[laneIndex][noteNum] = 3;
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetResultsList");
            judgedNotesCountPerLaneList[laneIndex]++;
            judgesCountList[3]++;
            totalJudgedNotes++;
            RequestSerialization();
            gameManager.Judge_miss(laneIndex);
        }

        /// <summary>
        /// ノーツ判定値を更新します
        /// </summary>
        public void SetResultsList()
        {
            noteJudgeResultsList[syncManager.targetlane][syncManager.targetid_a] = syncManager.targetid_b;
        }
    }
}