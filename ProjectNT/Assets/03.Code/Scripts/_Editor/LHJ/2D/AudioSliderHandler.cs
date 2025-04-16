using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSliderHandler : MonoBehaviour
{
    [SerializeField] private Slider audioSlider;
    [SerializeField] private TextMeshProUGUI currentSongLengthText;
    [SerializeField] private TextMeshProUGUI songLengthText;

    private AudioSourceManager _audioSourceManager;
    private NCT _nct;
    private bool _isPlaying;

    private void Awake()
    {
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
        _nct = FindObjectOfType<NCT>();
        _audioSourceManager.audioCallback += HandleAudioClipLoaded;
        _isPlaying = false;
    }

    private void Update()
    {
        if (_audioSourceManager.AudioSource == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isPlaying = !_isPlaying;
            HandlePushSpace(_isPlaying);
        }

        audioSlider.value = _audioSourceManager.AudioSource.time / _audioSourceManager.AudioDuration;
        SetSongLengthText(currentSongLengthText, _audioSourceManager.AudioSource.time);
    }

    public void OnClickPauseButton(bool paused)
    {
        _isPlaying = !_isPlaying;
        HandlePushSpace(_isPlaying);
    }

    public Action<bool> onClickSpace;
    private void HandlePushSpace(bool clickedSpace)
    {
        onClickSpace?.Invoke(clickedSpace);
        if (clickedSpace == true)
        {
            _audioSourceManager.AudioSource.Pause();

            float currentTime = _audioSourceManager.AudioSource.time;
            double gridStep = _nct.cellHeight / _nct.GetComponent<SpriteRenderer>().size.y * _audioSourceManager.AudioDuration;

            int nearestGridIndex = Mathf.RoundToInt((float)(currentTime / gridStep));

            double snappedTime = nearestGridIndex * gridStep;
            snappedTime = Math.Max(0, Math.Min(snappedTime, _audioSourceManager.AudioDuration));

            _audioSourceManager.AudioSource.time = (float)snappedTime;

            audioSlider.value = _audioSourceManager.AudioSource.time / _audioSourceManager.AudioDuration;
        }
        else
            _audioSourceManager.AudioSource.Play();
    }

    private void HandleAudioClipLoaded(float time)
    {
        SetSongLengthText(songLengthText, time);
    }

    private void SetSongLengthText(TextMeshProUGUI text, float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);
        text.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }
}
