using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceManager : MonoBehaviour
{
    private CameraController _cameraController;
    private AudioSource _audioSource;  
    private int _audioDuration;
    public AudioSource AudioSource => _audioSource;
    public int AudioDuration => _audioDuration;

    private bool _isPlaying;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _cameraController = FindObjectOfType<CameraController>();

        //반올림
        _audioDuration = Mathf.CeilToInt(_audioSource.clip.length);
        print($"노래 길이 {_audioDuration}");
    }

    private void Update()
    {
        if (_cameraController._isRotating == false)
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
        if (clickedSpace == true)
            _audioSource.Pause();
        else
            _audioSource.Play();
    }
}
