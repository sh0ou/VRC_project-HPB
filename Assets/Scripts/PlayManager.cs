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
        [SerializeField] GameObject drumStick_L;
        [SerializeField] GameObject drumStick_R;
        [SerializeField] Transform stickpos_L;
        [SerializeField] Transform stickpos_R;

        public float playTime;

        [SerializeField, Tooltip("楽曲スコア")]
        public int score_now;

        [SerializeField, Tooltip("総ノーツ数")]
        public int notesValue;

        [SerializeField, Tooltip("チェイン数")]
        public int chain;

        [SerializeField, Tooltip("終了時間")]
        public float endTime;

        [SerializeField, Tooltip
            ("ノーツ判定数\n0=Happy\n1=Good\n2=Sad\n3=Miss")]
        public int[] judgedValue = new int[4];

        [SerializeField, Tooltip
            ("スコア計算用値\n0=総ノーツ数\n1=1ノーツあたりのスコア")]
        public int[] scoreCalcValue = new int[2];

        [SerializeField, Tooltip("フルチェインフラグ")]
        public bool fcFlag;

        [SerializeField, Tooltip("オールハッピーフラグ")]
        public bool ahFlag;
        #endregion

        void Start()
        {
            playTime = 0;
        }

        public void RespawnStick()
        {
            drumStick_L.transform.position = stickpos_L.position;
            drumStick_L.transform.rotation = stickpos_L.rotation;
            drumStick_R.transform.position = stickpos_R.position;
            drumStick_R.transform.rotation = stickpos_R.rotation;
        }
    }
}