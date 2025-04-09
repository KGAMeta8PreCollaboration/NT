using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EffectManager : Singleton<EffectManager>
{
    [Header("Spot Light Effect")]
    [SerializeField] private LightEffect lightEffect;
    public Action<Note, int> OnMapEffect;
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
        lightEffect = FindObjectOfType<LightEffect>();
        OnMapEffect += EffectInvoke;
    }

    public void EffectInvoke(Note note, int combo)
    {

        Debug.Log(note.judgementType);
        if (note.judgementType == JudgementType.PERFECT)
        {
            lightEffect.LightsEffectOn();
        }
        if (combo % 10 == 0)
        {

        }

        if (combo % 20 == 0)
        {
        }

        if (note is TopNote)
        {

        }

    }

}
