using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Analytics;

public class NeonEffect : MonoBehaviour
{
    private Sequence neonSequence;
    [SerializeField]

    private List<Material> neonsList;
    public float targetIntensity;
    public float targetDuration;
    public float defaultIntensity;
    public float defaultDuration;
    [ColorUsage(false, true)] public Color signs_Albedo_color;
    [ColorUsage(false, true)] public Color signs_Albedo1_color;
    [ColorUsage(false, true)] public Color signs_Albedo2_color;
    [ColorUsage(false, true)] public Color signs_Albedo3_color;
    [ColorUsage(false, true)] public Color text_Albedo_color;
    [ColorUsage(false, true)] public Color text_Albedo1_color;
    [ColorUsage(false, true)] public Color text_Albedo2_color;
    [ColorUsage(false, true)] public Color text_Albedo3_color;
    [ColorUsage(false, true)] public Color text_Albedo4_color;
    private void Awake()
    {
        Init();
    }
    private void Start()
    {
    }
    public void NeonDotween()
    {
        neonSequence.Restart();
    }

    // private void Init()
    // {
    //     neonSequence = DOTween.Sequence().SetAutoKill(false).
    //     Append(neonsList[0].DOColor(signs_Albedo_color * targetIntensity, "_EmissionColor", targetDuration)).SetEase(Ease.OutQuint).
    //     Join(neonsList[1].DOColor(signs_Albedo1_color * targetIntensity, "_EmissionColor", targetDuration)).SetEase(Ease.OutQuint).
    //     Join(neonsList[2].DOColor(signs_Albedo2_color * targetIntensity, "_EmissionColor", targetDuration)).SetEase(Ease.OutQuint).
    //     Join(neonsList[3].DOColor(signs_Albedo3_color * targetIntensity, "_EmissionColor", targetDuration)).SetEase(Ease.OutQuint).
    //     Join(neonsList[4].DOColor(text_Albedo_color * targetIntensity, "_EmissionColor", targetDuration)).SetEase(Ease.OutQuint).
    //     Join(neonsList[5].DOColor(text_Albedo1_color * targetIntensity, "_EmissionColor", targetDuration)).SetEase(Ease.OutQuint).
    //     Join(neonsList[6].DOColor(text_Albedo2_color * targetIntensity, "_EmissionColor", targetDuration)).SetEase(Ease.OutQuint).
    //     Join(neonsList[7].DOColor(text_Albedo3_color * targetIntensity, "_EmissionColor", targetDuration)).SetEase(Ease.OutQuint).
    //     Join(neonsList[8].DOColor(text_Albedo4_color * targetIntensity, "_EmissionColor", targetDuration)).SetEase(Ease.OutQuint).
    //     Append(neonsList[0].DOColor(signs_Albedo_color * defaultIntensity, "_EmissionColor", defaultDuration)).SetEase(Ease.InSine).
    //     Join(neonsList[1].DOColor(signs_Albedo1_color * defaultIntensity, "_EmissionColor", defaultDuration)).SetEase(Ease.InSine).
    //     Join(neonsList[2].DOColor(signs_Albedo2_color * defaultIntensity, "_EmissionColor", defaultDuration)).SetEase(Ease.InSine).
    //     Join(neonsList[3].DOColor(signs_Albedo3_color * defaultIntensity, "_EmissionColor", defaultDuration)).SetEase(Ease.InSine).
    //     Join(neonsList[4].DOColor(text_Albedo_color * defaultIntensity, "_EmissionColor", defaultDuration)).SetEase(Ease.InSine).
    //     Join(neonsList[5].DOColor(text_Albedo1_color * defaultIntensity, "_EmissionColor", defaultDuration)).SetEase(Ease.InSine).
    //     Join(neonsList[6].DOColor(text_Albedo2_color * defaultIntensity, "_EmissionColor", defaultDuration)).SetEase(Ease.InSine).
    //     Join(neonsList[7].DOColor(text_Albedo3_color * defaultIntensity, "_EmissionColor", defaultDuration)).SetEase(Ease.InSine).
    //     Join(neonsList[8].DOColor(text_Albedo4_color * defaultIntensity, "_EmissionColor", defaultDuration)).SetEase(Ease.InSine);
    //     neonSequence.Pause();
    // }

    private void Init()
    {
        neonSequence = DOTween.Sequence().SetAutoKill(false);

        // Target Intensity 애니메이션
        for (int i = 0; i < neonsList.Count; i++)
        {
            Color targetColor = i < 4 ? GetSignsAlbedoColor(i) : GetTextAlbedoColor(i - 4);
            neonSequence.Join(neonsList[i].DOColor(targetColor * targetIntensity, "_EmissionColor", targetDuration).SetEase(Ease.OutQuint));
        }

        // Default Intensity 애니메이션
        for (int i = 0; i < neonsList.Count; i++)
        {
            Color defaultColor = i < 4 ? GetSignsAlbedoColor(i) : GetTextAlbedoColor(i - 4);
            neonSequence.Join(neonsList[i].DOColor(defaultColor * defaultIntensity, "_EmissionColor", defaultDuration).SetEase(Ease.InSine));
        }

        neonSequence.Pause();
    }

    private Color GetSignsAlbedoColor(int index)
    {
        return index switch
        {
            0 => signs_Albedo_color,
            1 => signs_Albedo1_color,
            2 => signs_Albedo2_color,
            3 => signs_Albedo3_color,
            _ => Color.black // 기본값
        };
    }

    private Color GetTextAlbedoColor(int index)
    {
        return index switch
        {
            0 => text_Albedo_color,
            1 => text_Albedo1_color,
            2 => text_Albedo2_color,
            3 => text_Albedo3_color,
            4 => text_Albedo4_color,
            _ => Color.black // 기본값
        };
    }
}
