using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffect : MonoBehaviour
{
    private float intensity;
    private float duration;
    private float defaultIntensity;
    private float leftHoldingTime;
    private float rightHoldingTime;
    public float Intensity
    {
        get { return intensity; }
        set { intensity = value; }
    }
    public float Duration
    {
        get { return duration; }
        set { duration = value; }
    }

    public float DefaultIntensity
    {
        get { return defaultIntensity; }
        set { defaultIntensity = value; }
    }

    public float LeftHoldingTime
    {
        get { return leftHoldingTime; }
        private set
        {
            leftHoldingTime = value;
            if (leftHoldingTime > duration)
            {
                SetLightsIntensity(leftLights, defaultIntensity);
            }
        }
    }
    public float RightHoldingTime
    {
        get { return RightHoldingTime; }
        private set
        {
            rightHoldingTime = value;
            if (rightHoldingTime > duration)
            {
                SetLightsIntensity(rightLights, defaultIntensity);
            }
        }
    }
    public List<Light> leftLights;
    public List<Light> rightLights;
    private void Start()
    {
        SetLightsIntensity(leftLights, defaultIntensity);
        SetLightsIntensity(rightLights, defaultIntensity);
        Init();
    }
    private void Update()
    {
        if (leftHoldingTime < duration)
        {
            leftHoldingTime += Time.deltaTime;

            if (leftHoldingTime > duration)
            {
                SetLightsIntensity(leftLights, defaultIntensity);
            }

        }
        if (rightHoldingTime < duration)
        {
            rightHoldingTime += Time.deltaTime;

            if (rightHoldingTime > duration)
            {
                SetLightsIntensity(rightLights, defaultIntensity);
            }

        }
    }
    private void Init()
    {
        leftHoldingTime = duration;
        rightHoldingTime = duration;
    }
    private void SetLightsIntensity(List<Light> lightsList, float intensity)
    {
        foreach (Light light in lightsList)
        {
            light.intensity = intensity;
        }
    }

    public void LeftLightEffectOn()
    {
        LeftHoldingTime = 0;
        SetLightsIntensity(leftLights, intensity);
    }
    public void RightLightEffectOn()
    {
        RightHoldingTime = 0;
        SetLightsIntensity(rightLights, intensity);
    }

}
