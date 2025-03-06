using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public static TitleManager instance;

    public BlurEffectManager blurEffectManager;
    public bool isComplete = false;
    public bool isUIActive = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        blurEffectManager.ResetTitle();
        blurEffectManager.FadeOutStart(FadeOutEnd);
    }

    void Update()
    {
        
    }

    private void FadeOutEnd()
    {
        isComplete = true;
        Debug.Log("페이드 아웃 완료!");
    }
}
