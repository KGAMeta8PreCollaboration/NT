using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _player1ConnectText;
    [SerializeField] private TextMeshProUGUI _player2ConnectText;
    [SerializeField] private TextMeshProUGUI _logText;


    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Photon 서버 연결
    }

    public override void OnConnectedToMaster()
    {
        _logText.text = "Photon 연결 성공!";
        PhotonNetwork.JoinOrCreateRoom("LocalVRRoom", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        _logText.text = "방 참가 성공!";
        SpawnPlayer();
    }


    void SpawnPlayer()
    {
        _logText.text = "플레이어 프리팹 생성";
        //PhotonNetwork.Instantiate("Multi/PlayerPrefab", Vector3.zero, Quaternion.identity);
    }
}
