using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// ノーツオブジェクト
    /// </summary>
    public class NotesObj : UdonSharpBehaviour
    {
        private NotesJudger_V2 notesJudger;
        private NotesGenerator notesGenerator;
        private SettingsManager settingsMng;
        private PlayManager playMng;
        private MeshRenderer meshRenderer;
        private Vector2 vec2; //位置更新用Vector2

        [SerializeField, Tooltip("ノーツの参照番号\n0=レーン\n1=要素番号(レーン内)\n2=要素番号(単体)")]
        public int[] notesReferenceNo = new int[3];

        void Start()
        {
            notesJudger = GameObject.Find("NotesJudger").GetComponent<NotesJudger_V2>();
            notesGenerator = GameObject.Find("NotesGenerator").GetComponent<NotesGenerator>();
            settingsMng = GameObject.Find("GameManager").GetComponent<SettingsManager>();
            playMng = GameObject.Find("GameManager").GetComponent<PlayManager>();
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }
        void Update()
        {
            if (settingsMng.gamePlay)
            {
                //sad判定の範囲外に出たらミスとして判定
                if (playMng.playTime > notesJudger.noteTimeList[notesReferenceNo[0]][notesReferenceNo[1]] +
                   notesJudger.noteJudgeTimeSpanList[3])
                {
                    //Debug.Log("[<color=yellow>NotesObj</color>]Miss判定:" + gameObject.name);
                    notesJudger.Judge_miss(notesReferenceNo[0], notesReferenceNo[1]);
                }
                //判定済みの場合は消去
                if (notesJudger.noteJudgeResultsList[notesReferenceNo[0]][notesReferenceNo[1]] != 0)
                {
                    //Debug.Log("[<color=yellow>NotesObj</color>]判定が確認されました:" + gameObject.name);
                    if (notesGenerator.notesObjInstance[notesReferenceNo[2] + 10] != null)
                    {
                        notesGenerator.notesObjInstance[notesReferenceNo[2] + 10].GetComponent<MeshRenderer>().enabled = true;
                    }
                    gameObject.SetActive(false);
                }
                //位置を更新
                //Debug.Log("レーン更新を開始:" + notesReferenceNo[0] + "/" + notesReferenceNo[1]);
                if (meshRenderer.enabled)
                {
                    vec2 = notesGenerator.GetNotesPosValue_xy(notesReferenceNo[0]);
                    float f = notesGenerator.GetNotesPosValue_z(notesReferenceNo[0], notesReferenceNo[1]);
                    transform.position = new Vector3(vec2.x, vec2.y, f);
                }
            }
        }
    }
}