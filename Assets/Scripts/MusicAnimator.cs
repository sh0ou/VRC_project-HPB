using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// 楽曲再生中のアニメーションを管理するスクリプト
    /// </summary>
    public class MusicAnimator : UdonSharpBehaviour
    {
        [SerializeField] public Animator animator;
        [SerializeField] SyncManager syncManager;

        void Start()
        {
            AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
            Debug.Log("clipcheck: " + clipInfos[0].clip.name);
        }

        public void PlayMusicAnim()
        {
            if (syncManager.isActivePlayer)
            {
                animator.Play(("Music_" + syncManager.targetid_a), 0, 0);
            }
            else
            {
                animator.Play(("Music_" + syncManager.targetid_a), 0, 0.4f);
            }
        }

        public void StopAnim()
        {
            animator.Play("Idle", 0, 0);
        }
    }
}