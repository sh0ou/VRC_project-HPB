using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// オブジェクトを初期位置に移動します
    /// </summary>
    public class RespawnObject : UdonSharpBehaviour
    {
        [SerializeField] GameObject stick_L;
        [SerializeField] GameObject stick_R;
        [SerializeField] Transform stickPos_L;
        [SerializeField] Transform stickPos_R;

        public void Respawn()
        {
            stick_L.transform.position = stickPos_L.position;
            stick_L.transform.rotation = stickPos_L.rotation;
            stick_R.transform.position = stickPos_R.position;
            stick_R.transform.rotation = stickPos_R.rotation;
            RequestSerialization();
        }
    }
}