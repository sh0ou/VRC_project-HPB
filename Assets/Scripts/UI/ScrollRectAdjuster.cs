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
        ScrollRect scrollRect;
        void Start()
        {
            scrollRect = GetComponent<ScrollRect>();
            PosUpdate();
        }
        public void PosUpdate()
        {
            //GetComponent<ScrollRect>().verticalNormalizedPosition = 1.0f;
            scrollRect.velocity = new Vector2(0f, -10000f);
        }
    }
}