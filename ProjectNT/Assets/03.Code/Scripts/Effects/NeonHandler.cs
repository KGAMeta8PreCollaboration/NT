using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Analytics;

public class NeonHandler : MapEffect<Material>
{
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
        Init(left, ref leftSequence);
        Init(right, ref rightSequence);
    }
    protected override void Init(List<Material> list, ref Sequence sequence)
    {
        sequence = DOTween.Sequence().SetAutoKill(false);

        // Target Intensity DOTween
        for (int i = 0; i < list.Count; i++)
        {
            Color targetColor = i < 4 ? GetSignsAlbedoColor(i) : GetTextAlbedoColor(i - 4);
            sequence.Join(list[i].DOColor(targetColor * targetIntensity, "_EmissionColor", targetDuration).SetEase(Ease.OutQuint));
        }

        // Default Intensity DOTween
        for (int i = 0; i < list.Count; i++)
        {
            Color defaultColor = i < 4 ? GetSignsAlbedoColor(i) : GetTextAlbedoColor(i - 4);
            sequence.Join(list[i].DOColor(defaultColor / defaultIntensity, "_EmissionColor", defaultDuration).SetEase(Ease.InSine));

            list[i].SetColor("_EmissionColor", defaultColor / defaultIntensity);
        }
        sequence.Pause();
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

    public override void P1EffectInvoke()
    {
        leftSequence.Restart();
    }

    public override void P2EffectInvoke()
    {
        rightSequence.Restart();
    }

    public override void LeftEffectEnd()
    {
        leftSequence.Kill();
        for (int i = 0; i < left.Count; i++)
        {
            Color targetColor = i < 4 ? GetSignsAlbedoColor(i) : GetTextAlbedoColor(i - 4);
            left[i].DOColor(targetColor * targetIntensity, "_EmissionColor", targetDuration).SetEase(Ease.OutQuint);
        }

    }

    public override void RightEffectEnd()
    {
        rightSequence.Kill();
        for (int i = 0; i < right.Count; i++)
        {
            Color targetColor = i < 4 ? GetSignsAlbedoColor(i) : GetTextAlbedoColor(i - 4);
            right[i].DOColor(targetColor * targetIntensity, "_EmissionColor", targetDuration).SetEase(Ease.OutQuint);
        }
    }
}
