using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceManager : MonoBehaviour
{
    private BeatMapManager _beatMapManager;
    private CameraController _cameraController;
    private AudioSource _audioSource;
    private AudioVisualizable _audioVisualizable;
    private GridManager _gridManager;
    private int _audioDuration;
    public AudioSource AudioSource => _audioSource;
    public int AudioDuration => _audioDuration;

    private bool _isPlaying;
    //public Action<bool> callback;

    private void Awake()
    {
        _beatMapManager = FindObjectOfType<BeatMapManager>();
        _audioSource = GetComponent<AudioSource>();
        _cameraController = FindObjectOfType<CameraController>();
        _audioVisualizable = FindObjectOfType<AudioVisualizable>();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => _beatMapManager.isLoaded == true && AudioSource.clip != null); 
    }

    public void InitializeFromBeatMapManager(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.LogWarning("노래가 없습니다.");
            return;
        }

        _audioSource.clip = audioClip;
        //올림
        _audioDuration = Mathf.CeilToInt(_audioSource.clip.length);
        _audioVisualizable.InitWaveform();
    }

    private void Update()
    {
        if (_cameraController._isRotating == false && _beatMapManager.isLoaded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isPlaying = !_isPlaying;
                HandlePushSpace(_isPlaying);
            }
        }
    }

    private void HandlePushSpace(bool clickedSpace)
    {
        //callback?.Invoke(clickedSpace);
        if (clickedSpace == true)
            _audioSource.Pause();
        else
            _audioSource.Play();
    }
}
