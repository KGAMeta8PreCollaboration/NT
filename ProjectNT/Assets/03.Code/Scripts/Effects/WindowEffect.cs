using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WindowEffect : MapEffect<WindowEffect>
{
    public float targetIntensity;
    public float targetDuration;
    public float defaultIntensity;
    public float defaultDuration;
    [ColorUsage(false, true)] public Color W_W_Banana2;
    [ColorUsage(false, true)] public Color W_W_Banana3;
    [ColorUsage(false, true)] public Color W_W_Gray;
    [ColorUsage(false, true)] public Color W_W_Lemon;
    [ColorUsage(false, true)] public Color W_W_Corn;
    [ColorUsage(false, true)] public Color W_W_Orange;
    [ColorUsage(false, true)] public Color W_W_Chick;
    [ColorUsage(false, true)] public Color W_W_ICE;
    [ColorUsage(false, true)] public Color W_W_Dirt;
    [ColorUsage(false, true)] public Color W_W_Blue;
    [ColorUsage(false, true)] public Color W_W_Blue2;
    [ColorUsage(false, true)] public Color W_W_Pink;
    [ColorUsage(false, true)] public Color W_W_Green;
    [ColorUsage(false, true)] public Color W_W_White;
    [ColorUsage(false, true)] public Color W_W_Red;

    private void Awake()
    {
        Init(left, ref leftSequence);
        Init(right, ref rightSequence);
    }

    public override void P1EffectInvoke()
    {

    }

    public override void P2EffectInvoke()
    {

    }

    public override void LeftEffectEnd()
    {

    }
    public override void RightEffectEnd()
    {

    }

    protected override void Init(List<WindowEffect> list, ref Sequence sequence)
    {

    }
}
