using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiGameController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] _spawnPointPlayers;

    private void Start()
    {
        Transform playerTransform = GetPlayerSpawnPoint();
        GameManager.Instance.PhotonManager.SpawnPlayer("Multi/GamePlayer", playerTransform);
        GameManager.Instance.PhotonManager.disconnectedServer += GotoLobbyScene;
    }

    private void OnDestroy()
    {
        GameManager.Instance.PhotonManager.disconnectedServer -= GotoLobbyScene;
    }

    public void GotoLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    private Transform GetPlayerSpawnPoint()
    {
        return _spawnPointPlayers[PhotonNetwork.LocalPlayer.NickName == "Player1" ? 0 : 1];
    }
}
