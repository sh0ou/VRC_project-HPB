using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// ノーツ判定表示オブジェクト用スクリプト
    /// </summary>
    public class JudgeTextObj : UdonSharpBehaviour
    {
        public int judgeValue = 1;

        void Start()
        {
            this.transform.SetParent(GameObject.Find("UI_JudgeText").transform);
            this.transform.localScale = new Vector3(1, 1, 1);
            //判定に応じて出現位置を変更
            float posx = 0;
            switch (judgeValue)
            {
                case 1:
                    posx = -75;
                    break;
                case 2:
                    posx = -25;
                    break;
                case 3:
                    posx = 25;
                    break;
                case 4:
                    posx = 75;
                    break;
            }
            this.transform.localPosition = new Vector3(posx, 0, 0);
        }
        public void AnimEnd()
        {
            Destroy(this.gameObject);
        }
    }
}