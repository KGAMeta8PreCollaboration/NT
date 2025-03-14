using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingUI : BaseTitleUI
{
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
    }

    public override void RemoveEventListeners()
    {
        base.RemoveEventListeners();
    }

    public override void CloseUIButtonClick()
    {
        base.CloseUIButtonClick();
    }
}
