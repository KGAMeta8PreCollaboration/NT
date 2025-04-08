using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiGameController : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPointPlayers;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;

        Transform playerTransform = GetPlayerSpawnPoint();
        GameManager.Instance.PhotonManager.SpawnPlayer("Multi/GamePlayer", playerTransform);
        GameManager.Instance.PhotonManager.disconnectedServer += GotoLobbyScene;

        StartCoroutine(EnoughReadyPlayers());
        NotifyPlayerReady(PhotonNetwork.LocalPlayer.ActorNumber);
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

    //===============================================================
    private PhotonView _photonView;
    private HashSet<int> _readyPlayers = new HashSet<int>();
    private int _totalPlayers;

    public void NotifyPlayerReady(int actorNumber)
    {
        if (!_readyPlayers.Contains(actorNumber))
        {
            _photonView.RPC(nameof(RPC_PlayerReady), RpcTarget.AllBuffered, actorNumber);
        }
    }

    [PunRPC]
    private void RPC_PlayerReady(int actorNumber)
    {
        if (!_readyPlayers.Contains(actorNumber))
        {
            _readyPlayers.Add(actorNumber);
            Debug.Log($"플레이어 {actorNumber} 준비 완료! ({_readyPlayers.Count}/{_totalPlayers})");
        }
    }

    private IEnumerator EnoughReadyPlayers()
    {
        while (true)
        {
            if (_readyPlayers.Count == _totalPlayers)
            {
                _photonView.RPC("StartGameForAll", RpcTarget.All);
                break;
            }
            yield return null;
        }
    }

    [PunRPC]
    private void StartGameForAll()
    {
        Debug.Log("모든 플레이어 준비 완료 → 게임 시작!");

        StopCoroutine(GameManager.Instance.GameSceneInitCo());
        // 실제 게임 시작
        StartCoroutine(GameManager.Instance.GameSceneInitCo());
    }
    //===============================================================
}
