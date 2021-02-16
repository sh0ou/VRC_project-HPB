using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// パーティクル生成スクリプト
    /// </summary>
    public class ParticleGenerator : UdonSharpBehaviour
    {
        [SerializeField, Tooltip("各パーティクルプレハブ\n0-1=判定ライン\n2-3=ドラム\n4=リザルト")]
        private GameObject[] particleObjs;

        public void GenerateParticle(int id)
        {
            GameObject generateObj = null;
            string parentText = null;
            int rotateOp = 0;//rotation処理用
            float posx = 0;
            float posy = -20;
            float posz = 9999;
            switch (id)
            {
                //ノーツ
                case 0:
                    generateObj = particleObjs[1];
                    parentText = "UI_JudgeText";
                    posx = -75;
                    posy = 130;
                    break;
                case 1:
                    generateObj = particleObjs[0];
                    parentText = "UI_JudgeText";
                    posx = -75;
                    posy = -20;
                    break;
                case 2:
                    generateObj = particleObjs[0];
                    parentText = "UI_JudgeText";
                    posx = -25;
                    posy = -20;
                    break;
                case 3:
                    generateObj = particleObjs[0];
                    parentText = "UI_JudgeText";
                    posx = 25;
                    posy = -20;
                    break;
                case 4:
                    generateObj = particleObjs[0];
                    parentText = "UI_JudgeText";
                    posx = 75;
                    posy = -20;
                    break;
                case 5:
                    generateObj = particleObjs[1];
                    parentText = "UI_JudgeText";
                    posx = 75;
                    posy = 130;
                    break;
                //ドラム
                case 10:
                    generateObj = particleObjs[3];
                    parentText = "DrumSet";
                    posx = -0.5f;
                    posy = 0.2f;
                    posz = 0.2f;
                    rotateOp = 1;
                    break;
                case 11:
                    generateObj = particleObjs[2];
                    parentText = "DrumSet";
                    posx = -0.6f;
                    posy = 0;
                    posz = -0.1f;
                    break;
                case 12:
                    generateObj = particleObjs[2];
                    parentText = "DrumSet";
                    posx = -0.2f;
                    posy = 0;
                    posz = 0.1f;
                    break;
                case 13:
                    generateObj = particleObjs[2];
                    parentText = "DrumSet";
                    posx = 0.2f;
                    posy = 0;
                    posz = 0.1f;
                    break;
                case 14:
                    generateObj = particleObjs[2];
                    parentText = "DrumSet";
                    posx = 0.6f;
                    posy = 0;
                    posz = -0.1f;
                    break;
                case 15:
                    generateObj = particleObjs[3];
                    parentText = "DrumSet";
                    posx = 0.5f;
                    posy = 0.2f;
                    posz = 0.2f;
                    rotateOp = 1;
                    break;
                //リザルト
                case 90:
                    generateObj = particleObjs[4];
                    posx = 0;
                    posy = 0.5f;
                    posz = 25;
                    break;
            }

            if (generateObj == null)
            {
                Debug.LogError("[<color=yellow>ParticleGenerator</color>]パーティクルの生成に失敗しました");
            }
            GameObject g = VRCInstantiate(generateObj);

            //transformを設定
            if (parentText != null)
            {
                g.transform.SetParent(GameObject.Find(parentText).transform);
            }
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.localPosition =
                posz == 9999
                ? new Vector3(posx, posy, 0)
                : new Vector3(posx, posy, posz);
            if (rotateOp != 0)
            {
                g.transform.localRotation = Quaternion.Euler(-110, 30, 0);
            }
            ParticleSystem p = g.GetComponent<ParticleSystem>();
            p.Play();
        }
    }
}