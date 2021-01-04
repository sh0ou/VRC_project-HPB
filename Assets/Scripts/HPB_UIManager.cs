using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// ゲームプレイ時に必要なUIを格納するスクリプト
/// </summary>
public class HPB_UIManager : UdonSharpBehaviour
{
    /*メモ
    色は0~1の小数で指定　255指定はできない
    bottomが下　topが上
    */

    #region 共通
    [Header("共通")]
    [SerializeField, Tooltip("ゲームマネージャ")]
    private HPB_GameManager gameManager;

    [SerializeField, Tooltip
        ("ドラムガイドUI\n0=左UI_選曲\n1=左UI_レベル\n2=右UI=選曲\n3=右UI_レベル")]
    private GameObject[] guideUI;

    [SerializeField, Tooltip
        ("ジャケット格納変数\n1列目=曲番号\n2列目=レベル番号")]
    public Material[][] jacketList;

    #endregion
    #region ウインドウ（アニメーションに使用）
    [SerializeField, Tooltip
        ("ウインドウアニメーター\n0=タイトル\n1=選択1\n2=選択2\n3=プレイ1\n4=プレイ2\n5=リザルト")]
    private Animator[] windowAnims;

    #endregion
    #region 選曲画面（値更新に使用）
    [Header("選曲画面1")]
    [SerializeField, Tooltip
        ("値更新用UI\n0=タイトル\n1=アーティスト")]
    public InputField[] inputFields_select_1;

    [SerializeField, Tooltip
        ("選曲画面1ジャケット画像\n0=中\n1=左\n2=右")]
    public GameObject[] jacketImage_select_1;

    [Header("選曲画面2")]
    [SerializeField, Tooltip
        ("値更新用UI\n0=タイトル\n1=アーティスト\n2=レベル1\n3=レベル2\n4=レベル3")]
    public InputField[] inputFields_select_2;

    [SerializeField, Tooltip("選曲画面2ジャケット画像")]
    public GameObject jacketImage_select_2;

    #endregion
    #region プレイ画面（値,色更新に使用）
    [Header("プレイ画面1")]
    [SerializeField, Tooltip
        ("値更新用UI\n0=タイトル\n1=アーティスト\n2=レベル\n3=レベル種類")]
    public InputField[] inputFields_play_1;

    [SerializeField, Tooltip
        ("色変更用UI\n0=レベル種類\n1=レベル値")]
    private Animator[] fixedText_play_1;

    [Header("プレイ画面2")]
    [SerializeField, Tooltip
        ("値更新用UI\n0=チェイン\n1=スコア")]
    public InputField[] inputFields_play_2;

    //TMP指定できないのでAnimator
    //（プリセットどれかわからないので直接指定）
    [SerializeField, Tooltip
        ("色変更用UI\n0=チェイン値\n1=スコア値")]
    private Animator[] fixedText_play_2;

    #endregion
    #region リザルト画面（値,色更新に使用）
    [Header("リザルト画面")]
    [SerializeField, Tooltip("リザルト画面ジャケット画像")]
    public GameObject jacketImage_result;

    [SerializeField, Tooltip("プレイヤー名")]
    public Text text_playerName;

    [SerializeField, Tooltip
        ("値更新用UI\n0=ランク\n1=スコア\n2=H判定\n3=G判定\n4=S判定\n5=M判定")]
    public InputField[] valueFields_result;

    [SerializeField, Tooltip
        ("色更新用UI\n0=ランク\n1=スコア値")]
    private Animator[] fixedText_result;

    #endregion
    void Start()
    {

    }
    #region アニメーション処理関数
    /// <summary>
    /// タイトル画面クローズ
    /// </summary>
    public void Close_title()
    {
        windowAnims[0].Play("close_title");
    }

    /// <summary>
    /// 選曲画面クローズ
    /// </summary>
    public void Close_select1()
    {
        windowAnims[1].Play("close_select_1");
    }

