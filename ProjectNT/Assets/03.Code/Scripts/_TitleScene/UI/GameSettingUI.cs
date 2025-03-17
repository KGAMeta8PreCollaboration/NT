using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingUI : BaseTitleUI
{
    public Button gameExitButton;

    public override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        AddEventListeners();
    }

    private void OnDisable()
    {
        RemoveEventListeners();
    }

    public override void AddEventListeners()
    {
        base.AddEventListeners();
        gameExitButton.onClick.AddListener(GameExit);
    }

    public override void RemoveEventListeners()
    {
        base.RemoveEventListeners();
        gameExitButton.onClick.RemoveListener(GameExit);
    }

    public override void CloseUIButtonClick()
    {
        base.CloseUIButtonClick();
    }

    public void GameExit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
