using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneLightController : MonoBehaviour
{
    [Header("1페이즈 오브젝트들")]
    public GameSceneLightObject[] first;//1페이즈 빛날오브젝트들
    [Header("2페이즈 오브젝트들")]
    public GameSceneLightObject[] second;//2페이즈 빛날오브젝트들
    [Header("3페이즈 오브젝트들")]
    public GameSceneLightObject[] third;//3페이즈 빛날오브젝트들

    private GameSceneLightObject[] curPhase;//1페이즈 빛날오브젝트들

    [Header("시작 빛 강도")]
    public float startIntensity;
    [Header("빛 지속 시간")]
    public float duration;

    private void Awake()
    {
        foreach (GameSceneLightObject obj in first)
        {
            obj.OffLight();
        }
        foreach (GameSceneLightObject obj in second)
        {
            obj.OffLight();
        }
        foreach (GameSceneLightObject obj in third)
        {
            obj.OffLight();
        }
    }

    public void ChangeLightObject(int phase)
    {
        switch(phase)
        {
            case 1:
                Debug.Log("1페이즈 시작");
                curPhase = first;
                break;
            case 2:
                Debug.Log("1페이즈 시작");
                curPhase = second;
                break;
            case 3:
                Debug.Log("1페이즈 시작");
                curPhase = third;
                break;
            default:
                Debug.Log("ChangeLightObject 실패");
                break;
        }
    }

    public void OnLight()
    {
        Debug.Log($"{curPhase} OnLight");
        foreach (GameSceneLightObject light in curPhase)
        {
            light.StartFadeLight(startIntensity, duration);
        }
    }
}
