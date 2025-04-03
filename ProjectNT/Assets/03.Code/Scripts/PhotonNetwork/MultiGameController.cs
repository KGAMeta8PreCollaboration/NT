using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiGameController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] _spawnPointPlayers;

    private void Start()
    {
        Transform playerTransform = GetPlayerSpawnPoint();
        GameManager.Instance.PhotonManager.SpawnPlayer("Multi/GamePlayer", playerTransform);
    }

    private Transform GetPlayerSpawnPoint()
    {
        return _spawnPointPlayers[PhotonNetwork.LocalPlayer.NickName == "Player1" ? 0 : 1];
    }
}
