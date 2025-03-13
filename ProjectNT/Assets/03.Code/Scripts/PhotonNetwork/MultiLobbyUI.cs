using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiLobbyUI : MonoBehaviour
{
	[SerializeField] private Image _connectImagePlayer1;
	[SerializeField] private Image _connectImagePlayer2;
	[SerializeField] private Button _quitButton;
	[SerializeField] private PhotonManager _photonManager;

	private void Start()
	{
		_quitButton.onClick.AddListener(QuitButtonClick);
	}

	private void QuitButtonClick()
	{
		_photonManager.LeaveRoom();
		TitleManager.instance.CloseUI();
	}

	public void UpdateConnectImage(Player player, bool isQuit)
	{
        if (player.NickName == "Player1")
        {
            _connectImagePlayer1.color = isQuit == false ? Color.green : Color.red; // 초록색으로 변경
        }
        else if (player.NickName == "Player2")
        {
            _connectImagePlayer2.color = isQuit == false ? Color.green : Color.red; // 초록색으로 변경
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
	}
}
