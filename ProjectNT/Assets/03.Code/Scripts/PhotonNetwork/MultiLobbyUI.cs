using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiLobbyUI : MonoBehaviour
{
    [SerializeField] private Image _connectImagePlayer1;
    [SerializeField] private Image _connectImagePlayer2;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private PhotonManager _photonManager;
    [SerializeField] private TextMeshProUGUI _countStartGame;

    public int countStartGame;

    private PopupManager _popupManager;

    private Coroutine _startGameCoroutine;
    private Coroutine _countStartGameCoroutine;

    [SerializeField] private GamePlayUI ui;

    private void Start()
    {
        _popupManager = FindObjectOfType<PopupManager>();

        _quitButton.onClick.AddListener(QuitButtonClick);
        _startButton.onClick.AddListener(StartButtonClick);

        CountStartGameActive(false);
    }

    private void QuitButtonClick()
    {
        _photonManager.LeaveRoom();
    }

    private void StartButtonClick()
    {
        print($"플레이어 수: {PhotonNetwork.PlayerList.Length}");
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            //_startGameCoroutine = StartCoroutine(StartGameCoroutine());
            ////PopupManager.Instance.OpenPopup<AlarmPopup>().SetPopup("곧 합주가 시작됩니다.", "취소", CancelStartGame);
            //_photonManager.photonView.RPC("ShowStartGameAlarmPopupForAll", RpcTarget.All);

            _photonManager.photonView.RPC("GameStart", RpcTarget.All);

        }
        else
        {
            _popupManager.OpenPopup<AlarmPopup>().SetPopup("플레이어 수가 부족합니다.", "확인");
        }
    }

    public void GameStart()
    {
        if (_startGameCoroutine != null)
        {
            StopCoroutine(_startGameCoroutine);
        }

        CountStartGameActive(true);
        _countStartGame.text = ""; // 카운트 UI 초기화

        _startGameCoroutine = StartCoroutine(StartGameCoroutine());
        _popupManager.OpenPopup<AlarmPopup>().SetPopup("곧 합주가 시작됩니다.", "취소", () => _photonManager.photonView.RPC("CancelStartGame", RpcTarget.All));
    }

    private IEnumerator StartGameCoroutine()
    {
        _countStartGameCoroutine = StartCoroutine(CountStartGameCoroutine(countStartGame));
        yield return _countStartGameCoroutine;

        if (_startGameCoroutine != null) // 취소되지 않았는지 확인
        {
            PhotonNetwork.LoadLevel("LSH_MultiGame");
        }
    }

    private IEnumerator CountStartGameCoroutine(int count)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

        for (int i = count; i > 0; i--)
        {
            _countStartGame.text = i.ToString();
            yield return waitForSeconds;
        }

        _countStartGame.text = ""; // 게임 시작 직전에 UI 초기화
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

        _countStartGame.text = ""; // 취소 시 UI 초기화
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
            _connectImagePlayer1.color = isQuit == false ? Color.green : Color.red;
        }
        else if (player.NickName == "Player2")
        {
            _connectImagePlayer2.color = isQuit == false ? Color.green : Color.red;
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
    }
}
