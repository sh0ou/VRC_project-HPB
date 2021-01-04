using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// アニメーション処理中継スクリプト
/// </summary>
public class AnimRepeater : UdonSharpBehaviour
{
    [SerializeField, Tooltip("UIマネージャ")]
    private HPB_UIManager uiMng;

    [SerializeField, Tooltip("処理番号")]
    private int taskValue;

    /// <summary>
    /// ドラムアクティブ状態を切替
    /// </summary>
    public void SetEvent_0()
    {
        uiMng.AnimEnd(0);
    }

    /// <summary>
    /// 選曲画面を表示
    /// </summary>
    public void SetEvent_1()
    {
        uiMng.AnimEnd(1);
    }

    public void SetEvent()
    {
        uiMng.AnimEnd(taskValue);
    }
}
