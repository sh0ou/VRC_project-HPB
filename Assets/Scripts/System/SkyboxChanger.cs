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
    private Material skybox_cloud;
    [SerializeField]
    private Material skybox_sunset;
    [SerializeField]
    private Material skybox_night;
    public void SetSkybox_sun()
    {
        RenderSettings.skybox = skybox_sun;
    }
    public void SetSkybox_cloud()
    {
        RenderSettings.skybox = skybox_cloud;
    }

    public void SetSkybox_sunset()
    {
        RenderSettings.skybox = skybox_sunset;
    }

    public void SetSkybox_night()
    {
        RenderSettings.skybox = skybox_night;
    }
}