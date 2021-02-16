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

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("[<color=yellow>DrumsObj</color>]レイヤー番号:" + other.gameObject.layer);
            if (other.gameObject.layer == 23)
            {
                //ドラム処理
                gameMng.DrumAction(drumNum);
                //other.GetComponent<VRC_Pickup>().PlayHaptics();
            }
        }
    }
}