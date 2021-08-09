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
        [SerializeField] AudioSource audioSource;
        [SerializeField] PlayManager playManager;
        [SerializeField] GameManager gameManager;

        [SerializeField] int playingAnimID;
        [SerializeField] float animtimeleft;

        void Start()
        {
            //AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
            //Debug.Log("clipcheck: " + clipInfos[0].clip.name);
        }

        private void Update()
        {
            //アニメーションのズレ対策
            //if ((audioSource.time / playManager.endTime) - animator.GetCurrentAnimatorClipInfo(0)[playingAnimID].clip.no >= Mathf.Abs(3f))
            //{
            //    animator.Play(("Music_" + syncManager.targetid_a), 0, audioSource.time);
            //}
            //animtimeleft = (audioSource.time / playManager.endTime) - animator.GetCurrentAnimatorStateInfo(playingAnimID).normalizedTime;
            //Debug.Log(audioSource.time / playManager.endTime);
            //animtimeleft = animator.GetCurrentAnimatorStateInfo(playingAnimID).normalizedTime;
        }

        public void PlayMusicAnim()
        {
            //playingAnimID = syncManager.targetid_a;
            if (gameManager.GetMusicNum() == 0 && gameManager.GetLevelNum() != 0)
            {
                animator.Play("Idle");
            }
            if (syncManager.isActivePlayer)
            {
                animator.Play(("Music_" + syncManager.targetid_a), 0, 0);
            }
            else
            {
                animator.Play(("Music_" + syncManager.targetid_a), 0, 0.003f);
            }
            //Debug.Log("[MusicAnimator]NumCheck:" + gameManager.GetMusicNum() + "/" + gameManager.GetLevelNum());
        }

        public void StopAnim()
        {
            animator.Play("Idle", 0, 0);
        }

        //todo:ここにアニメーションのpublicメソッドを挿入
    }
}