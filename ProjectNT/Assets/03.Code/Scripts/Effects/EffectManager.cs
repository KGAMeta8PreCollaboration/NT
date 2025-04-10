using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Realtime;
using UnityEngine;

public delegate void EffectDelegate();
public class EffectManager : Singleton<EffectManager>
{

    private Enums.PlayMode playMode;
    public Enums.PlayMode PlayMode
    {
        get { return playMode; }
        set
        {
            playMode = value;
            SetPlayMode(playMode);
        }
    }

    [Header("Spot Light Effect")]
    [SerializeField]
    public Dictionary<Type, EffectDelegate> effects = new Dictionary<Type, EffectDelegate>();
    public Action<Note, int> OnMapEffect;

    public LightEffect lightEffect;
    public NeonEffect neonEffect;

    protected override void Awake()
    {
        base.Awake();

        //TODO: 작업 완료시 주석해제
        // SceneManager.activeSceneChanged += (x, y) =>
        // {
        //     if (SceneManager.GetActiveScene().name == "GameScene")
        //     {
        //         Initialize();
        //     }
        // };
        //TODO: 작업 완료시 지워야함
        Initialize();
    }
    private void OnDisable()
    {
        //TODO 씬전환 시 구독해제로 변경예정
        OnMapEffect -= EffectInvoke;
    }
    private void Initialize()
    {
        PlayMode = Enums.PlayMode.Both;
        OnMapEffect += EffectInvoke;
    }

    public void EffectInvoke(Note note, int combo)
    {

        if (note.judgementType == JudgementType.PERFECT)
        {
            Test<LightEffect>();
        }
        if (combo % 10 == 0)
        {
            Test<NeonEffect>();
        }

        if (combo % 20 == 0)
        {

        }

        if (note is TopNote)
        {

        }
    }

    public void Test<T>()
    {
        Type effectType = typeof(T);
        if (effects.TryGetValue(effectType, out var effectDelegate))
        {
            effectDelegate?.Invoke();
        }
    }

    public void SetPlayMode(Enums.PlayMode playMode)
    {
        SetDelegateNull();

        ConfigureEffectDelegates(playMode, lightEffect, neonEffect);

        UpdateEffectsDictionary();
    }

    private void SetDelegateNull()
    {
        lightEffect.effectDelegate = null;
        neonEffect.effectDelegate = null;
    }

    private void ConfigureEffectDelegates(Enums.PlayMode playMode, LightEffect light, NeonEffect neon)
    {
        if (playMode == Enums.PlayMode.Host || playMode == Enums.PlayMode.Both)
        {
            light.effectDelegate += light.LeftEffectInvoke;
            neon.effectDelegate += neon.LeftEffectInvoke;
        }

        if (playMode == Enums.PlayMode.Client || playMode == Enums.PlayMode.Both)
        {
            light.effectDelegate += light.RightEffectInvoke;
            neon.effectDelegate += neon.RightEffectInvoke;
        }

        if (playMode != Enums.PlayMode.Host &&
            playMode != Enums.PlayMode.Client &&
            playMode != Enums.PlayMode.Both)
        {
            Debug.LogError("PlayMode Error");
        }
    }
    private void UpdateEffectsDictionary()
    {
        effects.Add(lightEffect.GetType(), lightEffect.effectDelegate);
        effects.Add(neonEffect.GetType(), neonEffect.effectDelegate);
    }
}

