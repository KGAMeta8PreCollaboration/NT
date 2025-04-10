using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LightDotween : MonoBehaviour
{
    [SerializeField]
    private Light light;
    private float targetIntensity;
    private float duration;
    private float defaultIntensity;
    private float defaultDuration;
    public float TargetIntensity
    {
        get { return targetIntensity; }
        set { targetIntensity = value; }
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
    public float DefaultDuration
    {
        get { return defaultDuration; }
        set { defaultDuration = value; }
    }
    private void Awake()
    {
        if (null == light)
        {
            light = GetComponent<Light>();
        }

    }
    // private void Start()
    // {
    //     sequence = DOTween.Sequence().SetAutoKill(false).
    //     Append(light.DOIntensity(targetIntensity, duration).SetEase(Ease.OutQuint)).
    //     Append(light.DOIntensity(defaultIntensity, defaultDuration).SetEase(Ease.OutQuint));
    //     sequence.Pause();
    // }
}
