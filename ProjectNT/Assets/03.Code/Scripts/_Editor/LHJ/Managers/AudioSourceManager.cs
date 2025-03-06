using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController; // -> 이부분 마음에 안든다. 나중에 camera이동중일때를 넘겨줄 방법을 찾아보자

    private AudioSource _audioSource;
    private float _audioDuration;
    public AudioSource AudioSource => _audioSource;
    public float AudioDuration => _audioDuration;

    private bool _isPlaying;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioDuration = _audioSource.clip.length;
    }

    private void Update()
    {
        if (cameraController._isRotating == false)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                print($"스페이스바 눌림"); 
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
