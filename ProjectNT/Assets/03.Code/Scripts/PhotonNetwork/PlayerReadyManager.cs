using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerReadyManager : MonoBehaviourPun
{
    private HashSet<int> _readyPlayers = new HashSet<int>();
    private int _totalPlayers;

    public Action AllPlayersReady;

    private void Start()
    {
        _totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        StartCoroutine(WaitUntilAllReady());
    }

    public void NotifyPlayerReady(int actorNumber)
    {
        if (!_readyPlayers.Contains(actorNumber))
        {
            photonView.RPC(nameof(RPC_PlayerReady), RpcTarget.AllBuffered, actorNumber);
        }
    }

    [PunRPC]
    private void RPC_PlayerReady(int actorNumber)
    {
        if (!_readyPlayers.Contains(actorNumber))
        {
            _readyPlayers.Add(actorNumber);
            print($"플레이어 {actorNumber} 준비 완료! ({_readyPlayers.Count}/{_totalPlayers})");
        }
    }

    private IEnumerator WaitUntilAllReady()
    {
        while (true)
        {
            if (_readyPlayers.Count == _totalPlayers)
            {
                AllPlayersReady?.Invoke();
                break;
            }
            yield return null;
        }
    }
}