    /// <summary>
    /// レベル選択画面クローズ
    /// </summary>
    public void Close_select2()
    {
        windowAnims[2].Play("close_select_2");
        guideUI[0].SetActive(false);
        guideUI[1].SetActive(false);
        guideUI[2].SetActive(false);
        guideUI[3].SetActive(false);
    }

    /// <summary>
    /// 選曲画面に戻る
    /// </summary>
    public void Close_select2_return()
    {
        windowAnims[2].Play("close_select_2r");
    }

    /// <summary>
    /// プレイ画面クローズ
    /// </summary>
    public void Close_play()
    {
        windowAnims[3].Play("close_play_1");
        windowAnims[4].Play("close_play_2");
    }

    /// <summary>
    /// リザルト画面クローズ
    /// </summary>
    public void Close_result()
    {
        windowAnims[5].Play("close_result");
    }

    /// <summary>
    /// 選曲画面1
    /// </summary>
    public void Open_select1()
    {
        windowAnims[1].Play("open_select_1");
        guideUI[0].SetActive(true);
        guideUI[1].SetActive(false);
        guideUI[2].SetActive(true);
        guideUI[3].SetActive(false);
    }

    /// <summary>
    /// 選曲画面2
    /// </summary>
    public void Open_select2()
    {
        windowAnims[2].Play("open_select_2");
        guideUI[0].SetActive(false);
        guideUI[1].SetActive(true);
        guideUI[2].SetActive(false);
        guideUI[3].SetActive(true);
    }

    public void Open_play()
    {
        windowAnims[3].Play("open_play_1");
        windowAnims[4].Play("open_play_2");
    }

    public void Open_result()
    {
        windowAnims[5].Play("open_result");
    }
    #endregion

    /// <summary>
    /// TMP色変更
    /// </summary>
    public void TMPAnim_lv(int i)
    {
        switch (i)
        {
            case 0:
                fixedText_play_1[0].Play("TMPC_Level_1");
                fixedText_play_1[1].Play("TMPC_Level_1");
                break;
            case 1:
                fixedText_play_1[0].Play("TMPC_Level_2");
                fixedText_play_1[1].Play("TMPC_Level_2");
                break;
            case 2:
                fixedText_play_1[0].Play("TMPC_Level_3");
                fixedText_play_1[1].Play("TMPC_Level_3");
                break;
        }
    }

    /// <summary>
    /// TMPスコア,チェイン色変更
    /// </summary>
    /// <param name="i">ランク</param>
    /// <param name="b_ah">AllHappyフラグ</param>
    /// <param name="b_fc">FullChainフラグ</param>
    public void TMPAnim_value(int i, bool b_ah, bool b_fc)
    {
        //スコア色設定
        switch (i)
        {
            case 0:
                fixedText_play_2[1].Play("TMPC_Rank_D");
                break;
            case 1:
                fixedText_play_2[1].Play("TMPC_Rank_C");
                break;
            case 2:
                fixedText_play_2[1].Play("TMPC_Rank_B");
                break;
            case 3:
                fixedText_play_2[1].Play("TMPC_Rank_A");
                break;
            case 4:
                fixedText_play_2[1].Play("TMPC_Rank_S");
                break;
        }
        //チェイン色設定
        if (b_ah)
        {
            fixedText_play_2[0].Play("TMPC_Rank_S");
        }
        else if (b_fc)
        {
            fixedText_play_2[0].Play("TMPC_FC");
        }
        else
        {
            fixedText_play_2[0].Play("TMPC_clear");
        }
    }

    /// <summary>
    /// アニメーション終了後処理
    /// </summary>
    public void AnimEnd(int i)
    {
        switch (i)
        {
            case 0://アクティブ状態切替
                gameManager.DrumActive();
                break;
            case 1://選曲画面オープン
                Open_select1();
                break;
            case 2://レベル画面オープン
                Open_select2();
                break;
            case 3://プレイ画面オープン
                Open_play();
                break;
            case 4://リザルト画面オープン
                Open_result();
                break;
            default:
                Debug.LogError("[<color=red>E103</color>]アニメ後処理値が不正です");
                break;
        }
    }
}