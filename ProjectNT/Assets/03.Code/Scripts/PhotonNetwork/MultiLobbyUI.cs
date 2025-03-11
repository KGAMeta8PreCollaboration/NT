using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiLobbyUI : MonoBehaviour
{
    [SerializeField] public Image connectImagePlayer1;
    [SerializeField] public Image connectImagePlayer2;
    [SerializeField] public Button _quitButton;
    [SerializeField] private PhotonManager _photonManager;

    private void Start()
    {
        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    private void QuitButtonClick()
    {
        _photonManager.LeaveRoom();
    }

    private void OnDestroy()
    {
        _quitButton.onClick.RemoveListener(QuitButtonClick);
    }
}
