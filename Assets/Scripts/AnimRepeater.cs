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

    public void SetEvent()
    {
        uiMng.AnimEnd(taskValue);
    }
}
