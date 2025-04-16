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

    public PlayerResultContainer GetPlayerResultContainer(string nickname) { return _multiScoreManager.GetPlayerResultContainer(nickname); }

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

        GameManager.Instance.OnGameEnd += () =>
        {
            StartCoroutine(WaitForAllResultsThenShowUI());
        };
    }

    private IEnumerator WaitForAllResultsThenShowUI()
    {
        _multiScoreManager.SendMyResult();

        yield return new WaitUntil(() =>
            _multiScoreManager.ReceivedPlayerCount == PhotonNetwork.PlayerList.Length
        );

        Debug.Log("모든 결과 수신 완료.");

        PopupManager popupManager = FindObjectOfType<PopupManager>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            string nickname = player.NickName;
            if (!_multiScoreManager.TryGetPlayerResultContainer(nickname, out PlayerResultContainer resultContainer))
            {
                Debug.LogWarning($"{nickname}의 결과를 찾을 수 없습니다.");
                continue;
            }

            ResultPanel resultPanel = GameManager.Instance.MultiGameController
                .GetPlayerModuleByNick(nickname)
                .GetComponentInChildren<ResultPanel>();

            resultPanel.SetResult(resultContainer);
            popupManager.OpenPopup(resultPanel);
        }
    }
}

