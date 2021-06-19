using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    public class Animation_Chain : UdonSharpBehaviour
    {
        [SerializeField] GameObject chainUIObj;
        [SerializeField] Animator chainUIAnim;
        [SerializeField] SyncManager syncManager;
        public void PlayAnim()
        {
            Debug.Log("[<color=green>Animation_Chain</color>]PlayAnim:アクティブ");
            if (syncManager.targetBool)
            {
                chainUIObj.SetActive(true);
                chainUIAnim.Play("addvalue", 0, 0);
            }
            else
            {
                chainUIObj.SetActive(false);
            }
        }
    }
}