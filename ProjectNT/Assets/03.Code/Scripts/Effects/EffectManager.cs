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

    public Dictionary<Type, EffectDelegate> leftEffects = new Dictionary<Type, EffectDelegate>();
    public Dictionary<Type, EffectDelegate> rightEffects = new Dictionary<Type, EffectDelegate>();
    public Action<Note, int> player1MapEffect;
    public Action<Note, int> player2MapEffect;
    public LightEffect lightEffect;
    public NeonEffect neonEffect;
    public CarEffect carEffect;
    protected override void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += (x, y) =>
        {
            if (SceneManager.GetActiveScene().name == GameManager.Instance.gameSceneName)
            {
                Initialize();
            }
            if (SceneManager.GetActiveScene().name == "LobbyScene")
            {
                SetEffectObjectNull();
                player1MapEffect -= EffectInvoke;
                player2MapEffect -= EffectInvoke;
            }
        };
    }
    private void Initialize()
    {
        FindEffectObjects();
        player1MapEffect += EffectInvoke;
        player2MapEffect += EffectInvoke;
    }

    private void SetEffectObjectNull()
    {
        lightEffect = null;
        neonEffect = null;
        carEffect = null;
    }
    private void FindEffectObjects()
    {
        lightEffect = FindObjectOfType<LightEffect>();
        neonEffect = FindObjectOfType<NeonEffect>();
        carEffect = FindObjectOfType<CarEffect>();
    }
    public void EffectInvoke(Note note, int combo)
    {

        if (note.judgementType == JudgementType.PERFECT)
        {
            GenericEffectOn<LightEffect>();
        }
        Debug.LogError(combo);
        if (combo % 10 == 0)
        {
            GenericEffectOn<NeonEffect>();
            Debug.LogError("!!");
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
        if (leftEffects.TryGetValue(effectType, out var leftDelegate))
        {
            leftDelegate?.Invoke();
        }
        if (rightEffects.TryGetValue(effectType, out var rightDelegate))
        {
            rightDelegate?.Invoke();
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
        lightEffect.player1Delegate = null;
        neonEffect.player1Delegate = null;
        carEffect.player1Delegate = null;

        lightEffect.player2Delegate = null;
        neonEffect.player2Delegate = null;
        carEffect.player2Delegate = null;
    }

    private void ConfigureEffectDelegates(Enums.PlayMode playMode, LightEffect light, NeonEffect neon, CarEffect car)
    {
        if (playMode == Enums.PlayMode.Player1 || playMode == Enums.PlayMode.Single)
        {
            light.player1Delegate += light.LeftEffectInvoke;
            neon.player1Delegate += neon.LeftEffectInvoke;
            car.player1Delegate += car.LeftEffectInvoke;
        }

        if (playMode == Enums.PlayMode.Player2 || playMode == Enums.PlayMode.Single)
        {
            light.player2Delegate += light.RightEffectInvoke;
            neon.player2Delegate += neon.RightEffectInvoke;
            car.player2Delegate += car.RightEffectInvoke;
        }

        if (playMode != Enums.PlayMode.Player1 &&
            playMode != Enums.PlayMode.Player2 &&
            playMode != Enums.PlayMode.Single)
        {
            Debug.LogError("PlayMode Error");
        }
    }
    private void UpdateEffectsDictionary()
    {
        leftEffects.Add(lightEffect.GetType(), lightEffect.player1Delegate);
        rightEffects.Add(lightEffect.GetType(), lightEffect.player2Delegate);
        leftEffects.Add(neonEffect.GetType(), neonEffect.player1Delegate);
        rightEffects.Add(neonEffect.GetType(), neonEffect.player2Delegate);
        leftEffects.Add(carEffect.GetType(), carEffect.player1Delegate);
        rightEffects.Add(carEffect.GetType(), carEffect.player2Delegate);
    }
}

