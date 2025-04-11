using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MultiLobbyController : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPointPlayers;
    [SerializeField] private MultiLobbyUI _multiLobbyUI;

    [SerializeField] private VRPlayer _lobbyPlayer;

    private IEnumerator Start()
    {
        yield return null;
        GameManager.Instance.PhotonManager.joinedRoom += OnJoinedRoom;
        GameManager.Instance.PhotonManager.disconnectedServer += OnDisconnectedServer;
        GameManager.Instance.PhotonManager.leftRoomPlayer += OnLeftRoomPlayer;
    }

    public void OnJoinedRoom()
    {
        _lobbyPlayer.PlayerCameraAndAudioListenerActive(false);

        Transform playerTransform = GetPlayerSpawnPoint();
        GameManager.Instance.PhotonManager.SpawnPlayer("Multi/LobbyPlayer", playerTransform);

        _multiLobbyUI.CallLobbyUIUpdate();
    }


    public void OnDisconnectedServer()
    {
        _multiLobbyUI.ResetConnectImage();
        _lobbyPlayer.PlayerCameraAndAudioListenerActive(true);
    }

    public void OnLeftRoomPlayer(Player player)
    {
        _multiLobbyUI.UpdateConnectImage(player, true);
    }

    private void OnDestroy()
    {
        GameManager.Instance.PhotonManager.joinedRoom -= OnJoinedRoom;
        GameManager.Instance.PhotonManager.disconnectedServer -= OnDisconnectedServer;
        GameManager.Instance.PhotonManager.leftRoomPlayer -= OnLeftRoomPlayer;
    }

    private Transform GetPlayerSpawnPoint()
    {
        return _spawnPointPlayers[PhotonNetwork.LocalPlayer.NickName == "Player1" ? 0 : 1];
    }
}
