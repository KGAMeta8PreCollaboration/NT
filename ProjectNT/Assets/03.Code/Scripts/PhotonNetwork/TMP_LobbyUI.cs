using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TMP_LobbyUI : MonoBehaviour
{
    [SerializeField] private Button _multiLobbyButton;
    [SerializeField] private GameObject _multiLobbyUI;
    [SerializeField] private GameObject _player;

    private void Start()
    {
        _multiLobbyButton.onClick.AddListener(MultiLobbyButtonClick);
    }

    private void MultiLobbyButtonClick()
    {
        PhotonNetwork.ConnectUsingSettings(); // Photon 서버 연결
    }

    private void OnDestroy()
    {
        _multiLobbyButton.onClick.RemoveListener(MultiLobbyButtonClick);
    }
}
