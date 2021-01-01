using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// ゲーム設定,オプションUI格納スクリプト
/// </summary>
public class HPB_SettingsManager : UdonSharpBehaviour
{
    #region システム変数
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

    [SerializeField, Tooltip("エフェクト表示フラグ")]
    public bool effectFlag;

    [SerializeField, Tooltip("デバッグフラグ")]
    private bool debugFlag;

    #endregion
    #region UIオブジェクト
    [SerializeField, Tooltip("各設定スライダーUI\n0=BGM\n1=SE\n2=スピード")]
    private Slider[] opSliders = new Slider[3];

    [SerializeField, Tooltip("値更新用InputField\n0=bgm\n1=se\n2=speed")]
    private InputField[] opValueFields = new InputField[3];

    [SerializeField, Tooltip("各設定トグルUI\n0=エフェクト\n1=デバッグ")]
    private Toggle[] opToggles = new Toggle[2];

    #endregion
    void Start()
    {
        //初期値設定
        bgmVol = 5;
        seVol = 5;
        notesSpeed = 1;
        gamePlay = false;
        debugFlag = false;
        effectFlag = true;
    }
    private void Update()
    {
        //スライダーの値を反映
        bgmVol = (int)opSliders[0].value;
        seVol = (int)opSliders[1].value;
        notesSpeed = (int)opSliders[2].value;
        //スライダーの値を表示
        opValueFields[0].text = bgmVol.ToString();
        opValueFields[1].text = seVol.ToString();
        opValueFields[2].text = notesSpeed.ToString();
        //トグルを反映
        effectFlag = opToggles[0].isOn;
        debugFlag = opToggles[1].isOn;
    }
}