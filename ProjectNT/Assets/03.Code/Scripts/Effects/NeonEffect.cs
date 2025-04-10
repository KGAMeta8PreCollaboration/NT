using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Analytics;

public class NeonEffect : MonoBehaviour
{
    private Sequence leftNeonSequence;
    private Sequence rightNeonSequence;
    [SerializeField]
    private List<Material> rightMaterialsList;
    [SerializeField]
    private List<Material> leftMaterialsList;
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
        Init(leftMaterialsList, ref leftNeonSequence);
        Init(rightMaterialsList, ref rightNeonSequence);
    }

    private void LeftNeonDotween()
    {
        leftNeonSequence.Restart();
    }

    private void RightNeonDotween()
    {
        rightNeonSequence.Restart();
    }

    public void NeonEffectOn()
    {
        RightNeonDotween();
        LeftNeonDotween();
    }

    private void Init(List<Material> materialsList, ref Sequence neonSequence)
    {
        neonSequence = DOTween.Sequence().SetAutoKill(false);

        // Target Intensity 애니메이션
        for (int i = 0; i < materialsList.Count; i++)
        {
            Color targetColor = i < 4 ? GetSignsAlbedoColor(i) : GetTextAlbedoColor(i - 4);
            neonSequence.Join(materialsList[i].DOColor(targetColor * targetIntensity, "_EmissionColor", targetDuration).SetEase(Ease.OutQuint));
        }

        // Default Intensity 애니메이션
        for (int i = 0; i < materialsList.Count; i++)
        {
            Color defaultColor = i < 4 ? GetSignsAlbedoColor(i) : GetTextAlbedoColor(i - 4);
            neonSequence.Join(materialsList[i].DOColor(defaultColor / defaultIntensity, "_EmissionColor", defaultDuration).SetEase(Ease.InSine));
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
