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
            soundManager.audioSources[1].PlayOneShot(soundManager.seLists[syncManager.targetid_a + 10]);
        }
    }
}