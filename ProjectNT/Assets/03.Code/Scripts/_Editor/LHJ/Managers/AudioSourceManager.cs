using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider audioSlider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TextMeshProUGUI songLengthText;
    [SerializeField] private TextMeshProUGUI currentSongLengthText;
    [SerializeField] private TMP_InputField phase2Input;
    [SerializeField] private TMP_InputField phase3Input;
    [SerializeField] private Camera cam;

    //get,set 둘다 필요 -> 노래의 시간을 조절할 수 있어야하기 때문 (0~1의 값)
    public float audioSourceValue;

    private NCT _nct;
    private BeatMapManager _beatMapManager;
    private CameraController _cameraController;
    private AudioSource _audioSource;
    private AudioVisualizable _audioVisualizable; //-> 변경 전 사항
    private Waveform _waveform; //-> 변경 후 사항
    private GridManager _gridManager;
    private float _audioDuration;
    public AudioSource AudioSource => _audioSource;
    public float AudioDuration => _audioDuration;
    public int phase2;
    public int phase3;

    private bool _isPlaying;
    //public Action<bool> callback;

    private void Awake()
    {
        _nct = FindObjectOfType<NCT>();
        _beatMapManager = FindObjectOfType<BeatMapManager>();
        _cameraController = FindObjectOfType<CameraController>();
        _audioVisualizable = FindObjectOfType<AudioVisualizable>();
        _waveform = FindObjectOfType<Waveform>();
        _gridManager = FindObjectOfType<GridManager>();
        _audioSource = GetComponent<AudioSource>();

        // 볼륨 슬라이더 초기화
        if (volumeSlider != null)
        {
            volumeSlider.value = 1f;  // 초기 볼륨 100%
            HandleVolume(volumeSlider.value);
        }
        phase2Input.onEndEdit.AddListener(SavePhase2);
        phase3Input.onEndEdit.AddListener(SavePhase3);
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
        if (volumeSlider != null)
        {
            HandleVolume(volumeSlider.value);
        }

        //올림
        _audioDuration = _audioSource.clip.length;
        _waveform.CreateWaveform(_audioSource);
        //_gridManager.CreateNodeContainer(_audioSource);
        audioSlider.onValueChanged.AddListener(HandleAudioClip);
        //_waveform.DrawWaveform(_audioSource);
        //_audioVisualizable.InitWaveform();
        volumeSlider.onValueChanged.AddListener(HandleVolume);

        SetSongLengthText(songLengthText, _audioDuration);
    }

    public void InitializeFromSongData(SongData songData)
    {
        phase2Input.text = (songData?.phase2 ?? 0).ToString();
        phase3Input.text = (songData?.phase3 ?? 0).ToString();
    }

    private double gridTimeStep;
    private bool ctrlKeyDown = false;
    private void Update()
    {
        if (_beatMapManager.isLoaded == false)
        {
            Debug.Log("return");
            return;
        }

        SetSongLengthText(currentSongLengthText, _audioSource.time);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print($"스페이스바 들어옴");
            _isPlaying = !_isPlaying;
            HandlePushSpace(_isPlaying);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.001f)
            {
                cam.transform.position += Vector3.forward * (scroll * 2.0f); // 이동 속도 조절 가능
            }
        }
        else
        {
            //-0.1 ~ 0.1사이값이 나옴
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.001f)
            {
                double currentTime = _audioSource.time;
                //float newValue = Mathf.Clamp(scroll, 0f, 1f);
                gridTimeStep = (_nct.cellHeight / _nct.GetComponent<SpriteRenderer>().size.y) * _audioDuration;

                if (scroll > 0)
                {
                    currentTime += gridTimeStep;
                }
                else
                {
                    currentTime -= gridTimeStep;
                }
                currentTime = Mathf.Clamp((float)currentTime, 0, _audioDuration);
                _audioSource.time = (float)currentTime;
            }
        }
        audioSlider.value = _audioSource.time / _audioDuration;
    }

    private void HandlePushSpace(bool clickedSpace)
    {
        //callback?.Invoke(clickedSpace);
        if (clickedSpace == true)
        {
            _audioSource.Pause();

            float currentTime = _audioSource.time;
            double gridStep = _nct.cellHeight / _nct.GetComponent<SpriteRenderer>().size.y * _audioDuration;

            int nearestGridIndex = Mathf.RoundToInt((float)(currentTime / gridStep));

            double snappedTime = nearestGridIndex * gridStep;
            snappedTime = Math.Max(0, Math.Min(snappedTime, _audioDuration));

            _audioSource.time = (float)snappedTime;

            audioSlider.value = _audioSource.time / _audioDuration;
        }
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
        if (audioMixer == null) return;

        try
        {
            // volume 값은 0~1 사이의 값
            float dB = volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20f;
            audioMixer.SetFloat("BGM", dB);

            // AudioSource의 볼륨도 함께 조절(이게 없으면 안됨)
            if (_audioSource != null)
            {
                _audioSource.volume = volume;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"볼륨 조절 중 오류 발생: {e.Message}");
        }
    }

    private void SavePhase2(string value)
    {
        if (int.TryParse(value, out int parsedValue))
        {
            phase2 = parsedValue;
        }
    }
    private void SavePhase3(string value)
    {
        if (int.TryParse(value, out int parsedValue))
        {
            phase3 = parsedValue;
        }
    }

    private void SetSongLengthText(TextMeshProUGUI text, float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);
        text.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }
}
