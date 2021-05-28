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

        [SerializeField, Tooltip("各パーティクル位置\n0-5=判定ライン\n6-11=ドラム\n12=リザルト")]
        private Transform[] particlePoss;

        private void Start()
        {
            for (int round = 0; round < particlePoss.Length; round++)
            {
                Debug.Log("Pos" + round + ":" + particlePoss[round].position);
            }
        }

        public void GenerateParticle(int id)
        {
            GameObject generateObj = null;
            //string parentText = null;
            int rotateOp = 0;//rotation処理用

            float posx = 0;
            float posy = 0;
            float posz = 0;
            switch (id)
            {
                //ノーツ
                case 0:
                    generateObj = particleObjs[1];
                    posx = particlePoss[0].position.x;
                    posy = particlePoss[0].position.y;
                    posz = particlePoss[0].position.z;
                    break;
                case 1:
                    generateObj = particleObjs[0];
                    posx = particlePoss[1].position.x;
                    posy = particlePoss[1].position.y;
                    posz = particlePoss[1].position.z;
                    break;
                case 2:
                    generateObj = particleObjs[0];
                    posx = particlePoss[2].position.x;
                    posy = particlePoss[2].position.y;
                    posz = particlePoss[2].position.z;
                    break;
                case 3:
                    generateObj = particleObjs[0];
                    posx = particlePoss[3].position.x;
                    posy = particlePoss[3].position.y;
                    posz = particlePoss[3].position.z;
                    break;
                case 4:
                    generateObj = particleObjs[0];
                    posx = particlePoss[4].position.x;
                    posy = particlePoss[4].position.y;
                    posz = particlePoss[4].position.z;
                    break;
                case 5:
                    generateObj = particleObjs[1];
                    posx = particlePoss[5].position.x;
                    posy = particlePoss[5].position.y;
                    posz = particlePoss[5].position.z;
                    break;
                //ドラム
                case 10:
                    generateObj = particleObjs[3];
                    posx = particlePoss[6].position.x;
                    posy = particlePoss[6].position.y;
                    posz = particlePoss[6].position.z;
                    rotateOp = 1;
                    break;
                case 11:
                    generateObj = particleObjs[2];
                    posx = particlePoss[7].position.x;
                    posy = particlePoss[7].position.y;
                    posz = particlePoss[7].position.z;
                    rotateOp = 3;
                    break;
                case 12:
                    generateObj = particleObjs[2];
                    posx = particlePoss[8].position.x;
                    posy = particlePoss[8].position.y;
                    posz = particlePoss[8].position.z;
                    rotateOp = 3;
                    break;
                case 13:
                    generateObj = particleObjs[2];
                    posx = particlePoss[9].position.x;
                    posy = particlePoss[9].position.y;
                    posz = particlePoss[9].position.z;
                    rotateOp = 3;
                    break;
                case 14:
                    generateObj = particleObjs[2];
                    posx = particlePoss[10].position.x;
                    posy = particlePoss[10].position.y;
                    posz = particlePoss[10].position.z;
                    rotateOp = 3;
                    break;
                case 15:
                    generateObj = particleObjs[3];
                    posx = particlePoss[11].position.x;
                    posy = particlePoss[11].position.y;
                    posz = particlePoss[11].position.z;
                    rotateOp = 2;
                    break;
                //リザルト
                case 90:
                    generateObj = particleObjs[4];
                    posx = particlePoss[12].position.x;
                    posy = particlePoss[12].position.y;
                    posz = particlePoss[12].position.z;
                    rotateOp = 3;
                    break;
            }

            if (generateObj == null)
            {
                Debug.LogError("[<color=yellow>ParticleGenerator</color>]パーティクルの生成に失敗しました");
            }
            GameObject g = VRCInstantiate(generateObj);

            //transformを設定
            g.transform.localScale = new Vector3(1, 1, 1);
            g.transform.position = new Vector3(posx, posy, posz);
            g.transform.localRotation =
                rotateOp == 1 ? Quaternion.Euler(-110, -30, 0) ://シンバルL
                rotateOp == 2 ? Quaternion.Euler(-110, 30, 0) ://シンバルR
                rotateOp == 3 ? Quaternion.Euler(-90, 0, 0) :
                Quaternion.Euler(0, 0, 0);
            ParticleSystem p = g.GetComponent<ParticleSystem>();
            p.Play();
        }
    }
}