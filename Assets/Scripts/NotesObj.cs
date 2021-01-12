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
        #region archive
        //[SerializeField, Tooltip("ノーツの生存時間")]
        //public float lifeTime;

        //[SerializeField, Tooltip("判定時間")]
        //public float judgeTime;

        //private HPB_SettingsManager settingsMng;

        //private HPB_NotesJudger notesJudger;
        //void Start()
        //{
        //    judgeTime = lifeTime;
        //    notesJudger = GameObject.Find("NotesJudger").GetComponent<HPB_NotesJudger>();
        //    settingsMng = GameObject.Find("GameManager").GetComponent<HPB_SettingsManager>();
        //}
        //private void Update()
        //{
        //    //ゲーム中のみ動かす
        //    if (settingsMng.gamePlay)
        //    {
        //        if()
        //        transform.position += transform.forward * 3 * settingsMng.notesSpeed * Time.deltaTime;
        //        lifeTime -= Time.deltaTime;
        //        if (lifeTime <= 0.0f)
        //        {
        //            Destroy(gameObject);
        //        }
        //    }
        //}
        #endregion
        private NotesJudger_V2 notesJudger;
        private SettingsManager settingsMng;
        private PlayManager playMng;

        [SerializeField, Tooltip("0=レーン\n1=要素番号")]
        public int[] notesValue = new int[2];

        void Start()
        {
            notesJudger = GameObject.Find("NotesJudger").GetComponent<NotesJudger_V2>();
            settingsMng = GameObject.Find("GameManager").GetComponent<SettingsManager>();
            playMng = GameObject.Find("GameManager").GetComponent<PlayManager>();
        }
        void Update()
        {
            if (settingsMng.gamePlay)
            {
                //sad判定の範囲外に出たらミスとして判定
                if (playMng.playTime > notesJudger.noteTimeList[notesValue[0]][notesValue[1]] +
                   notesJudger.noteJudgeTimeSpanList[3])
                {
                    Debug.Log("[<color=yellow>NotesObj</color>]Miss判定:" + gameObject.name);
                    notesJudger.Judge_miss(notesValue[0], notesValue[1]);
                }
                //判定済みの場合は消去
                if (notesJudger.noteJudgeResultsList[notesValue[0]][notesValue[1]] != 0)
                {
                    Debug.Log("[<color=yellow>NotesObj</color>]判定が確認されました:" + gameObject.name);
                    Destroy(gameObject);
                }
                transform.position += transform.forward * 3 * settingsMng.notesSpeed * Time.deltaTime;
            }
        }
    }
}