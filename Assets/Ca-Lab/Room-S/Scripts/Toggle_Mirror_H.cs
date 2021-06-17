using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Toggle_Mirror_H : UdonSharpBehaviour
{
    [SerializeField] Toggle_Mirror_L subScr;
    public GameObject mirrorObj_h;
    public AudioSource se;
    public int mirrorStatus; //0=off 1=low 2=high

    void Start()
    {
        mirrorStatus = 0;
    }
    public override void Interact()
    {
        se.PlayOneShot(se.clip);
        switch (mirrorStatus)
        {
            case 0:
                mirrorObj_h.SetActive(true);
                mirrorStatus = 2;
                break;
            case 1:
                subScr.mirrorObj_l.SetActive(false);
                mirrorObj_h.SetActive(true);
                mirrorStatus = 2;
                break;
            case 2:
                mirrorObj_h.SetActive(false);
                mirrorStatus = 0;
                break;
        }
    }
}