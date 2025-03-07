using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongTimelineController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI timeText;

    private AudioSourceManager _audioSourceManager;
    private BeatMapPlane _beatMapPlane;
    private float _songLength;
    private float _lastSongTime;

    private void Awake()
    {
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
        _beatMapPlane = FindObjectOfType<BeatMapPlane>();
        if (_audioSourceManager == null)
        {
            Debug.LogError("AudioSourceManager가 씬에 존재하지 않습니다.");
        }
    }

    private IEnumerator Start()                                                                                                     
    {
        yield return new WaitUntil(() => _audioSourceManager.AudioSource.clip != null);
        _songLength = _audioSourceManager.AudioSource.clip.length;
        slider.value = 0;
        slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    private void Update()
    {
        float currentTime = _audioSourceManager.AudioSource.time;

        if (currentTime != _lastSongTime)
        {
            slider.value = currentTime / _songLength;
            UpdateTimeText(currentTime);
            _lastSongTime = currentTime;
        }
    }

    private void UpdateTimeText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);
        timeText.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    //슬라이더 값 변화할때 마다 불리는 이벤트 함수
    public void OnSliderValueChanged()
    {
        float newTime = slider.value * _songLength;
        _audioSourceManager.AudioSource.time = newTime;
        UpdateTimeText(newTime);

        _beatMapPlane.HandleBeatMapPosZ(slider.value);
    }
}
