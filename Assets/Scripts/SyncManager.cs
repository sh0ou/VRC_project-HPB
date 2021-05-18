using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

namespace HPB
{
    public class SyncManager : UdonSharpBehaviour
    {
        [UdonSynced, SerializeField, Tooltip("現在のプレイ状況\n0.タイトル画面\n1.選曲画面\n2.プレイ中")]
        int activePhase;
        [SerializeField, Tooltip("ドラムスティック")]
        VRC_Pickup[] stickObj;
        [SerializeField, Tooltip("プレイヤー名表示UI")]
        TextMeshProUGUI text_playerName;
        [SerializeField, Tooltip("プレイモード表示UI")]
        TextMeshProUGUI text_playerMode;
        [SerializeField]
        SettingsManager settingsManager;
        [SerializeField]
        PlayManager playManager;

        void Start()
        {

        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            //プレイ,リザルト,難易度画面の場合、Wait画面を表示させる（同期ズレのため）
        }

        private void Update()
        {
            text_playerName.text = Networking.GetOwner(stickObj[0].gameObject).displayName;
            text_playerMode.text = Networking.LocalPlayer.IsUserInVR() ? "VR Mode" : "Desktop Mode";
            if (settingsManager.gamePlay)
            {
                stickObj[0].pickupable = Networking.GetOwner(stickObj[0].gameObject) == Networking.LocalPlayer ? true : false;
                stickObj[1].pickupable = Networking.GetOwner(stickObj[1].gameObject) == Networking.LocalPlayer ? true : false;
            }
        }

        /// <summary>
        /// 画面を同期します
        /// </summary>
        public void SyncWindow()
        {
            //playManager.
        }

        /// <summary>
        /// Playerを変更します
        /// </summary>
        public void ChangePlayer()
        {
            Networking.SetOwner(Networking.LocalPlayer, stickObj[0].gameObject);
            Networking.SetOwner(Networking.LocalPlayer, stickObj[1].gameObject);
        }
    }
}