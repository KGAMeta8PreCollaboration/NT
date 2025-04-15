using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSceneLightController : MonoBehaviour
{
    // [Header("1페이즈 오브젝트들")]
    // public GameSceneLightObject[] first;//1페이즈 빛날오브젝트들
    // [Header("2페이즈 오브젝트들")]
    // public GameSceneLightObject[] second;//2페이즈 빛날오브젝트들
    // [Header("3페이즈 오브젝트들")]
    // public GameSceneLightObject[] third;//3페이즈 빛날오브젝트들

    // public List<GameSceneLightObject> onLightObj;//빛날오브젝트들

    // [Header("시작 빛 강도")]
    // public float startIntensity;
    // [Header("빛 지속 시간")]
    // public float duration;

    // private void Awake()
    // {
    //     foreach (GameSceneLightObject obj in first)
    //     {
    //         obj.OffLight();
    //     }
    //     foreach (GameSceneLightObject obj in second)
    //     {
    //         obj.OffLight();
    //     }
    //     foreach (GameSceneLightObject obj in third)
    //     {
    //         obj.OffLight();
    //     }
    // }

    // public void AddFirstLightObject()//1페이즈 추가
    // {
    //     Debug.Log("1페이즈 시작");
    //     onLightObj.AddRange(first);
    // }    
    // public void AddSecondLightObject()//2페이즈 추가
    // {
    //     Debug.Log("2페이즈 시작");
    //     onLightObj.AddRange(second);
    // }
    // public void AddThirdLightObject()//3페이즈 추가
    // {
    //     Debug.Log("3페이즈 시작");
    //     onLightObj.AddRange(third);
    // }

    // public void RemoveFirstLightObject()//1페이즈 제거
    // {
    //     Debug.Log("1페이즈 시작");
    //     onLightObj.RemoveAll(lightObject => first.Contains(lightObject));
    // }
    // public void RemoveSecondLightObject()//2페이즈 제거
    // {
    //     Debug.Log("2페이즈 시작");
    //     onLightObj.RemoveAll(lightObject => second.Contains(lightObject));
    // }
    // public void RemoveThirdLightObject()//3페이즈 제거
    // {
    //     Debug.Log("3페이즈 시작");
    //     onLightObj.RemoveAll(lightObject => third.Contains(lightObject));
    // }

    // public void OnLight()
    // {
    //     Debug.Log($"{onLightObj} OnLight");
    //     foreach (GameSceneLightObject light in onLightObj)
    //     {
    //         light.StartFadeLight(startIntensity, duration);
    //     }
    // }
}
