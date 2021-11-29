using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

namespace HPB
{
    /// <summary>
    /// ゲーム設定,オプションUI格納スクリプト
    /// </summary>
    public class SettingsManager : UdonSharpBehaviour
    {
        #region システム変数
        [SerializeField, Tooltip("サウンドマネージャ")]
        private SoundManager soundMng;

        [SerializeField]
        private UIManager uiManager;

        [UdonSynced, SerializeField, Tooltip("表示されている画面番号\n0=タイトル\n1=選曲\n2=レベル選択\n3=プレイ\n4=リザルト")]
        public int windowFlag;

        [SerializeField, Tooltip("リズムゲーム進行フラグ")]
        public bool gamePlay;

        [SerializeField, Tooltip("キーボード有効化フラグ")]
        public bool isActiveKeyBoard;

        #endregion
        #region オプション変数
        //ドラム
        [SerializeField, Tooltip("ドラムの高さ")]
        public int drumHeight;

        [SerializeField, Tooltip("ドラムの幅")]
        public int drumWidth;

        [SerializeField, Tooltip("ドラムの大きさ")]
        public int drumSize;

        [SerializeField, Tooltip("ドラムの判定エリア")]
        public int drumJudgeArea;

        [SerializeField, Tooltip("ノーツスピード")]
        public int notesSpeed;

        [SerializeField, Tooltip("判定調整値\n（x0.01）")]
        public int timingAdjust;

        //グラフィック

        [SerializeField, Tooltip("パーティクルアニメーション")]
        public bool isParticleAnimation;

        [SerializeField, Tooltip("判定エフェクト")]
        public bool isHitEffect;

        [SerializeField, Tooltip("シンバルレーンを分割")]
        public bool isSplitSimbalLane;

        //サウンド

        [SerializeField, Tooltip("BGM音量")]
        private int bgmVol;

        [SerializeField, Tooltip("効果音音量")]
        private int seVol;

        //デバッグ

        //[SerializeField, Tooltip("楽曲エフェクト表示フラグ")]
        //public bool effectFlag;

        [SerializeField, Tooltip("デバッグモードフラグ")]
        public bool debugFlag;

        #endregion
        #region UIオブジェクト
        [SerializeField, Tooltip
        ("UIオンオフ用Obj\n0=オン時\n1=オフ時")]
        private GameObject[] opUIObj;

        [SerializeField, Tooltip("注意書きウインドウ")]
        private GameObject cautionWindowObj;

        [SerializeField, Tooltip
            ("各設定スライダーUI\n0=height\n1=width\n2=size\n3=area\n4=speed\n5=adjust\n6=BGM\n7=SE")]
        private Slider[] opSliders = new Slider[12];

        [SerializeField, Tooltip
            ("値更新用TMP\n0=height\n1=width\n2=size\n3=area\n4=speed\n5=adjust\n6=BGM\n7=SE")]
        private TextMeshProUGUI[] opValueText;

        [SerializeField, Tooltip
            ("各設定トグルUI\n0=PA\n1=Drum Effect\n2=Split Lane\n3=Debug")]
        private Toggle[] opToggles = new Toggle[2];

        #endregion
        void Start()
        {
            isActiveKeyBoard = false;
            ResetOption();
            SetUIActive(true);
            gamePlay = false;
        }

        /// <summary>
        /// オプションの値を初期化します
        /// </summary>
        public void ResetOption()
        {
            Debug.Log("値をリセットします");

            //初期値設定、スライダーに値を反映
            drumHeight = 10;
            opSliders[0].value = drumHeight;

            drumWidth = 12;
            opSliders[1].value = drumWidth;

            drumSize = 10;
            opSliders[2].value = drumSize;

            drumJudgeArea = 20;
            opSliders[3].value = drumJudgeArea;

            notesSpeed = 1;
            opSliders[4].value = notesSpeed;

            timingAdjust = 0;
            opSliders[5].value = timingAdjust;


            isParticleAnimation = true;
            opToggles[0].isOn = isParticleAnimation;

            isHitEffect = true;
            opToggles[1].isOn = isHitEffect;

            isSplitSimbalLane = true;
            opToggles[2].isOn = isSplitSimbalLane;


            bgmVol = 5;
            opSliders[6].value = bgmVol;

            seVol = 5;
            opSliders[7].value = seVol;

            debugFlag = false;
            opToggles[3].isOn = debugFlag;

            uiManager.ObjectAdjust();

            //値を表示
            opValueText[0].text = drumHeight.ToString();
            opValueText[1].text = drumWidth.ToString();
            opValueText[2].text = drumSize.ToString();
            opValueText[3].text = drumJudgeArea.ToString();
            opValueText[4].text = notesSpeed.ToString();
            opValueText[5].text = (timingAdjust * 10).ToString() + "ms";
            opValueText[6].text = bgmVol.ToString();
            opValueText[7].text = seVol.ToString();
            soundMng.audioSources[0].volume = bgmVol * 0.1f;
            soundMng.audioSources[1].volume = seVol * 0.1f;
        }

        /// <summary>
        /// オプション設定を反映させます
        /// </summary>
        public void SetOptionValue()
        {
            //オプションの値を反映
            //ドラム
            drumHeight = (int)opSliders[0].value;
            drumWidth = (int)opSliders[1].value;
            drumSize = (int)opSliders[2].value;
            drumJudgeArea = (int)opSliders[3].value;
            notesSpeed = (int)opSliders[4].value;
            timingAdjust = (int)opSliders[5].value;
            //エフェクト
            isParticleAnimation = opToggles[0].isOn;
            isHitEffect = opToggles[1].isOn;
            isSplitSimbalLane = opToggles[2].isOn;
            //サウンド
            bgmVol = (int)opSliders[6].value;
            seVol = (int)opSliders[7].value;
            //デバッグ
            debugFlag = opToggles[3].isOn;

            uiManager.ObjectAdjust();

            //オプションの値を表示
            opValueText[0].text = drumHeight.ToString();
            opValueText[1].text = drumWidth.ToString();
            opValueText[2].text = drumSize.ToString();
            opValueText[3].text = drumJudgeArea.ToString();
            opValueText[4].text = notesSpeed.ToString();
            opValueText[5].text = (timingAdjust * 10).ToString() + "ms";
            opValueText[6].text = bgmVol.ToString();
            opValueText[7].text = seVol.ToString();
            soundMng.audioSources[0].volume = bgmVol * 0.1f;
            soundMng.audioSources[1].volume = seVol * 0.1f;

        }

        /// <summary>
        /// UIのアクティブ状態を切り替えます
        /// </summary>
        /// <param name="b"></param>
        public void SetUIActive(bool b)
        {
            Debug.Log("アクティブ:SetUIActive");
            if (b)
            {
                opUIObj[0].SetActive(true);
                opUIObj[1].SetActive(false);
            }
            else
            {
                opUIObj[0].SetActive(false);
                opUIObj[1].SetActive(true);
            }
        }

        /// <summary>
        /// 注意書きウインドウを閉じて、キーボードを有効化させます
        /// </summary>
        public void CloseCaution()
        {
            cautionWindowObj.SetActive(false);
            if (!Networking.LocalPlayer.IsUserInVR())
            {
                isActiveKeyBoard = true;
            }
        }
    }
}