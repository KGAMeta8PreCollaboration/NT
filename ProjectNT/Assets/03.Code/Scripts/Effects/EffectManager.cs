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
    private Action p1PerfectAct;
    private Action p2PerfectAct;
    private Action p1TenComboAct;
    private Action p2TenComboAct;
    private Action p1TwentyComboAct;
    private Action p2TwentyComboAct;
    private Action p1TopNoteAct;
    private Action p2TopNoteAct;
    private LightEffect lightEffect;
    private NeonEffect neonEffect;
    private CarEffect carEffect;
    private MeteorHandler meteorHandler;
    private FireplayHandler fireplayHandler;
    private LazerHandler lazerHandler;

    public Action<Note, int, Enums.PlayMode> player1MapEffect;
    // public Action<Note, int> player2MapEffect;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
            return;
        SceneManager.sceneLoaded += (x, y) =>
        {
            if (SceneManager.GetActiveScene().name == GameManager.Instance.gameSceneName || SceneManager.GetActiveScene().name == "MultiGame")
            {
                Initialize();
            }
            if (SceneManager.GetActiveScene().name == "LobbyScene")
            {
                SetNull();
            }
        };
    }

    // 로비씬 전환시 명시적 Null
    private void SetNull()
    {
        lightEffect = null;
        neonEffect = null;
        carEffect = null;
        SetActionNull();
    }

    private void Initialize()
    {
        FindEffectObjects();
        // player1MapEffect += EffectInvoke;
        // player2MapEffect += EffectInvoke;
        player1MapEffect += EffectInvoke;
    }

    // 게임 씬 진입 시 배치된 이펙트오브젝트 찾음
    private void FindEffectObjects()
    {
        lightEffect = FindObjectOfType<LightEffect>();
        neonEffect = FindObjectOfType<NeonEffect>();
        carEffect = FindObjectOfType<CarEffect>();
        meteorHandler = FindObjectOfType<MeteorHandler>();
        fireplayHandler = FindObjectOfType<FireplayHandler>();
        lazerHandler = FindObjectOfType<LazerHandler>();
    }

    public void EffectInvoke(Note note, int combo, Enums.PlayMode playMode)
    {

        if (note.judgementType == JudgementType.PERFECT)
        {
            InvokeByPlayMode(p1PerfectAct, p2PerfectAct, playMode);
        }
        if (combo % 10 == 0)
        {
            InvokeByPlayMode(p1TenComboAct, p2TenComboAct, playMode);
        }

        if (combo % 20 == 0)
        {
            InvokeByPlayMode(p1TwentyComboAct, p2TwentyComboAct, playMode);
        }

        if (note is TopNote)
        {
            print($"탑 노트인 것 까진 확인");
            InvokeByPlayMode(p1TopNoteAct, p2TopNoteAct, playMode);
        }
    }
    private void InvokeByPlayMode(Action p1Act, Action p2Act, Enums.PlayMode playMode)
    {
        switch (playMode)
        {
            case Enums.PlayMode.Single:
                p1Act?.Invoke();
                p2Act?.Invoke();
                break;
            case Enums.PlayMode.Player1:
                p1Act?.Invoke();
                break;
            case Enums.PlayMode.Player2:
                p2Act?.Invoke();
                break;
            default:
                Debug.LogWarning("Unhandled play mode: " + playMode);
                break;
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

                //퍼펙트 판정 구독

                //10콤보 구독

                //20콤보 구독
                p1TwentyComboAct += lazerHandler.Play_S_P_2;
                p1TwentyComboAct += lazerHandler.Play_M_L_P_2;
                p2TwentyComboAct += lazerHandler.Play_S_P_2;
                p2TwentyComboAct += lazerHandler.Play_M_R_P_2;

                //상단 노트 클리어 구독
                p1TopNoteAct += lazerHandler.Play_S_P_3;
                p1TopNoteAct += lazerHandler.Play_M_L_P_3;
                p2TopNoteAct += lazerHandler.Play_S_P_3;
                p2TopNoteAct += lazerHandler.Play_M_R_P_3;

                break;

            // 페이즈 3 구독
            case Enums.Phase.Phase3:
                SetActionNull();
                Phase2End();

                //퍼펙트 판정 구독
                p1PerfectAct += fireplayHandler.PlayFireplay;
                p2PerfectAct += fireplayHandler.PlayFireplay;

                //10콤보 구독

                //20콤보 구독
                p1TwentyComboAct += lazerHandler.Play_S_P_3;
                p1TwentyComboAct += lazerHandler.Play_M_L_P_3;
                p2TwentyComboAct += lazerHandler.Play_S_P_3;
                p2TwentyComboAct += lazerHandler.Play_M_R_P_3;

                //상단 노트 클리어 구독
                p1TopNoteAct += meteorHandler.PlayMeteor;
                p2TopNoteAct += meteorHandler.PlayMeteor;
                break;
            default:
                Debug.LogError("PhaseEffect Error");
                break;
        }
    }

    private void Phase1End()
    {
        lightEffect?.LeftEffectEnd();
        lightEffect?.RightEffectEnd();

        neonEffect?.LeftEffectEnd();
        neonEffect?.RightEffectEnd();

        carEffect?.LeftEffectEnd();
        carEffect?.RightEffectEnd();
        carEffect?.MovePhase2Pos();
    }
    private void Phase2End()
    {
        carEffect?.MovePhase3Pos();

        //퍼펙트 구독 해제

        //10콤보 구독 해제

        //20콤보구독 해제
        p1TwentyComboAct -= lazerHandler.Play_S_P_2;
        p1TwentyComboAct -= lazerHandler.Play_M_L_P_2;
        p2TwentyComboAct -= lazerHandler.Play_S_P_2;
        p2TwentyComboAct -= lazerHandler.Play_M_R_P_2;

        //상단 노트 클리어 구독 해제
        p1TopNoteAct -= lazerHandler.Play_S_P_3;
        p1TopNoteAct -= lazerHandler.Play_M_L_P_3;
        p2TopNoteAct -= lazerHandler.Play_S_P_3;
        p2TopNoteAct -= lazerHandler.Play_M_R_P_3;
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

