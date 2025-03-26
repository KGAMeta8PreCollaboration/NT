using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.UI;

public class EndGameMenuPopup : Popup
{
    [SerializeField] private Button _restartButton;

    public override void Init(PopupManager popupManager)
    {
        base.Init(popupManager);
        _restartButton?.onClick.AddListener(RestartButtonClick);
    }

    public override void CloseButtonClick()
    {
        base.CloseButtonClick();
        GameManager.Instance.GoToLobby();
    }

    private void RestartButtonClick()
    {

    }
}
