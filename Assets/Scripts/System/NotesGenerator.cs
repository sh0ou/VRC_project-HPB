﻿using UdonSharp;
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
        private SettingsManager settingsMng;

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

        [SerializeField, Tooltip("ノーツ設定変更用")]
        public GameObject[] notesObjInstance;

        [SerializeField, Tooltip("ノーツカウント用")]
        public int[] noteCountList;

        [SerializeField, Tooltip("メッシュObj")]
        private Mesh[] notesMeshs;

        [SerializeField, Tooltip("ノーツ色設定用マテリアル")]
        private Material[] notesMaterials;

        void Start()
        {
            //Obj非表示化
            for (int i = 0; i < notesRootObj.transform.childCount; i++)
            {
                notesRootObj.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// オブジェクトをアタッチします
        /// </summary>
        /// <param name="value">アタッチする数</param>
        void AttachObj(int value)
        {
            for (int i1 = 0; i1 < value; i1++)
            {
                notesObjInstance[i1] = notesRootObj.transform.GetChild(i1).gameObject;
            }
        }

        #endregion
        /// <summary>ノーツを生成</summary>
        public void GenerateNotes()
        {
            noteCountList = new int[notesJudger.LaneCount];

            notesObjInstance = new GameObject[textFileConverter.textDB[1].Length];

            AttachObj(textFileConverter.textDB[1].Length);

            Debug.Log("ノーツ位置生成を開始します...");
            Debug.Log("生成数:" + textFileConverter.textDB[1].Length);

            //for = 条件が続く限り続行するやつ
            for (int i = 0; i < textFileConverter.textDB[1].Length; i++)
            {
                notesObjInstance[i].SetActive(true);

                //値宣言　判定時間を参照,設定
                int notesLane = int.Parse(textFileConverter.textDB[3][i]);
                float notesPosx = 0;
                float notesPosy = 1.5f;
                float notesPosz = (endPosition.position.z +
                    (notesJudger.noteTimeList[notesLane][noteCountList[notesLane]] * (settingsMng.notesSpeed * 5)));
                #region デバッグ用
                //Debug.Log("ノーツNo." + i);
                //Debug.Log("終点位置:" + endPosition.position.z);
                //Debug.Log("テスト:" + textFileConverter.textDB[1][i]);
                //Debug.Log("DB数値:" + float.Parse(textFileConverter.textDB[1][i]));
                //Debug.Log("再生時間:" + soundManager.playTime);
                //Debug.Log("スピード:" + hpb_GM.notesSpeed);
                //Debug.Log("[<color=yellow>NotesGenerator</color>]NoteTime:" + notesJudger.noteTimeList[notesLane][noteCountList[notesLane]]);
                #endregion

                //ノーツ回転,形を設定
                Vector2 vec2 = GetNotesPosValue_xy(notesLane);
                notesPosx = vec2.x;
                notesPosy = vec2.y;
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

                notesObjInstance[i] = notesRootObj.transform.GetChild(i).gameObject;
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
                //ノーツの参照番号をセット
                notesObjInstance[i].GetComponent<NotesObj>().notesReferenceNo = new int[3]
                {
                    notesLane, noteCountList[notesLane],i
                };
                notesObjInstance[i].SetActive(true);
                //(負荷軽減のため)10ノーツ以降の位置更新処理を無効化させる
                if (i > 9)
                {
                    notesObjInstance[i].GetComponent<MeshRenderer>().enabled = false;
                }

                noteCountList[notesLane]++;
            }
            playMng.notesValue = textFileConverter.textDB[1].Length;
            notesRootObj.SetActive(true);
            Debug.Log("[<color=green>NotesGenerator</color>]ノーツ生成が完了しました");
        }

        /// <summary>
        /// ルートオブジェクトを非表示
        /// </summary>
        public void DisableRootObj()
        {
            notesRootObj.SetActive(false);
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
            float f = settingsMng.drumHeight * 0.1f;
            switch (notesLane)
            {
                case 0:
                    vec2 = new Vector2(-0.45f, f + 1f);
                    break;
                case 5:
                    vec2 = new Vector2(0.45f, f + 1f);
                    break;
                case 1:
                    vec2 = new Vector2(-0.45f, f - 0.05f);
                    break;
                case 2:
                    vec2 = new Vector2(-0.15f, f - 0.05f);
                    break;
                case 3:
                    vec2 = new Vector2(0.15f, f - 0.05f);
                    break;
                case 4:
                    vec2 = new Vector2(0.45f, f - 0.05f);
                    break;
            }
            if (vec2.x == 9999)
            {
                Debug.LogError("[<color=red>NotesGenerator</color>]ノーツの位置セットに失敗しました");
            }
            return vec2;
        }

        /// <summary>
        /// ノーツの位置を取得
        /// </summary>
        /// <param name="notesLane">レーン番号</param>
        /// <param name="notesNum">ノーツ番号</param>
        /// <returns></returns>
        public float GetNotesPosValue_z(int notesLane, int notesNum)
        {
            return (endPosition.position.z +
                ((notesJudger.noteTimeList[notesLane][notesNum] - playMng.playTime)
                * 3 * settingsMng.notesSpeed));
        }
    }
}