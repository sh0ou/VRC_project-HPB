using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    public class GenerateJudgeText : UdonSharpBehaviour
    {
        [SerializeField] UIManager uIManager;
        [SerializeField] SyncManager syncManager;

        public void GenerateText()
        {
            GameObject g = VRCInstantiate(uIManager.uiObj_judge[syncManager.targetTextid_a]);
            g.GetComponent<JudgeTextObj>().judgeValue = syncManager.targetTextid_b;
        }
    }
}