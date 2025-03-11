using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : MonoBehaviour
{
	[SerializeField] private PhotonView _Playerview;
	private Camera _playerCamera;
	private AudioListener playerAudioListener;

	private void Awake()
	{
		_playerCamera = GetComponentInChildren<Camera>();
		playerAudioListener = GetComponentInChildren<AudioListener>();

		PlayerCameraAndAudioListenerActive(_Playerview.IsMine);
	}

	public void PlayerCameraAndAudioListenerActive(bool on)
	{
		if (on)
		{
			_playerCamera.enabled = true;
			playerAudioListener.enabled = true;
		}
		else
		{
			_playerCamera.enabled = false;
			playerAudioListener.enabled = false;
		}
	}
}
