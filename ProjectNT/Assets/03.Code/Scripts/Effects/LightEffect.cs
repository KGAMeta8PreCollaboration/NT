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
            sequence.Join(list[i].DOIntensity(targetIntensity, targetDuration).SetEase(Ease.OutQuint));
        }
        for (int i = 0; i < list.Count; i++)
        {
            sequence.Join(list[i].DOIntensity(defaultIntensity, defaultDuration).SetEase(Ease.OutQuint));
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
}