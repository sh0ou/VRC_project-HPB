using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// プレイヤーを変更するスクリプト
    /// </summary>
    public class ChangePlayer : UdonSharpBehaviour
    {
        [SerializeField] SyncManager syncManager;
        [SerializeField] GameObject[] targetObjs = new GameObject[4];
        public override void Interact()
        {
            if (!syncManager.isActivePlayer)
            {
                Debug.Log("[<color=yellow>SyncManager</color>]検知:Player変更");
                //Networking.SetOwner(Networking.LocalPlayer, GameObject.Find("StickPos_L"));
                Networking.SetOwner(Networking.LocalPlayer, targetObjs[0]);
                //Networking.SetOwner(Networking.LocalPlayer, GameObject.Find("StickPos_R"));
                Networking.SetOwner(Networking.LocalPlayer, targetObjs[1]);
                //Networking.SetOwner(Networking.LocalPlayer, GameObject.Find("SyncManager"));
                Networking.SetOwner(Networking.LocalPlayer, targetObjs[2]);
                //Networking.SetOwner(Networking.LocalPlayer, GameObject.Find("NotesJudger"));
                Networking.SetOwner(Networking.LocalPlayer, targetObjs[3]);
                RequestSerialization();
            }
        }
        //public void Change()
        //{

        //}
    }
}