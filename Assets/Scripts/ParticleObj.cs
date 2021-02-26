using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// 各種パーティクル処理用スクリプト
    /// </summary>
    public class ParticleObj : UdonSharpBehaviour
    {
        [Tooltip("0-5=判定ライン\n10-15=ドラム\n90=リザルト")]
        public int catValue = 1;

        void Start()
        {
            //判定に応じて出現位置を変更
            float posx = 0;
            float posy = -20;
            float posz = 9999;
            switch (catValue)
            {
                case 0:
                    this.transform.SetParent(GameObject.Find("UI_JudgeText").transform);
                    posx = -75;
                    posy = 130;
                    break;
                case 1:
                    this.transform.SetParent(GameObject.Find("UI_JudgeText").transform);
                    posx = -75;
                    posy = -20;
                    break;
                case 2:
                    this.transform.SetParent(GameObject.Find("UI_JudgeText").transform);
                    posx = -25;
                    posy = -20;
                    break;
                case 3:
                    this.transform.SetParent(GameObject.Find("UI_JudgeText").transform);
                    posx = 25;
                    posy = -20;
                    break;
                case 4:
                    this.transform.SetParent(GameObject.Find("UI_JudgeText").transform);
                    posx = 75;
                    posy = -20;
                    break;
                case 5:
                    this.transform.SetParent(GameObject.Find("UI_JudgeText").transform);
                    posx = 75;
                    posy = 130;
                    break;
                case 10:
                    this.transform.SetParent(GameObject.Find("DrumSet").transform);
                    posx = -0.5f;
                    posy = 0.2f;
                    posz = 0.2f;
                    this.transform.localRotation = Quaternion.Euler(-110, -30, 0);
                    break;
                case 11:
                    this.transform.SetParent(GameObject.Find("DrumSet").transform);
                    posx = -0.6f;
                    posy = 0;
                    posz = -0.1f;
                    break;
                case 12:
                    this.transform.SetParent(GameObject.Find("DrumSet").transform);
                    posx = -0.2f;
                    posy = 0;
                    posz = 0.1f;
                    break;
                case 13:
                    this.transform.SetParent(GameObject.Find("DrumSet").transform);
                    posx = 0.2f;
                    posy = 0;
                    posz = 0.1f;
                    break;
                case 14:
                    this.transform.SetParent(GameObject.Find("DrumSet").transform);
                    posx = 0.6f;
                    posy = 0;
                    posz = -0.1f;
                    break;
                case 15:
                    this.transform.SetParent(GameObject.Find("DrumSet").transform);
                    posx = 0.5f;
                    posy = 0.2f;
                    posz = 0.2f;
                    this.transform.localRotation = Quaternion.Euler(-110, 30, 0);
                    break;
                case 90:
                    posx = 0;
                    posy = 0.5f;
                    posz = 25;
                    break;
            }

            this.transform.localScale = new Vector3(1, 1, 1);
            this.transform.localPosition =
                posz == 9999
                ? new Vector3(posx, posy, 0)
                : new Vector3(posx, posy, posz);
            ParticleSystem p = this.GetComponent<ParticleSystem>();
            p.Play();
        }
    }
}