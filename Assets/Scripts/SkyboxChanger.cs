using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

public class SkyboxChanger : UdonSharpBehaviour
{
    [SerializeField]
    private Material skybox_sun;
    [SerializeField]
    private Material skybox_sunset;
    [SerializeField]
    private Material skybox_night;
    void Start()
    {
        int time = DateTime.Now.Hour;
        //PC“à‚Ì‚ğQÆ‚µ‚ÄSkybox‚ğİ’è
        switch (time)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 20:
            case 21:
            case 22:
            case 23:
                RenderSettings.skybox = skybox_night;
                break;
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
                RenderSettings.skybox = skybox_sun;
                break;
            case 15:
            case 16:
            case 17:
            case 18:
            case 19:
                RenderSettings.skybox = skybox_sunset;
                break;
        }
    }
}