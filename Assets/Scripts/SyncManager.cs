using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

namespace HPB
{
    public class SyncManager : UdonSharpBehaviour
    {
        //[/*UdonSynced,*/ SerializeField, Tooltip("現在のプレイ状況\n0.タイトル画面\n1.選曲画面\n2.プレイ中")]
        //int activeWindow;
        [SerializeField, Tooltip("ドラムスティック")]
        VRC_Pickup[] stickObj;
        [SerializeField, Tooltip("プレイヤー名表示UI")]
        TextMeshProUGUI text_playerName;
        [SerializeField, Tooltip("プレイモード表示UI")]
        TextMeshProUGUI text_playerMode;
        [SerializeField] GameManager gameManager;
        [SerializeField] SettingsManager settingsManager;
        [SerializeField] PlayManager playManager;
        [SerializeField] UIManager uIManager;
        [SerializeField] SoundManager soundManager;
        [SerializeField] ParticleGenerator particleGenerator;
        [SerializeField, Tooltip("自分がプレイヤーなのか")]
        public bool isActivePlayer;
        [Header("デバッグモード"), SerializeField]
        public bool isDebugMode;

        [UdonSynced] public int targetlane;//レーン指定用変数
        [UdonSynced] public int targetid_a;//同期用汎用変数1
        [UdonSynced] public int targetid_b;//同期用納用変数2
        [UdonSynced] public int targetTextid_a;//判定テキスト同期用変数1
        [UdonSynced] public int targetTextid_b;//判定テキスト同期用変数2
        [UdonSynced] public bool targetBool;//同期用汎用フラグ

        void Start()
        {
            if (isDebugMode) { Debug.Log("[<color=red>SyncManager</color>]デバッグモードが有効になってます"); }
            isActivePlayer = isDebugMode ? true : (Networking.IsInstanceOwner ? true : false);

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
                    RequestSerialization();
                }
            }
        }

        public override void OnPreSerialization()
        {
            Debug.Log("[<color=blue>SyncManager</color>]値を送信しました");
        }

        public override void OnDeserialization()
        {
            Debug.Log("[<color=green>SyncManager</color>]値を受信しました");
        }

        /// <summary>
        /// Playerを変更します
        /// </summary>
        public void ChangePlayer()
        {
            Networking.SetOwner(Networking.LocalPlayer, stickObj[0].gameObject);
            Networking.SetOwner(Networking.LocalPlayer, stickObj[1].gameObject);
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
            Networking.SetOwner(Networking.LocalPlayer, GameObject.Find("NotesJudger"));
            RequestSerialization();
        }

        public void Anim_Drums()
        {
            uIManager.Anim_Drums(targetlane, targetid_a);
        }

        public void Anim_SimbalAcc()
        {
            uIManager.Anim_SimbalAcc(targetlane, targetid_a);
        }

        public void UIAnim_level()
        {
            uIManager.UIAnim_level(targetid_a);
        }

        public void UIAnim_value()
        {
            uIManager.UIAnim_value(targetBool);
        }

        /// <summary>
        /// (使用ID:A)SEを再生します
        /// </summary>
        public void PlayDrumSE()
        {
            soundManager.audioSources[1].PlayOneShot(soundManager.seLists[targetid_a + 10]);
        }

        public void GenerateParticle()
        {
            particleGenerator.GenerateParticle(targetlane);
        }

        /// <summary>
        /// (使用ID:A,B)判定テキストを生成します
        /// </summary>
        public void GenerateJudgeText()
        {
            GameObject g = VRCInstantiate(uIManager.uiObj_judge[targetTextid_a]);
            g.GetComponent<JudgeTextObj>().judgeValue = targetTextid_b;
        }
    }
}