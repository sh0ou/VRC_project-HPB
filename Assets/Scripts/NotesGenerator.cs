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
        #region システム変数
        [Header("システム変数")]
        [SerializeField]
        private SettingsManager hpb_GM;

        [SerializeField]
        private PlayManager playMng;

        [SerializeField]
        private TextFileConverter textFileConverter;

        [SerializeField]
        private NotesJudger_V2 notesJudger;

        #endregion
        #region ノーツ変数
        [Header("ノーツ変数")]
        [SerializeField, Tooltip("ノーツ郡のルートオブジェクト")]
        private GameObject notesRootObj;

        [SerializeField, Tooltip("終点の位置")]
        private Transform endPosition;

        //[SerializeField, Tooltip("ノーツオブジェクト")]
        //private GameObject[] notesObj;

        [SerializeField, Tooltip("ノーツ設定変更用")]
        private GameObject[] notesObjInstance;

        [SerializeField, Tooltip("ノーツカウント用")]
        public int[] noteCountList;

        [SerializeField, Tooltip("メッシュObj")]
        private Mesh[] notesMeshs;

        [SerializeField, Tooltip("ノーツ色設定用マテリアル")]
        private Material[] notesMaterials;

        void Start()
        {
            //オブジェクトをアタッチ
            for (int i1 = 0; i1 < notesRootObj.transform.childCount; i1++)
            {
                notesObjInstance[i1] = notesRootObj.transform.GetChild(i1).gameObject;
            }
            //Obj非表示化
            for (int i2 = 0; i2 < notesObjInstance.Length; i2++)
            {
                notesObjInstance[i2].SetActive(false);
            }
        }

        #endregion
        /// <summary>ノーツを生成</summary>
        public void GenerateNotes()
        {
            noteCountList = new int[notesJudger.LaneCount];

            notesObjInstance = new GameObject[textFileConverter.textDB[1].Length];

            Debug.Log("ノーツ位置生成を開始します...");
            Debug.Log("生成数;" + notesObjInstance.Length);

            //for = 条件が続く限り続行するやつ
            for (int i = 0; i < textFileConverter.textDB[1].Length; i++)
            {
                //値宣言　判定時間を参照,設定
                int notesLane = int.Parse(textFileConverter.textDB[3][i]);
                float notesPosx = 0;
                float notesPosy = 1.5f;
                float notesPosz = (endPosition.position.z +
                    (notesJudger.noteTimeList[notesLane][noteCountList[notesLane]] * (hpb_GM.notesSpeed * 5)));
                /*2行目の数字は微調整用
                ↑明らかにずれてる（スタート時ノーツとエンド時ノーツがずれてる）*/
                #region デバッグ用
                //Debug.Log("ノーツNo." + i);
                //Debug.Log("終点位置:" + endPosition.position.z);
                //Debug.Log("テスト:" + textFileConverter.textDB[1][i]);
                //Debug.Log("DB数値:" + float.Parse(textFileConverter.textDB[1][i]));
                //Debug.Log("再生時間:" + soundManager.playTime);
                //Debug.Log("スピード:" + hpb_GM.notesSpeed);
                //Debug.Log("[<color=yellow>NotesGenerator</color>]NoteTime:" + notesJudger.noteTimeList[notesLane][noteCountList[notesLane]]);
                #endregion
                #region archive
                //ノーツ生成
                //if (notesLane == 0)
                //{
                //    notesObjInstance.SetValue(VRCInstantiate(notesObj[1]), i);
                //    notesPosy = 2;
                //}
                //else
                //{
                //    notesObjInstance.SetValue(VRCInstantiate(notesObj[0]), i);
                //    notesPosy = 0.5f;
                //}
                #endregion

                notesObjInstance[i] = notesRootObj.transform.GetChild(i).gameObject;
                Vector2 vec2 = GetNotesPosValue_xy(notesLane);
                notesPosx = vec2.x;
                notesPosy = vec2.y;

                //各種値を設定
                switch (notesLane)
                {
                    case 0:
                    case 5:
                        SetNotes(1, i);
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        SetNotes(0, i);
                        break;
                }

                //ノーツ位置を設定
                notesObjInstance[i].gameObject.transform.position = new Vector3(notesPosx, notesPosy, notesPosz);

                #region デバッグ用
                //Debug.Log("ゲームマネージャ:" + hpb_GM);
                //Debug.Log("ノーツスピード:" + hpb_GM.notesSpeed);
                //Debug.Log("ノーツ移動速度計算:" + 0.1f * hpb_GM.notesSpeed);
                //Debug.Log("オブジェクトチェック:" + notesObjInstance[i]);
                //Debug.Log("コンポーネント:" + notesObjInstance[i].GetComponent<NotesObjScr>());
                //Debug.Log("lifetime取得チェック:" + notesObjInstance[i].GetComponent<NotesObjScr>().lifeTime);
                //Debug.Log("Parseチェック" + float.Parse(textFileConverter.textDB[1][i]));
                #endregion
                //ノーツ設定を変更
                notesObjInstance[i].GetComponent<NotesObj>().notesReferenceNo =
                    new int[] { notesLane, noteCountList[notesLane] };
                notesObjInstance[i].SetActive(true);
                noteCountList[notesLane]++;
            }
            playMng.notesValue = notesObjInstance.Length;
            notesRootObj.SetActive(true);
            Debug.Log("<color=green>ノーツ生成が完了しました</color>");
        }
        //public GameObject SendNotesObj(int i)
        //{
        //    return notesObjInstance[i];
        //}

        /// <summary>
        /// ルートオブジェクトを非表示
        /// </summary>
        public void DisableRootObj()
        {
            notesRootObj.SetActive(false);
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
        /// ノーツの位置セット
        /// </summary>
        /// <param name="cat">ノーツ種類</param>
        /// <param name="notesNum">ノーツ番号</param>
        public void SetNotes(int cat, int notesNum)
        {
            //位置設定
            switch (cat)
            {
                case 0:
                    notesObjInstance[notesNum].transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 1:
                    notesObjInstance[notesNum].transform.rotation = Quaternion.Euler(0, -180, 90);
                    break;
            }
            //メッシュ,マテリアル差し替え
            notesObjInstance[notesNum].GetComponent<MeshFilter>().mesh = notesMeshs[cat];
            notesObjInstance[notesNum].GetComponent<Renderer>().material = notesMaterials[cat];
        }

        /// <summary>
        /// ノーツのx,y位置情報を取得
        /// </summary>
        public Vector2 GetNotesPosValue_xy(int notesLane)
        {
            Vector2 vec2 = new Vector2(9999, 9999);
            switch (notesLane)
            {
                case 0:
                    vec2 = new Vector2(-0.7f, 2);
                    break;
                case 5:
                    vec2 = new Vector2(0.7f, 2);
                    break;
                case 1:
                    vec2 = new Vector2(-0.75f, 0.45f);
                    break;
                case 2:
                    vec2 = new Vector2(-0.25f, 0.45f);
                    break;
                case 3:
                    vec2 = new Vector2(0.25f, 0.45f);
                    break;
                case 4:
                    vec2 = new Vector2(0.75f, 0.45f);
                    break;
            }
            if (vec2.x == 9999)
            {
                Debug.LogError("[<color=red>NotesGenerator</color>]ノーツの位置セットに失敗しました");
            }
            return vec2;
        }

        public float GetNotesPosValue_z(int notesLane, int notesNum)
        {
            return (endPosition.position.z +
                ((notesJudger.noteTimeList[notesLane][notesNum] - playMng.playTime)
                * 3 * hpb_GM.notesSpeed));
        }
    }
}