using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayer : MonoBehaviour
{
	[SerializeField] private PhotonView _playerView;
	[SerializeField] private ActionBasedController _leftHandCtrl;
	[SerializeField] private ActionBasedController _rightHandCtrl;

	private Camera _playerCamera;
	private AudioListener playerAudioListener;

	private void Awake()
	{
		_playerCamera = GetComponentInChildren<Camera>();
		playerAudioListener = GetComponentInChildren<AudioListener>();

		PlayerCameraAndAudioListenerActive(_playerView.IsMine);
	}

    public void PlayerCameraAndAudioListenerActive(bool on)
	{
		if (on)
		{
			_playerCamera.enabled = true;
			playerAudioListener.enabled = true;
			_leftHandCtrl.enableInputTracking = true;
			_rightHandCtrl.enableInputTracking = true;
		}
		else
		{
			_playerCamera.enabled = false;
			playerAudioListener.enabled = false;
            _leftHandCtrl.enableInputTracking = false;
            _rightHandCtrl.enableInputTracking = false;
        }
	}
}
