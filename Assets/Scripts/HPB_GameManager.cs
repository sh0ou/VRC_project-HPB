using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// ゲーム処理用スクリプト
/// </summary>
public class HPB_GameManager : UdonSharpBehaviour
{
    #region 変数
    [SerializeField, Tooltip("設定マネージャ")]
    private HPB_SettingsManager settingScr;

    [SerializeField, Tooltip("UIマネージャ")]
    private HPB_UIManager uiScr;

    [SerializeField, Tooltip("テキスト変換スクリプト")]
    private TextFileConverter txtConverter;

    [SerializeField, Tooltip("ドラム処理フラグ（falseだと判定が無視される）")]
    private bool drumActive;

    #endregion
    void Start()
    {
        drumActive = true;
    }

    /// <summary>
    /// ドラム処理
    /// </summary>
    /// <param name="i"></param>
    public void DrumAction(int i)
    {
        #region 処理メモ
        /*
        ドラム番号取得
        ゲーム状態取得
        タイトル画面の場合
            タイトルアニメ再生
        選曲画面の場合
            曲移動処理
        レベル選択画面の場合
            レベル移動処理、譜面生成処理
        プレイ画面の場合
            判定処理
        リザルト画面の場合
            選曲画面遷移処理
        */
        #endregion
        //処理待ち対策
        if (drumActive)
        {
            switch (settingScr.windowFlag)
            {
                case 0:
                    //High時のみ
                    if (i == 0)
                    {
                        drumActive = false;
                        SetMusicData();
                        uiScr.GameStart();
                    }
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    Debug.LogError("[<color=red>E100</color>]画面番号が不正です");
                    break;
            }
        }
    }

    /// <summary>
    /// ドラム処理を可能にする
    /// </summary>
    public void DrumActive()
    {
        drumActive = true;
    }

    /// <summary>
    /// 譜面基本情報をセット
    /// </summary>
    public void SetMusicData()
    {
        txtConverter.SetTextFile(1,1);
    }
}