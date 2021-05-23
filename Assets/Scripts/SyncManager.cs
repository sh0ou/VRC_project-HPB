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
        [SerializeField] GameManager gameManager;
        [SerializeField] SettingsManager settingsManager;
        [SerializeField] PlayManager playManager;
        [SerializeField, Tooltip("自分がプレイヤーなのか")]
        public bool isActivePlayer;
        [Header("デバッグモード"), SerializeField]
        private bool isDebugMode;

        void Start()
        {
            if (isDebugMode) { Debug.Log("[<color=red>SyncManager</color>]デバッグモードが有効になってます"); }
            isActivePlayer = isDebugMode ? true : (Networking.IsInstanceOwner ? true : false);

        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (player == Networking.LocalPlayer)
            {
                //プレイ,リザルト,難易度画面の場合、Wait画面を表示させる（同期ズレのため）

            }
        }

        private void Update()
        {
            if (!isDebugMode)
            {
                text_playerName.text = Networking.GetOwner(stickObj[0].gameObject).displayName;
                text_playerMode.text = Networking.LocalPlayer.IsUserInVR() ? "VR Mode" : "Desktop Mode";
                isActivePlayer = Networking.GetOwner(stickObj[0].gameObject) == Networking.LocalPlayer ? true : false;
                if (settingsManager.gamePlay)
                {
                    stickObj[0].pickupable = Networking.GetOwner(stickObj[0].gameObject) == Networking.LocalPlayer ? true : false;
                    stickObj[1].pickupable = Networking.GetOwner(stickObj[1].gameObject) == Networking.LocalPlayer ? true : false;
                }
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