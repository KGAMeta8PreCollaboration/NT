using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
