using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// ゲームプレイ時に必要な変数を格納するスクリプト
    /// </summary>
    public class PlayManager : UdonSharpBehaviour
    {
        #region 変数
        //[SerializeField] GameObject drumStick_L;
        //[SerializeField] GameObject drumStick_R;
        //[SerializeField] Transform stickpos_L;
        //[SerializeField] Transform stickpos_R;

        private GameManager gameManager;
        private SyncManager syncManager;
        private SettingsManager settingsManager;
        public float playTime;

        [SerializeField, Tooltip("楽曲スコア"), UdonSynced]
        public int score_now;

        [SerializeField, Tooltip("総ノーツ数")]
        public int notesValue;

        [SerializeField, Tooltip("チェイン数"), UdonSynced]
        public int chain;

        [SerializeField, Tooltip("終了時間")]
        public float endTime;

        [SerializeField, Tooltip
            ("ノーツ判定数\n0=Happy\n1=Good\n2=Sad\n3=Miss"), UdonSynced]
        public int[] judgedValue = new int[4];

        [SerializeField, Tooltip
            ("スコア計算用値\n0=総ノーツ数\n1=1ノーツあたりのスコア")]
        public int[] scoreCalcValue = new int[2];

        [SerializeField, Tooltip("フルチェインフラグ"), UdonSynced]
        public bool fcFlag;

        [SerializeField, Tooltip("オールハッピーフラグ"), UdonSynced]
        public bool ahFlag;
        #endregion

        void Start()
        {
            playTime = 0;
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            syncManager = GameObject.Find("GameManager").GetComponent<SyncManager>();
            settingsManager = GameObject.Find("GameManager").GetComponent<SettingsManager>();
        }

        //public void RespawnStick()
        //{
        //    drumStick_L.transform.position = stickpos_L.position;
        //    drumStick_L.transform.rotation = stickpos_L.rotation;
        //    drumStick_R.transform.position = stickpos_R.position;
        //    drumStick_R.transform.rotation = stickpos_R.rotation;
        //    RequestSerialization();
        //}

        //public GameObject GetStickOwner()
        //{
        //    return drumStick_L;
        //}

        public void StopPlay()
        {
            score_now = 0;
            chain = 0;
            judgedValue = new int[4] { 0, 0, 0, 0 };
            fcFlag = false;
            ahFlag = false;
            RequestSerialization();
            gameManager.EndMusic();
        }
    }
}