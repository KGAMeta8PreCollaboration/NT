using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] _spawnPointPlayers;
    [SerializeField] private MultiLobbyUI _multiLobbyUI;

    [SerializeField] private TextMeshProUGUI _logText;

    [SerializeField] private GameObject _player;


    [SerializeField] private GameObject _tmp_LobbyUI;
    [SerializeField] private Transform _lobbyPoint;

    public override void OnConnectedToMaster()
    {
        _logText.text = "Photon 연결 성공!";
        print(_logText.text);
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        print("로비 입장 성공!");
        PhotonNetwork.JoinOrCreateRoom("LocalVRRoom", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        _logText.text = "방 참가 성공!";
        print(_logText.text);

        _tmp_LobbyUI.SetActive(false);
        _multiLobbyUI.gameObject.SetActive(true);

        AssignPlayerRole();
    }

    public void LeaveRoom()
    {
        //현재 방 나가기
        PhotonNetwork.LeaveRoom();
    }

    //현재 방에서 나왔을 때 호출
    public override void OnLeftRoom()
    {
        print("방 나감");
        //현재 로비 나가기
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        else
        {
            PhotonNetwork.Disconnect();
        }
    }

    //현재 로비에서 나왔을 때 호출
    public override void OnLeftLobby()
    {
        print("로비 나감");
        //연결 끊기
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("포톤 연결 해제");
        _multiLobbyUI?.gameObject.SetActive(false);
        _tmp_LobbyUI?.SetActive(true);

        _player.transform.position = _lobbyPoint.position;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("플레이어 입장 시 실행되는 함수");
        UpdateMultiLobbyUI2(newPlayer, true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateMultiLobbyUI2(otherPlayer, true);
    }

    public void AssignPlayerRole()
    {
        if (PhotonNetwork.IsMasterClient) // 가장 먼저 들어온 플레이어 = Player1
        {
            PhotonNetwork.LocalPlayer.NickName = "Player1";
            _player.transform.position = _spawnPointPlayers[0].position;
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = "Player2";
            _player.transform.position = _spawnPointPlayers[1].position;
        }

        //photonView.RPC("UpdateMultiLobbyUI", RpcTarget.All);
        UpdateMultiLobbyUI2(PhotonNetwork.LocalPlayer, false);
    }

    [PunRPC]
    public void UpdateMultiLobbyUI()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == "Player1")
            {
                _multiLobbyUI.connectImagePlayer1.color = Color.green; // 초록색으로 변경
            }
            else if (player.NickName == "Player2")
            {
                _multiLobbyUI.connectImagePlayer2.color = Color.green; // 초록색으로 변경
            }
        }
    }

    public void UpdateMultiLobbyUI2(Player player, bool isQuit)
    {
        if (player.NickName == "Player1")
        {
            _multiLobbyUI.connectImagePlayer1.color = isQuit == false ? Color.green : Color.red; // 초록색으로 변경
        }
        else if (player.NickName == "Player2")
        {
            _multiLobbyUI.connectImagePlayer2.color = isQuit == false ? Color.green : Color.red;  // 초록색으로 변경
        }
    }


    void SpawnOtherPlayer()
    {
        _logText.text = "플레이어 프리팹 생성";
        print(_logText.text);
        PhotonNetwork.Instantiate("Multi/Player", Vector3.zero, Quaternion.identity);
    }
}
