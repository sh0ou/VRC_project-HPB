using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// ドラムオブジェクト
/// </summary>
public class DrumsObj : UdonSharpBehaviour
{
    [SerializeField, Tooltip("ゲームマネージャ")]
    private HPB_GameManager gameMng;

    [SerializeField, Tooltip("設定マネージャ")]
    private HPB_SettingsManager settingsMng;

    [SerializeField, Tooltip("サウンドマネージャ")]
    private SoundManager soundMng;

    [SerializeField, Tooltip("ドラム番号（手動指定）")]
    private int drumNum;

    [SerializeField, Tooltip("ドラムヒット時パーティクル")]
    private GameObject hitParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 23)
        {
            //ドラムエフェクト
            if (settingsMng.effectFlag)
            {
                //VRCInstantiate(hitParticle);
            }
            //ドラム処理
            gameMng.DrumAction(drumNum);
            if(settingsMng.windowFlag == 3)
            {
                soundMng.audioSources[1].PlayOneShot(soundMng.seLists[1]);
            }
        }
    }
}
