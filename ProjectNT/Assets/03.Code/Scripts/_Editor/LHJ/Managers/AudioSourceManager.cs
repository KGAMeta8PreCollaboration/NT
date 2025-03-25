using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider audioSlider;
    [SerializeField] private AudioMixer audioMixer;

    //get,set 둘다 필요 -> 노래의 시간을 조절할 수 있어야하기 때문 (0~1의 값)
    public float audioSourceValue;

    private BeatMapManager _beatMapManager;
    private CameraController _cameraController;
    private AudioSource _audioSource;
    private AudioVisualizable _audioVisualizable; //-> 변경 전 사항
    private Waveform _waveform; //-> 변경 후 사항
    private GridManager _gridManager;
    private float _audioDuration;
    public AudioSource AudioSource => _audioSource;
    public float AudioDuration => _audioDuration;

    private bool _isPlaying;
    //public Action<bool> callback;

    private void Awake()
    {
        _beatMapManager = FindObjectOfType<BeatMapManager>();
        _cameraController = FindObjectOfType<CameraController>();
        _audioVisualizable = FindObjectOfType<AudioVisualizable>();
        _waveform = FindObjectOfType<Waveform>();
        _gridManager = FindObjectOfType<GridManager>();
        //_audioSource = GetComponent<AudioSource>();
        //_audioMixer = GetComponent<AudioMixer>();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => _beatMapManager.isLoaded == true && AudioSource.clip != null);
    }

    //_audioSource 초기화 및 waveform 이미지 생성
    public void InitializeFromBeatMapManager(AudioClip audioClip)
    {
        print(2);
        if (audioClip == null)
        {
            Debug.LogWarning("노래가 없습니다.");
            return;
        }
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = audioClip;

        print(3);
        //올림
        _audioDuration = _audioSource.clip.length;
        _waveform.CreateWaveform(_audioSource);
        _gridManager.CreateNodeContainer(_audioSource);
        audioSlider.onValueChanged.AddListener(HandleAudioClip);
        //_waveform.DrawWaveform(_audioSource);
        //_audioVisualizable.InitWaveform();
        //volumeSlider.onValueChanged.AddListener(HandleVolume);
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

        audioSlider.value = _audioSource.time / _audioDuration;
    }

    private void HandlePushSpace(bool clickedSpace)
    {
        //callback?.Invoke(clickedSpace);
        if (clickedSpace == true)
            _audioSource.Pause();
        else
            _audioSource.Play();
    }

    private void HandleAudioClip(float value)
    {
        _audioSource.time = value * _audioDuration;
        audioSourceValue = value;
    }

    private void HandleVolume(float volume)
    {
        //-80f면 사실상 무음이라고 한다
        float dB = (volume > 0) ? Mathf.Log10(volume) * 20 : -80f;
        audioMixer.SetFloat("BGM", dB);
    }
}
