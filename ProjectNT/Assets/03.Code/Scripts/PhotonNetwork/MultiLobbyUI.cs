using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiLobbyUI : MonoBehaviourPun
{
    [SerializeField] private Image _connectImagePlayer1;
    [SerializeField] private Image _connectImagePlayer2;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _prevSongButton;
    [SerializeField] private Button _nextSongButton;
    [SerializeField] private TextMeshProUGUI _countStartGame;
    [SerializeField] private int countStartGame = 5;
    [SerializeField] public GamePlayUI gamePlayUI;
    [Header("게임을 시작하기 위한 플레이어 수")]
    public int peopleCount = 2;


    private PopupManager _popupManager;

    private Coroutine _startGameCoroutine;
    private Coroutine _countStartGameCoroutine;

    [SerializeField] private List<Button> _gamePlayUIButtons = new List<Button>();

    private void Start()
    {

        _popupManager = FindObjectOfType<PopupManager>();

        _quitButton.onClick.AddListener(QuitButtonClick);
        _startButton.onClick.AddListener(StartButtonClick);
        _prevSongButton.onClick.AddListener(PrevSongButtonClick);
        _nextSongButton.onClick.AddListener(NextSongButtonClick);

        CountStartGameActive(false);
    }

    private void QuitButtonClick()
    {
        GameManager.Instance.PhotonManager.LeaveRoom();
    }

    private void StartButtonClick()
    {
        Debug.Log($"플레이어 수: {PhotonNetwork.PlayerList.Length}");
        if (PhotonNetwork.PlayerList.Length == peopleCount)
        {
            photonView.RPC(nameof(RPC_GameStart), RpcTarget.All);
        }
        else
        {
            _popupManager.OpenPopup<AlarmPopup>().SetPopup("플레이어 수가 부족합니다.", "확인");
        }
    }

    private void PrevSongButtonClick()
    {
        // 얘네를 동기화해야함
        //photonView.RPC(nameof(Temp), RpcTarget.All, gamePlayUI.musicChangeSelect.CurMusicData.musicName);
        photonView.RPC(nameof(RPC_PreviousMusicButton), RpcTarget.Others);
    }
    private void NextSongButtonClick()
    {
        //photonView.RPC(nameof(Temp), RpcTarget.OthersBuffered, gamePlayUI.musicChangeSelect.CurMusicData.musicName);

        photonView.RPC(nameof(RPC_NextMusicButton), RpcTarget.Others);
    }

    public void SendSetMusicNodeToString(string musicName)
    {
        print("SetMusicNodeToString");
        //gamePlayUI.musicChangeSelect.SetMusicNodeToString(musicName);
        photonView.RPC(nameof(RPC_SetMusicNodeToString), RpcTarget.Others, musicName);
    }

    [PunRPC]
    public void RPC_SetMusicNodeToString(string musicName)
    {
        gamePlayUI.musicChangeSelect.SetMusicNodeToString(musicName);
    }

    [PunRPC]
    private void RPC_GameStart()
    {
        if (_startGameCoroutine != null)
        {
            StopCoroutine(_startGameCoroutine);
            _startGameCoroutine = null;
        }


        ControlGamePlayUIButtonInteractable(false);

        CountStartGameActive(true);
        _countStartGame.text = ""; // 카운트 UI 초기화

        _startGameCoroutine = StartCoroutine(StartGameCoroutine());

        _popupManager.OpenPopup<AlarmPopup>().SetPopup(
            "곧 합주가 시작됩니다.",
            "취소",
            () => photonView.RPC(nameof(RPC_CancelStartGame), RpcTarget.All)
        );
    }

    private IEnumerator StartGameCoroutine()
    {
        _countStartGameCoroutine = StartCoroutine(CountStartGameCoroutine(countStartGame));
        yield return _countStartGameCoroutine;

        if (_startGameCoroutine != null && PhotonNetwork.PlayerList.Length == peopleCount)
        {
            _popupManager.ClosePopup<AlarmPopup>();

            var data = gamePlayUI.GetMultiGameStartData();
            GameManager.Instance.SetDataForMultiGameStart(data.beatMapData1, data.beatMapData2, data.projectPath, data.musicName);
            if (PhotonNetwork.IsMasterClient) GameManager.Instance.MultiGameStart();
        }
    }

    private IEnumerator CountStartGameCoroutine(int count)
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        for (int i = count; i > 0; i--)
        {
            _countStartGame.text = i.ToString();
            yield return wait;
        }
        _countStartGame.text = "";
    }

    [PunRPC]
    private void RPC_CancelStartGame()
    {
        CancelStartGame();
    }

    public void CancelStartGame()
    {
        if (_startGameCoroutine != null)
        {
            StopCoroutine(_startGameCoroutine);
            StopCoroutine(_countStartGameCoroutine);
            _startGameCoroutine = null;
            _countStartGameCoroutine = null;
        }

        Debug.Log("게임 시작이 취소되었습니다!");

        ControlGamePlayUIButtonInteractable(true);

        _countStartGame.text = "";
        CountStartGameActive(false);
        _popupManager.ClosePopup<AlarmPopup>();
    }

    public void CountStartGameActive(bool isActive)
    {
        _countStartGame.gameObject.SetActive(isActive);
    }

    public void UpdateConnectImage(Player player, bool isQuit)
    {
        if (player.NickName == "Player1")
        {
            _connectImagePlayer1.color = isQuit ? Color.red : Color.green;
        }
        else if (player.NickName == "Player2")
        {
            _connectImagePlayer2.color = isQuit ? Color.red : Color.green;
        }
    }

    public void ResetConnectImage()
    {
        _connectImagePlayer1.color = Color.red;
        _connectImagePlayer2.color = Color.red;
    }

    private void OnDestroy()
    {
        _quitButton.onClick.RemoveListener(QuitButtonClick);
        _startButton.onClick.RemoveListener(StartButtonClick);

        _prevSongButton.onClick.RemoveListener(PrevSongButtonClick);
        _nextSongButton.onClick.RemoveListener(NextSongButtonClick);
    }

    // 외부에서 전체 UI 초기화용 RPC 호출
    public void CallLobbyUIUpdate()
    {
        photonView.RPC(nameof(RPC_UpdateLobbyUI), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_UpdateLobbyUI()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            UpdateConnectImage(player, false);
        }
    }

    [PunRPC]
    private void RPC_NextMusicButton()
    {
        gamePlayUI.NextMusicButton();
    }

    [PunRPC]
    private void RPC_PreviousMusicButton()
    {
        gamePlayUI.PreviousMusicButton();
    }

    private void ControlGamePlayUIButtonInteractable(bool isOn)
    {
        foreach (Button button in _gamePlayUIButtons)
        {
            button.interactable = isOn;
        }
    }
}
