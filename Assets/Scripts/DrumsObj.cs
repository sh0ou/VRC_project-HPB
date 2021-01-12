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

        [SerializeField, Tooltip("ドラム番号（手動指定）")]
        private int drumNum;

        [SerializeField, Tooltip("ドラムヒット時パーティクル")]
        private GameObject hitParticle;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("[<color=yellow>DrumsObj</color>]レイヤー番号:" + other.gameObject.layer);
            if (other.gameObject.layer == 23)
            {
                //ドラムエフェクト
                if (settingsMng.effectFlag)
                {
                    //VRCInstantiate(hitParticle);
                }
                //ドラム処理
                Debug.Log("[<color=yellow>DrumsObj</color>]ドラム判定:" + drumNum);
                gameMng.DrumAction(drumNum);
                if (settingsMng.windowFlag == 3)
                {
                    soundMng.audioSources[1].PlayOneShot(soundMng.seLists[1]);
                }
            }
        }
    }
}