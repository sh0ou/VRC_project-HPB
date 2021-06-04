using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace HPB
{
    /// <summary>
    /// ScrollRect調整スクリプト
    /// </summary>
    public class ScrollRectAdjuster : UdonSharpBehaviour
    {
        void Start() { GetComponent<ScrollRect>().verticalNormalizedPosition = 1.0f; }
    }
}