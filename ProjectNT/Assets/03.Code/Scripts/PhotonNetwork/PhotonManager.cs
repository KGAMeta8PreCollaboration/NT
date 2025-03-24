using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] _spawnPointPlayers;
    [SerializeField] private MultiLobbyUI _multiLobbyUI;

    [SerializeField] private VRPlayer _lobbyPlayer;

    public override void OnConnectedToMaster()
    {
        print("Photon 연결 성공!");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        print("로비 입장 성공!");
        PhotonNetwork.JoinOrCreateRoom("LocalVRRoom", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {

        print("방 참가 성공!");

        PhotonNetwork.AutomaticallySyncScene = true;

        _lobbyPlayer.GetComponent<VRPlayer>().PlayerCameraAndAudioListenerActive(false);

        AssignPlayerRole();
        SpawnPlayer();

        photonView.RPC("UpdateMultiLobbyUI", RpcTarget.All);
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
        _multiLobbyUI.ResetConnectImage();
        PhotonNetwork.LocalPlayer.NickName = "";
        print(PhotonNetwork.LocalPlayer.NickName);

        _lobbyPlayer.GetComponent<VRPlayer>().PlayerCameraAndAudioListenerActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("들어온 플레이어: " + newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print("나간 플레이어: " + otherPlayer.NickName);
        _multiLobbyUI.UpdateConnectImage(otherPlayer, true);
        otherPlayer.NickName = "";
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        print($"{newMasterClient.NickName}이 새로운 마스터가 됌.");
    }

    public void AssignPlayerRole()
    {
        // 현재 방에 있는 플레이어들 가져오기
        List<Player> players = new List<Player>(PhotonNetwork.PlayerList);
        print(players.Count);
        // 기존 역할이 남아있는지 확인
        bool isPlayer1Assigned = players.Exists(p => p.NickName == "Player1");
        bool isPlayer2Assigned = players.Exists(p => p.NickName == "Player2");

        if (!isPlayer1Assigned) // Player1 역할이 비어있으면 현재 플레이어를 Player1로 지정
        {
            PhotonNetwork.LocalPlayer.NickName = "Player1";
            Debug.Log("새로운 Player1 설정됨: " + PhotonNetwork.LocalPlayer.NickName);
        }
        else if (!isPlayer2Assigned) // Player2 역할이 비어있으면 현재 플레이어를 Player2로 지정
        {
            PhotonNetwork.LocalPlayer.NickName = "Player2";
            Debug.Log("새로운 Player2 설정됨: " + PhotonNetwork.LocalPlayer.NickName);
        }
    }

    [PunRPC]
    public void UpdateMultiLobbyUI()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            _multiLobbyUI.UpdateConnectImage(player, false);
        }
    }

    [PunRPC]
    public void GameStart()
    {
        _multiLobbyUI.GameStart();
    }

    [PunRPC]
    public void CancelStartGame()
    {
        _multiLobbyUI.CancelStartGame();
    }



    private void SpawnPlayer()
    {
        print("플레이어 컨트롤러 생성");
        if (PhotonNetwork.LocalPlayer.NickName == "Player1")
        {
            PhotonNetwork.Instantiate("Multi/LobbyPlayer", _spawnPointPlayers[0].position, _spawnPointPlayers[0].rotation).GetComponent<VRPlayer>();
        }
        else if (PhotonNetwork.LocalPlayer.NickName == "Player2")
        {

            PhotonNetwork.Instantiate("Multi/LobbyPlayer", _spawnPointPlayers[1].position, _spawnPointPlayers[1].rotation).GetComponent<VRPlayer>();
        }
    }
}
