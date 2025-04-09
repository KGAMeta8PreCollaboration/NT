using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun.Demo.Cockpit.Forms;
using UnityEngine;

public class LightEffect : MonoBehaviour
{
    public float targetIntensity;
    public float targetDuration;
    public float defaultIntensity;
    public float defaultDuration;
    [SerializeField]
    private List<LightDotween> leftLights;
    [SerializeField]
    private List<LightDotween> rightLights;

    private void Awake()
    {
        Init(leftLights);
        Init(rightLights);
    }
    private void Init(List<LightDotween> lightsList)
    {
        foreach (LightDotween light in lightsList)
        {
            light.TargetIntensity = targetIntensity;
            light.Duration = targetDuration;
            light.DefaultIntensity = defaultIntensity;
            light.DefaultDuration = defaultDuration;
        }
    }
    private void SetLightsIntensity(List<LightDotween> lightsList)
    {
        foreach (LightDotween light in lightsList)
        {
            light.sequence.Restart();
        }
    }

    public void LightsEffectOn()
    {
        SetLightsIntensity(leftLights);
        SetLightsIntensity(rightLights);
    }

}
