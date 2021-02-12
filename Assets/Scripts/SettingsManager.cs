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

        [SerializeField, Tooltip("表示されている画面番号\n0=タイトル\n1=選曲\n2=レベル選択\n3=プレイ\n4=リザルト")]
        public int windowFlag;

        [SerializeField, Tooltip("リズムゲーム進行フラグ")]
        public bool gamePlay;

        #endregion
        #region オプション変数
        [SerializeField, Tooltip("BGM音量")]
        private int bgmVol;

        [SerializeField, Tooltip("効果音音量")]
        private int seVol;

        [SerializeField, Tooltip("ノーツスピード")]
        public int notesSpeed;

        [SerializeField, Tooltip("判定調整値")]
        public float judgeAdjust;

        [SerializeField, Tooltip("エフェクト表示フラグ")]
        public bool effectFlag;

        [SerializeField, Tooltip("デバッグフラグ")]
        private bool debugFlag;

        #endregion
        #region UIオブジェクト
        [SerializeField, Tooltip
        ("UIオンオフ用Obj\n0=オン時\n1=オフ時")]
        private GameObject[] opUIObj;

        [SerializeField, Tooltip
            ("各設定スライダーUI\n0=BGM\n1=SE\n2=スピード\n3=判定調整値")]
        private Slider[] opSliders = new Slider[4];

        [SerializeField, Tooltip
            ("値更新用TMP\n0=bgm\n1=se\n2=speed\n3=Adjust")]
        private TextMeshProUGUI[] opValueText;

        [SerializeField, Tooltip
            ("各設定トグルUI\n0=エフェクト\n1=デバッグ")]
        private Toggle[] opToggles = new Toggle[2];

        #endregion
        void Start()
        {
            //初期値設定
            bgmVol = 5;
            seVol = 5;
            notesSpeed = 1;
            judgeAdjust = 0;
            gamePlay = false;
            debugFlag = false;
            effectFlag = true;
            SetUIActive(true);
        }
        private void Update()
        {
            SetOptionValue();
        }

        /// <summary>
        /// オプション設定を反映させます
        /// </summary>
        public void SetOptionValue()
        {
            //スライダーの値を反映
            bgmVol = (int)opSliders[0].value;
            seVol = (int)opSliders[1].value;
            notesSpeed = (int)opSliders[2].value;
            if(opSliders[3].value == 0)
            {
                judgeAdjust = 0;
            }
            else
            {
                judgeAdjust = (int)opSliders[3].value * 0.01f;
            }
            soundMng.audioSources[0].volume = bgmVol;
            soundMng.audioSources[1].volume = seVol;
            //スライダーの値を表示
            opValueText[0].text = bgmVol.ToString();
            opValueText[1].text = seVol.ToString();
            opValueText[2].text = notesSpeed.ToString();
            opValueText[3].text = judgeAdjust.ToString();
            //トグルを反映
            effectFlag = opToggles[0].isOn;
            debugFlag = opToggles[1].isOn;
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
    }
}