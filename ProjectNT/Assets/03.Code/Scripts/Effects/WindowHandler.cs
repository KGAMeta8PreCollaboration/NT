using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using DG.Tweening;
using UnityEngine;

public class WindowHandler : MapEffect<Material>
{
    public List<float> targetEmission;
    public float targetDuration;
    public List<float> defaultEmission;
    public float defaultDuration;

    private void Awake()
    {
        Init(left, ref leftSequence);
        Init(right, ref rightSequence);
    }

    protected override void Init(List<Material> list, ref Sequence sequence)
    {
        sequence = DOTween.Sequence().SetAutoKill(false);

        for (int i = 0; i < list.Count; i++)
        {
            // Emission 기본값 설정
            list[i].SetFloat("_Emission", defaultEmission[i]);

            sequence.Join(list[i].DOFloat(targetEmission[i], "_Emission", targetDuration).SetEase(Ease.OutQuint));
        }
        for (int i = 0; i < list.Count; i++)
        {
            sequence.Insert(targetDuration, list[i].DOFloat(defaultEmission[i], "_Emission", defaultDuration).SetEase(Ease.InQuad));
        }
        sequence.Pause();
    }
    public override void P1EffectInvoke()
    {
        rightSequence.Restart();
    }

    public override void P2EffectInvoke()
    {
        leftSequence.Restart();
    }

    public override void LeftEffectEnd()
    {
        leftSequence.Kill();
        SetTargetEmission(left);
    }
    public override void RightEffectEnd()
    {
        rightSequence.Kill();
        SetTargetEmission(right);
    }

    private void SetTargetEmission(List<Material> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetFloat("_Emission", targetEmission[i]);
        }
    }
}
