using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    public class Animation_Drum : UdonSharpBehaviour
    {
        [SerializeField] SyncManager syncManager;
        [SerializeField] Animator[] animator;
        public void PlayAnim()
        {
            //Debug.Log("[<color=green>Animation_Drum</color>]PlayAnim:" + animator[syncManager.targetlane]);
            switch (syncManager.targetid_a)
            {
                case 0:
                    animator[syncManager.targetlane].Play("Drum_happy", 0, 0);
                    break;
                case 1:
                    animator[syncManager.targetlane].Play("Drum_good", 0, 0);
                    break;
                case 2:
                    animator[syncManager.targetlane].Play("Drum_sad", 0, 0);
                    break;
            }
        }
    }
}