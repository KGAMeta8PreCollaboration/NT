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
    public Dictionary<Type, EffectDelegate> leftEffects = new Dictionary<Type, EffectDelegate>();
    public Dictionary<Type, EffectDelegate> rightEffects = new Dictionary<Type, EffectDelegate>();
    public Action<Note, int> player1MapEffect;
    public Action<Note, int> player2MapEffect;
    public Action p1PerfectAct;
    public Action p2PerfectAct;
    public Action p1TenComboAct;
    public Action p2TenComboAct;
    public Action p1TwentyComboAct;
    public Action p2TwentyComboAct;
    public Action p1TopNoteAct;
    public Action p2TopNoteAct;
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
            }
        };
    }

    // 로비씬 전환시 명시적 Null
    private void SetEffectObjectNull()
    {
        lightEffect = null;
        neonEffect = null;
        carEffect = null;
        SetActionNull();
    }

    private void Initialize()
    {
        FindEffectObjects();
        player1MapEffect += EffectInvoke;
        player2MapEffect += EffectInvoke;
    }

    // 게임 씬 진입 시 배치된 이펙트오브젝트 찾음
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
            p1PerfectAct?.Invoke();
            p2PerfectAct?.Invoke();
        }
        if (combo % 10 == 0)
        {
            p1TenComboAct?.Invoke();
            p2TenComboAct?.Invoke();
        }

        if (combo % 20 == 0)
        {
            p1TwentyComboAct?.Invoke();
            p2TwentyComboAct?.Invoke();
        }

        if (note is TopNote)
        {
            p1TopNoteAct?.Invoke();
            p2TopNoteAct?.Invoke();
        }
    }

    public void SetPhaseEffect(Enums.Phase phase)
    {
        switch (phase)
        {
            // 페이즈 1 구독
            case Enums.Phase.Phase1:
                SetActionNull();

                if (null == lightEffect) { break; }
                p1PerfectAct += lightEffect.P1EffectInvoke;
                p2PerfectAct += lightEffect.P2EffectInvoke;

                if (null == neonEffect) { break; }
                p1TenComboAct += neonEffect.P1EffectInvoke;
                p2TenComboAct += neonEffect.P2EffectInvoke;

                if (null == carEffect) { break; }
                p1TwentyComboAct += carEffect.P1EffectInvoke;
                p2TwentyComboAct += carEffect.P2EffectInvoke;

                break;
            // 페이즈 2 구독
            case Enums.Phase.Phase2:
                SetActionNull();
                Phase1End();
                break;

            // 페이즈 3 구독
            case Enums.Phase.Phase3:
                SetActionNull();
                Phase2End();

                break;
            default:
                Debug.LogError("PhaseEffect Error");
                break;
        }
    }

    private void Phase1End()
    {
        lightEffect.LeftEffectEnd();
        lightEffect.RightEffectEnd();
        neonEffect.LeftEffectEnd();
        neonEffect.RightEffectEnd();
        carEffect.LeftEffectEnd();
        carEffect.RightEffectEnd();
        carEffect.MovePhase2Pos();
    }
    private void Phase2End()
    {
        carEffect.MovePhase3Pos();
    }

    private void SetActionNull()
    {
        p1PerfectAct = null;
        p2PerfectAct = null;

        p1TenComboAct = null;
        p2TenComboAct = null;

        p1TwentyComboAct = null;
        p2TwentyComboAct = null;

        p1TopNoteAct = null;
        p2TopNoteAct = null;
    }
}

