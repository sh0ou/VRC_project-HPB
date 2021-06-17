using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Toggle_Mirror_L : UdonSharpBehaviour
{
    [SerializeField] private Toggle_Mirror_H mainScr;
    public GameObject mirrorObj_l;
    public override void Interact()
    {
        mainScr.se.PlayOneShot(mainScr.se.clip);
        switch (mainScr.mirrorStatus)
        {
            case 0:
                mirrorObj_l.SetActive(true);
                mainScr.mirrorStatus = 1;
                break;
            case 1:
                mirrorObj_l.SetActive(false);
                mainScr.mirrorStatus = 0;
                break;
            case 2:
                mirrorObj_l.SetActive(true);
                mainScr.mirrorObj_h.SetActive(false);
                mainScr.mirrorStatus = 1;
                break;
        }
    }
}