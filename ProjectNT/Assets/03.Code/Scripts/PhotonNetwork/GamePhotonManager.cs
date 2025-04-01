using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] _spawnPointPlayers;

    private void Awake()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        print("플레이어 컨트롤러 생성");
        if (PhotonNetwork.LocalPlayer.NickName == "Player1")
        {
            PhotonNetwork.Instantiate("Multi/GamePlayer", _spawnPointPlayers[0].position, _spawnPointPlayers[0].rotation);
        }
        else if (PhotonNetwork.LocalPlayer.NickName == "Player2")
        {

            PhotonNetwork.Instantiate("Multi/GamePlayer", _spawnPointPlayers[1].position, _spawnPointPlayers[1].rotation);
        }
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
        print("게임 나감");
        //연결 끊기
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("포톤 연결 해제");
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LocalPlayer.NickName = "";
        print(PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("들어온 플레이어: " + newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print("나간 플레이어: " + otherPlayer.NickName);
    }
}
