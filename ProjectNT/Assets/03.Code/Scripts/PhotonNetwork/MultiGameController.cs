using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiGameController : MonoBehaviour
{
    public PlayerModule[] playerModules;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        GameManager.Instance.PhotonManager.disconnectedServer += GotoLobbyScene;

        StartCoroutine(EnoughReadyPlayers());
    }

    private void OnDestroy()
    {
        GameManager.Instance.PhotonManager.disconnectedServer -= GotoLobbyScene;
    }

    public void SetPlayerModuleData(List<LoadedNoteData> player1SongData, List<LoadedNoteData> player2SongData)
    {
        foreach (PlayerModule playerModule in playerModules)
        {
            playerModule.SetPlayerModuleData(PhotonNetwork.LocalPlayer.NickName == "Player1" ? player1SongData : player2SongData);
        }

        _totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        NotifyPlayerReady(PhotonNetwork.LocalPlayer.ActorNumber);
    }

    public void GotoLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
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
                _photonView.RPC(nameof(StartGameForAll), RpcTarget.All);
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
