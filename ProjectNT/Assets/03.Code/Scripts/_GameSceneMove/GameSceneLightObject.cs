using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneLightObject : MonoBehaviour//빛날 오브젝트들에 달아야할 컴포넌트
{
    public Light[] lightObj;

    public void StartFadeLight(float startIntensity, float duration)
    {
        StartCoroutine(FadeLight(startIntensity, duration));
    }

    public void OffLight()
    {
        foreach (Light light in lightObj)
        {
            light.intensity = 0;
        }
    }

    public IEnumerator FadeLight(float startIntensity, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float lerpedIntensity = Mathf.Lerp(startIntensity, 0f, timeElapsed / duration);

            foreach (Light light in lightObj)
            {
                light.intensity = lerpedIntensity;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        foreach (Light light in lightObj)
        {
            light.intensity = 0f;
        }
    }
}
