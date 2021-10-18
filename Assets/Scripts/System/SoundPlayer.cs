using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    public class SoundPlayer : UdonSharpBehaviour
    {
        [SerializeField] SoundManager soundManager;
        [SerializeField] SyncManager syncManager;

        /// <summary>
        /// ドラム効果音を再生します
        /// </summary>
        public void PlayDrumSE()
        {
            //Debug.Log("[<color=green>SoundPlayer</color>]PlayDrum:アクティブ");
            if (soundManager.seLists[syncManager.targetSEid + 10] == null)
            {
                Debug.Log("[<color=red>SoundPlayer</color>]PlayDrum:範囲外です。処理を中断しました");
                return;
            }
            soundManager.audioSources[1].PlayOneShot(soundManager.seLists[syncManager.targetSEid + 10]);
        }
    }
}