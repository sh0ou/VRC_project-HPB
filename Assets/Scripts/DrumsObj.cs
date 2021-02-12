using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// ドラムオブジェクト
    /// </summary>
    public class DrumsObj : UdonSharpBehaviour
    {
        [SerializeField, Tooltip("ゲームマネージャ")]
        private GameManager gameMng;

        [SerializeField, Tooltip("設定マネージャ")]
        private SettingsManager settingsMng;

        [SerializeField, Tooltip("サウンドマネージャ")]
        private SoundManager soundMng;

        [SerializeField, Tooltip("パーティクルジェネレータ")]
        private ParticleGenerator particleGenerator;

        [SerializeField, Tooltip("ドラム番号（手動指定）")]
        private int drumNum;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TestMethod();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("[<color=yellow>DrumsObj</color>]レイヤー番号:" + other.gameObject.layer);
            if (other.gameObject.layer == 23)
            {
                //ドラム処理
                TestMethod();
                //other.GetComponent<VRC_Pickup>().PlayHaptics();
            }
        }
        public void TestMethod()
        {
            //Debug.Log("[<color=yellow>DrumsObj</color>]ドラム判定:" + drumNum);
            gameMng.DrumAction(drumNum);

            //ドラムエフェクト
            if (settingsMng.effectFlag)
            {
                switch (drumNum)
                {
                    case 0:
                        particleGenerator.GenerateParticle(10);
                        break;
                    case 5:
                        particleGenerator.GenerateParticle(15);
                        break;
                    case 1:
                        particleGenerator.GenerateParticle(11);
                        break;
                    case 2:
                        particleGenerator.GenerateParticle(12);
                        break;
                    case 3:
                        particleGenerator.GenerateParticle(13);
                        break;
                    case 4:
                        particleGenerator.GenerateParticle(14);
                        break;
                }
            }

            if (settingsMng.windowFlag == 3)
            {
                soundMng.audioSources[1].PlayOneShot(soundMng.seLists[1]);
            }
        }
    }
}