using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public Action joinedRoom; //방에 플레이어가 입장 했을 때 호출
    public Action disconnectedServer; //서버 연결이 해제될 때 호출
    public Action<Player> leftRoomPlayer; //플레이어가 방을 나갔을 때 호출

    public MultiLobbyUI multiLobbyUI;

    private void Start()
    {
        SceneManager.sceneLoaded += LobbySceneLoaded;
    }

    private void LobbySceneLoaded(Scene cur, LoadSceneMode arg1)
    {
        if (cur.name == "LobbyScene")
        {
            multiLobbyUI = FindObjectOfType<MultiLobbyUI>();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= LobbySceneLoaded;
    }

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
        AssignPlayerRole(); //플레이어 닉네임 설정
        joinedRoom?.Invoke();
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
        PhotonNetwork.Disconnect(); //연결 끊기
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LocalPlayer.NickName = ""; //플레이어 닉네임 초기화
        print("포톤 연결 해제");
        print($"로컬 플레이어: {PhotonNetwork.LocalPlayer.NickName}");
        PhotonNetwork.AutomaticallySyncScene = false;
        disconnectedServer?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("들어온 플레이어: " + newPlayer.NickName);
        photonView.RPC(nameof(SetMusicNodeToString), newPlayer, multiLobbyUI, multiLobbyUI.gamePlayUI.musicChangeSelect.CurMusicData.musicName);
    }

    [PunRPC]
    public void SetMusicNodeToString(string musicName)
    {
        multiLobbyUI.SetMusicNodeToString(musicName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print("나간 플레이어: " + otherPlayer.NickName);
        otherPlayer.NickName = "";
        leftRoomPlayer?.Invoke(otherPlayer);
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

    public GameObject SpawnPlayer(string path, Transform spawnPoint)
    {
        if (spawnPoint == null)
        {
            Debug.LogError("스폰 위치가 지정되지 않았습니다!");
            return null;
        }

        print("플레이어 컨트롤러 생성");

        return PhotonNetwork.Instantiate(path, spawnPoint.position, spawnPoint.rotation);
    }
}
