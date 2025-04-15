using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBarHandler : MonoBehaviour
{
    [SerializeField] private GameObject playBarPrefab;

    private AudioSourceManager _audioSourceManager;
    private Waveform _waveform;
    private NCT _nct;

    private void Awake()
    {
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
        _waveform = FindObjectOfType<Waveform>();
        _nct = FindObjectOfType<NCT>();
        _nct.loadComplete += Init;
    }

    private void Update()
    {
        SetPlayBarPos();
    }

    private void Init()
    {
        float progress = _audioSourceManager.AudioSource.time / _audioSourceManager.AudioDuration;
        float yOffset = progress * _waveform._spriteRenderer.size.x;
        //playBarPrefab.transform.localScale = new Vector3(_nct.bpmLineLength, playBarPrefab.transform.localScale.y);
    }

    //private float _progress;
    //private float _yOffset;
    public Action loaded;
    private void SetPlayBarPos()
    {
        if (_audioSourceManager.AudioSource == null && _waveform.isLoaded == false && _nct.isLoaded == false)
        {
            return;
        }

        //progress = 현재 노래 시간 / 전체 노래 시간
        float _progress = _audioSourceManager.AudioSource.time / _audioSourceManager.AudioDuration;
        //print($"_audioSource.time : {_audioSource.time}, _audioSource.clip.length : {_audioSource.clip.length}");
        float _yOffset = _progress * _waveform._spriteRenderer.size.x;
        float xPos = _nct.transform.position.x + _nct.xOffset;
        playBarPrefab.transform.position = new Vector3(xPos, _yOffset);

        loaded?.Invoke();
    }
}
