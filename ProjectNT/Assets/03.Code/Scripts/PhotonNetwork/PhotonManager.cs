using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private MultiLobbyUI _multiLobbyUI;

	[SerializeField] private TextMeshProUGUI _logText;

	public override void OnConnectedToMaster()
	{
		_logText.text = "Photon 연결 성공!";
		print(_logText.text);
		PhotonNetwork.JoinLobby();
	}
	public override void OnJoinedLobby()
	{
		print("로비 입장 성공!");
		PhotonNetwork.JoinOrCreateRoom("LocalVRRoom", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
	}
	public override void OnJoinedRoom()
	{
		_logText.text = "방 참가 성공!";
		print(_logText.text);
		AssignPlayerRole();
	}

	public void AssignPlayerRole()
	{
		if (PhotonNetwork.IsMasterClient) // 가장 먼저 들어온 플레이어 = Player1
		{
			PhotonNetwork.LocalPlayer.NickName = "Player1";
		}
		else
		{
			PhotonNetwork.LocalPlayer.NickName = "Player2";
		}

		photonView.RPC("UpdateMultiLobbyUI", RpcTarget.All);
	}

	[PunRPC]
	public void UpdateMultiLobbyUI()
	{
		foreach (var player in PhotonNetwork.PlayerList)
		{
			if (player.NickName == "Player1")
			{
				_multiLobbyUI.connectImagePlayer1.color = Color.green; // 초록색으로 변경
			}
			else if (player.NickName == "Player2")
			{
				_multiLobbyUI.connectImagePlayer2.color = Color.green; // 초록색으로 변경
			}
		}
	}


	void SpawnOtherPlayer()
	{
		_logText.text = "플레이어 프리팹 생성";
		print(_logText.text);
		PhotonNetwork.Instantiate("Multi/Player", Vector3.zero, Quaternion.identity);
	}
}
