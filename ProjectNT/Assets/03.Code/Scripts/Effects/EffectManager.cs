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
    private StreetLampHandler lightHandler;
    private NeonHandler neonHandler;
    private CarHandler carHandler;
    private MeteorHandler meteorHandler;
    private FireplayHandler fireplayHandler;
    private LazerHandler lazerHandler;
    private WindowHandler windowHandler;
    private DisplayBoardHandler displayBoardHandler;

    public Action<Note, int, Enums.PlayMode> playerMapEffect;

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
        lightHandler = null;
        neonHandler = null;
        carHandler = null;
        windowHandler = null;
        meteorHandler = null;
        fireplayHandler = null;
        lazerHandler = null;
        SetActionNull();
    }

    private void Initialize()
    {
        FindEffectObjects();
        playerMapEffect += EffectInvoke;
    }

    // 게임 씬 진입 시 배치된 이펙트오브젝트 찾음
    private void FindEffectObjects()
    {
        lightHandler = FindObjectOfType<StreetLampHandler>();
        neonHandler = FindObjectOfType<NeonHandler>();
        carHandler = FindObjectOfType<CarHandler>();
        meteorHandler = FindObjectOfType<MeteorHandler>();
        fireplayHandler = FindObjectOfType<FireplayHandler>();
        lazerHandler = FindObjectOfType<LazerHandler>();
        windowHandler = FindObjectOfType<WindowHandler>();
        displayBoardHandler = FindObjectOfType<DisplayBoardHandler>();
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
                if (null != lightHandler)
                {
                    // 노트 퍼펙트
                    p1PerfectAct += lightHandler.P1EffectInvoke;
                    p2PerfectAct += lightHandler.P2EffectInvoke;
                }

                if (null != neonHandler)
                {
                    // 10 콤보
                    p1TenComboAct += neonHandler.P1EffectInvoke;
                    p2TenComboAct += neonHandler.P2EffectInvoke;
                }

                if (null != carHandler)
                {
                    // 20 콤보
                    p1TwentyComboAct += carHandler.P1EffectInvoke;
                    p2TwentyComboAct += carHandler.P2EffectInvoke;
                }

                if (null != windowHandler)
                {
                    // 상단 노트 클리어
                    p1TopNoteAct += windowHandler.P1EffectInvoke;
                    p2TopNoteAct += windowHandler.P2EffectInvoke;
                }
                break;
            // 페이즈 2 구독
            case Enums.Phase.Phase2:
                SetActionNull();
                Phase1End();

                if (windowHandler != null)
                {
                    // 노트 퍼펙트
                    p1PerfectAct += windowHandler.P1EffectInvoke;
                    p2PerfectAct += windowHandler.P2EffectInvoke;
                }

                if (lazerHandler != null)
                {
                    //10콤보
                    p1TenComboAct += lazerHandler.Play_S_P_2;
                    p1TenComboAct += lazerHandler.Play_M_L_P_2;
                    p2TenComboAct += lazerHandler.Play_S_P_2;
                    p2TenComboAct += lazerHandler.Play_M_R_P_2;

                    //상단 노트 클리어
                    p1TopNoteAct += lazerHandler.Play_S_P_3;
                    p1TopNoteAct += lazerHandler.Play_M_L_P_3;
                    p2TopNoteAct += lazerHandler.Play_S_P_3;
                    p2TopNoteAct += lazerHandler.Play_M_R_P_3;
                }

                if (displayBoardHandler != null)
                {
                    //20콤보
                    p1TwentyComboAct += displayBoardHandler.ChangeDisplayBoard;
                    p2TwentyComboAct += displayBoardHandler.ChangeDisplayBoard;
                }

                if (null == windowHandler) { break; }

                break;

            // 페이즈 3 구독
            case Enums.Phase.Phase3:
                SetActionNull();
                Phase2End();

                //퍼펙트
                if (fireplayHandler)
                {
                    p1PerfectAct += fireplayHandler.PlayFireplay;
                    p2PerfectAct += fireplayHandler.PlayFireplay;
                }

                //상단 노드
                if (meteorHandler)
                {
                    p1TopNoteAct += meteorHandler.PlayMeteor;
                    p2TopNoteAct += meteorHandler.PlayMeteor;
                }

                //10콤보
                //p1TenComboAct += displayBoardHandler.ChangeDisplayBoard;
                //p2TenComboAct += displayBoardHandler.ChangeDisplayBoard;

                //20콤보
                if (lazerHandler)
                {
                    p1TwentyComboAct += lazerHandler.Play_S_P_3;
                    p1TwentyComboAct += lazerHandler.Play_M_L_P_3;
                    p2TwentyComboAct += lazerHandler.Play_S_P_3;
                    p2TwentyComboAct += lazerHandler.Play_M_R_P_3;
                }

                break;
            default:
                Debug.LogError("PhaseEffect Error");
                break;
        }
    }

    private void Phase1End()
    {
        lightHandler?.LeftEffectEnd();
        lightHandler?.RightEffectEnd();

        neonHandler?.LeftEffectEnd();
        neonHandler?.RightEffectEnd();

        carHandler?.LeftEffectEnd();
        carHandler?.RightEffectEnd();
        carHandler?.MovePhase2Pos();
    }
    private void Phase2End()
    {
        carHandler?.MovePhase3Pos();
        // TODO 페이즈 종료 시 SetActionNull 메서드 호출하기 때문에 액션 구독해제 따로 안하셔도 됩니다.
        // -> 오홍 알겠습니다
        if (windowHandler != null)
        {
            windowHandler.LeftEffectEnd();
            windowHandler.RightEffectEnd();
        }

        //2페 끝나면 전광판을 이퀄라이져로 고정
        displayBoardHandler.SetDisplayEqualizer();
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

