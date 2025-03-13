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

    private Coroutine _startGameCoroutine;

    private void Start()
    {
        _quitButton.onClick.AddListener(QuitButtonClick);
        _startButton.onClick.AddListener(StartButtonClick);

        CountStartGameActive(false);
    }

    private void QuitButtonClick()
    {
        _photonManager.LeaveRoom();
        TitleManager.instance.CloseUI();
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
            PopupManager.Instance.OpenPopup<AlarmPopup>().SetPopup("플레이어 수가 부족합니다.", "확인");
        }
    }

    public void GameStart()
    {
        CountStartGameActive(true);
        _startGameCoroutine = StartCoroutine(StartGameCoroutine());
        PopupManager.Instance.OpenPopup<AlarmPopup>().SetPopup("곧 합주가 시작됩니다.", "취소", () => _photonManager.photonView.RPC("CancelStartGame", RpcTarget.All));
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return StartCoroutine(CountStartGameCoroutine());
        PhotonNetwork.LoadLevel("LSH_MultiGame");
    }

    private IEnumerator CountStartGameCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
        _countStartGame.text = "5";
        yield return waitForSeconds;
        _countStartGame.text = "4";
        yield return waitForSeconds;
        _countStartGame.text = "3";
        yield return waitForSeconds;
        _countStartGame.text = "2";
        yield return waitForSeconds;
        _countStartGame.text = "1";
        yield return waitForSeconds;
    }

    public void CountStartGameActive(bool isActive)
    {
        _countStartGame.gameObject.SetActive(isActive);
    }


    public void CancelStartGame()
    {
        if (_startGameCoroutine != null)
        {
            StopCoroutine(_startGameCoroutine);
            _startGameCoroutine = null;
            Debug.Log("게임 시작이 취소되었습니다!");
        }

        CountStartGameActive(false);
        PopupManager.Instance.ClosePopup<AlarmPopup>();
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
