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
            float posy = 0;
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
            if (this.gameObject.name.Contains("JudgeTextsp_"))
            {
                //Debug.Log("fast&slowオブジェクトを生成");
                posy = -15;
            }
            this.transform.localPosition = new Vector3(posx, posy, 0);
            //Debug.Log("生成位置:" + this.transform.localPosition);
        }
        public void AnimEnd()
        {
            Destroy(this.gameObject);
        }
    }
}