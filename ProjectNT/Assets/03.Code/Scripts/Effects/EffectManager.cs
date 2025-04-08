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
    public float lightIntensity = 0;
    public float lightDefaultIntensity = 0;
    public float lightDuration = 0;

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

    private void Initialize()
    {
        lightEffect = FindObjectOfType<LightEffect>();
        LightEffectInit();
    }

    public void EffectInvoke(Note note, JudgementType judgementType, int combo)
    {
        if (judgementType == JudgementType.PERFECT)
        {
            lightEffect.LeftLightEffectOn();
            lightEffect.RightLightEffectOn();
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

    private void LightEffectInit()
    {
        lightEffect.Intensity = lightIntensity;
        lightEffect.Duration = lightDuration;
        lightEffect.DefaultIntensity = lightDefaultIntensity;
    }
}
