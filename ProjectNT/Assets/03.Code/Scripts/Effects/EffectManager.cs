using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public CarEffect carEffect;
    protected override void Awake()
    {
        base.Awake();

        SceneManager.activeSceneChanged += (x, y) =>
        {
            if (SceneManager.GetActiveScene().name == GameManager.Instance.gameSceneName)
            {
                Initialize();
            }
            if (SceneManager.GetActiveScene().name == "LobbyScene")
            {
                OnMapEffect -= EffectInvoke;

            }
        };
    }
    private void Initialize()
    {
        // lightEffect =
        PlayMode = Enums.PlayMode.Both;
        OnMapEffect += EffectInvoke;
    }

    public void EffectInvoke(Note note, int combo)
    {

        if (note.judgementType == JudgementType.PERFECT)
        {
            GenericEffectOn<LightEffect>();
        }
        if (combo % 10 == 0)
        {
            GenericEffectOn<NeonEffect>();
        }
        if (combo % 20 == 0)
        {
            GenericEffectOn<CarEffect>();
        }

        if (note is TopNote)
        {

        }
    }

    public void GenericEffectOn<T>()
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

        ConfigureEffectDelegates(playMode, lightEffect, neonEffect, carEffect);

        UpdateEffectsDictionary();
    }

    private void SetDelegateNull()
    {
        lightEffect.effectDelegate = null;
        neonEffect.effectDelegate = null;
        carEffect.effectDelegate = null;
    }

    private void ConfigureEffectDelegates(Enums.PlayMode playMode, LightEffect light, NeonEffect neon, CarEffect car)
    {
        if (playMode == Enums.PlayMode.Host || playMode == Enums.PlayMode.Both)
        {
            light.effectDelegate += light.LeftEffectInvoke;
            neon.effectDelegate += neon.LeftEffectInvoke;
            car.effectDelegate += car.LeftEffectInvoke;
        }

        if (playMode == Enums.PlayMode.Client || playMode == Enums.PlayMode.Both)
        {
            light.effectDelegate += light.RightEffectInvoke;
            neon.effectDelegate += neon.RightEffectInvoke;
            car.effectDelegate += car.RightEffectInvoke;
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
        effects.Add(carEffect.GetType(), carEffect.effectDelegate);
    }
}

