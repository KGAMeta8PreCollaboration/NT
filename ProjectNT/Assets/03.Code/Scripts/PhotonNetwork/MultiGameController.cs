using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiGameController : MonoBehaviour
{
    private PlayerModuleManager _playerModuleManager;
    private PlayerReadyManager _playerReadyManager;
    private MultiScoreManager _multiScoreManager;

    private void Awake()
    {
        Init();
    }

    private void OnDestroy()
    {
        GameManager.Instance.PhotonManager.disconnectedServer -= GotoLobbyScene;
        _playerReadyManager.AllPlayersReady -= StartGameForAll;
    }

    public void SetupAndReady(List<LoadedNoteData> player1Data, List<LoadedNoteData> player2Data)
    {
        _playerModuleManager.SetPlayerModuleData(player1Data, player2Data);
        _playerReadyManager.NotifyPlayerReady(PhotonNetwork.LocalPlayer.ActorNumber);
    }

    public Woofer GetPlayerWoofer(int wooferIndex, string nickname) { return _playerModuleManager.GetPlayerWoofer(wooferIndex, nickname); }

    public int GetWooferIndex(Woofer woofer, string nickname) { return _playerModuleManager.GetWooferIndex(woofer, nickname); }

    public TopNote GetPlayerTopNote(int index, string nickname) { return _playerModuleManager.GetPlayerTopNote(index, nickname); }

    public int GetTopNoteIndex(TopNote topNote, string nickname) { return _playerModuleManager.GetTopNoteIndex(topNote, nickname); }

    public PlayerModule GetPlayerModuleByNick(string nickname) { return _playerModuleManager.GetPlayerModuleByNick(nickname); }
    private void StartGameForAll()
    {
        Debug.Log("모든 플레이어 준비 완료 → 게임 시작!");
        StopCoroutine(GameManager.Instance.GameSceneInitCo());
        StartCoroutine(GameManager.Instance.GameSceneInitCo());
    }

    public void GotoLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    private void Init()
    {
        _playerModuleManager = GetComponentInChildren<PlayerModuleManager>();
        _playerReadyManager = GetComponentInChildren<PlayerReadyManager>();
        _multiScoreManager = GetComponentInChildren<MultiScoreManager>();

        GameManager.Instance.PhotonManager.disconnectedServer += GotoLobbyScene;

        _playerReadyManager.AllPlayersReady += StartGameForAll;
    }
}

