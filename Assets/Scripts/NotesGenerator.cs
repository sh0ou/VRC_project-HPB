using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// ノーツジェネレータ
    /// </summary>
    public class NotesGenerator : UdonSharpBehaviour
    {
        #region 変数
        [SerializeField]
        private SettingsManager hpb_GM;

        [SerializeField]
        private PlayManager playMng;

        [SerializeField]
        private TextFileConverter textFileConverter;

        [SerializeField]
        private NotesJudger_V2 notesJudger;

        [SerializeField, Tooltip("ノーツオブジェクト")]
        private GameObject[] notesObj;

        [SerializeField, Tooltip("終点の位置")]
        private Transform endPosition;

        [SerializeField, Tooltip("ノーツ設定変更用")]
        private GameObject[] notesObjInstance;

        public int[] noteCountList;
        #endregion
        void Start()
        {
            //textFileConverter.SetTextFile(1);
            //SetNotes();
        }
        /// <summary>
        /// ノーツを生成
        /// </summary>
        public void SetNotes()
        {
            noteCountList = new int[notesJudger.LaneCount];
            float notesPosz;
            Debug.Log("ノーツ位置生成を開始します...");
            notesObjInstance = new GameObject[textFileConverter.textDB[1].Length];
            Debug.Log("生成数;" + notesObjInstance.Length);
            //for = 条件が続く限り続行するやつ
            for (int i = 0; i < textFileConverter.textDB[1].Length; i++)
            {
                int notesLane = int.Parse(textFileConverter.textDB[3][i]);
                float notesPosx = 0;
                float notesPosy = 1.5f;

                #region デバッグ用
                //Debug.Log("ノーツNo." + i);
                //Debug.Log("終点位置:" + endPosition.position.z);
                //Debug.Log("テスト:" + textFileConverter.textDB[1][i]);
                //Debug.Log("DB数値:" + float.Parse(textFileConverter.textDB[1][i]));
                //Debug.Log("再生時間:" + soundManager.playTime);
                //Debug.Log("スピード:" + hpb_GM.notesSpeed);
                //ノーツ位置を計算
                #endregion
                Debug.Log("[<color=yellow>NotesGenerator</color>]NoteTime:" + notesJudger.noteTimeList[notesLane][noteCountList[notesLane]]);
                //判定時間を参照&設定
                notesPosz = (endPosition.position.z +
                    (notesJudger.noteTimeList[notesLane][noteCountList[notesLane]] * 3 * hpb_GM.notesSpeed));
                //2行目の数字は微調整用
                //↑明らかにずれてる（スタート時ノーツとエンド時ノーツがずれてる）
                //Debug.Log("生成位置:" + notesPosz);


                //ノーツ生成
                if (notesLane == 0)
                {
                    notesObjInstance.SetValue(VRCInstantiate(notesObj[1]), i);
                    notesPosy = 2;
                }
                else
                {
                    notesObjInstance.SetValue(VRCInstantiate(notesObj[0]), i);
                    notesPosy = 0.5f;
                }

                //Debug.Log("ノーツObj:" + notesObjInstance[i]);
                switch (notesLane)
                {
                    case 0:
                        notesPosx = 0;
                        break;
                    case 1:
                        notesPosx = -0.75f;
                        break;
                    case 2:
                        notesPosx = -0.25f;
                        break;
                    case 3:
                        notesPosx = 0.25f;
                        break;
                    case 4:
                        notesPosx = 0.75f;
                        break;
                }

                //ノーツ位置を設定
                notesObjInstance[i].gameObject.transform.position = new Vector3(notesPosx, notesPosy, notesPosz);
                //Debug.Log("ノーツ位置:" + notesObjInstance[i].transform.position);

                //ノーツ設定を変更
                #region デバッグ用
                //Debug.Log("ゲームマネージャ:" + hpb_GM);
                //Debug.Log("ノーツスピード:" + hpb_GM.notesSpeed);
                //Debug.Log("ノーツ移動速度計算:" + 0.1f * hpb_GM.notesSpeed);
                //Debug.Log("オブジェクトチェック:" + notesObjInstance[i]);
                //Debug.Log("コンポーネント:" + notesObjInstance[i].GetComponent<NotesObjScr>());
                //Debug.Log("lifetime取得チェック:" + notesObjInstance[i].GetComponent<NotesObjScr>().lifeTime);
                //Debug.Log("Parseチェック" + float.Parse(textFileConverter.textDB[1][i]));
                #endregion

                notesObjInstance[i].GetComponent<NotesObj>().notesValue =
                    new int[] { notesLane, noteCountList[notesLane] };
                noteCountList[notesLane]++;
                //notesObjInstance[i].GetComponent<NotesObj>().notesValue[1] = notesJudger //ここで詰んでいます
                //notesObjInstance[i].GetComponent<HPB_NotesObj>().lifeTime =
                //    float.Parse(textFileConverter.textDB[1][i]) + (0.1f * hpb_GM.notesSpeed);
                //Debug.Log("ノーツ生存時間:" + notesObjInstance[i].GetComponent<NotesObjScr>().lifeTime);
            }
            playMng.notesValue = notesObjInstance.Length;
            Debug.Log("<color=green>ノーツ生成が完了しました</color>");
        }

        public GameObject SendNotesObj(int i)
        {
            return notesObjInstance[i];
        }

        /// <summary>
        /// ノーツ数を返す
        /// </summary>
        /// <returns></returns>
        public int SendNotesValue()
        {
            return notesObjInstance.Length;
        }

        /// <summary>
        /// ノーツのレーンを返す
        /// </summary>
        /// <param name="i">対象要素番号</param>
        /// <returns></returns>
        public int SendNotesLaneNum(int i)
        {
            int i_r = -1;
            switch (notesObjInstance[i].transform.position.x)
            {
                case -0.75f:
                    i_r = 1;
                    break;
                case -0.25f:
                    i_r = 2;
                    break;
                case 0.25f:
                    i_r = 3;
                    break;
                case 0.75f:
                    i_r = 4;
                    break;
                    //Highノーツ用処理をここに
            }
            if (i_r == -1)
            {
                Debug.LogError("[<color=red>NotesJudger</color>]判定結果値が不正です");
            }
            return i_r;
        }
    }
}