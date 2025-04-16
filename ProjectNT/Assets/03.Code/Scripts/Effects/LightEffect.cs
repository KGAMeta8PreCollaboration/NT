using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class LightEffect : MapEffect<Light>
{
    public float targetIntensity;
    public float targetDuration;
    public float defaultIntensity;
    public float defaultDuration;

    private void Start()
    {
        Init(left, ref leftSequence);
        Init(right, ref rightSequence);
    }

    protected override void Init(List<Light> list, ref Sequence sequence)
    {
        sequence = DOTween.Sequence().SetAutoKill(false);
        for (int i = 0; i < list.Count; i++)
        {
            list[i].intensity = defaultIntensity;
            sequence.Join(list[i].DOIntensity(targetIntensity, targetDuration).SetEase(Ease.OutQuint));
        }
        for (int i = 0; i < list.Count; i++)
        {
            list[i].intensity = defaultIntensity;
            sequence.Insert(targetDuration, list[i].DOIntensity(defaultIntensity, defaultDuration).SetEase(Ease.InQuad));
        }
        sequence.Pause();
    }

    public override void LeftEffectInvoke()
    {
        leftSequence.Restart();
    }

    public override void RightEffectInvoke()
    {
        rightSequence.Restart();
    }

    public override void LeftEffectEnd()
    {
        player1Delegate -= LeftEffectInvoke;
        foreach (Light light in left)
        {
            light.intensity = targetIntensity;
        }
    }

    public override void RightEffectEnd()
    {
        player2Delegate -= RightEffectInvoke;
        foreach (Light light in right)
        {
            light.intensity = targetIntensity;
        }
    }
}
