using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public abstract class MapEffect<T> : MonoBehaviour where T : UnityEngine.Object
{
    [SerializeField]
    protected List<T> left;
    [SerializeField]
    protected List<T> right;
    protected Sequence leftSequence;
    protected Sequence rightSequence;

    public EffectDelegate effectDelegate;

    protected abstract void Init(List<T> list, ref Sequence sequence);

    public abstract void LeftEffectInvoke();
    public abstract void RightEffectInvoke();
}
