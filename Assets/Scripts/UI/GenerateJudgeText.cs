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
            //ここにfastSlow表示を実装　targetBool
            if (syncManager.targetTextid_fast == 1)
            {
                GameObject g2 = VRCInstantiate(uIManager.uiObj_judge_slow);
                g2.GetComponent<JudgeTextObj>().judgeValue = syncManager.targetTextid_b;
            }
            else if (syncManager.targetTextid_fast == -1)
            {
                GameObject g2 = VRCInstantiate(uIManager.uiObj_judge_fast);
                g2.GetComponent<JudgeTextObj>().judgeValue = syncManager.targetTextid_b;
            }

        }
    }
}