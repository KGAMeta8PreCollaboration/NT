using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBar : MonoBehaviour
{
    //private AudioSourceManager _audioSourceManager = null;
    //private Waveform _waveform;

    //private SpriteRenderer _spriteRenderer;

    //private float yPos;
    //private AudioSource _audioSource;

    //private void Awake()
    //{
    //    _audioSourceManager = FindObjectOfType<AudioSourceManager>();
    //    _waveform = FindObjectOfType<Waveform>();
    //    yPos = transform.position.y;
    //    _spriteRenderer = _waveform._spriteRenderer;
    //    _audioSource = _audioSourceManager.AudioSource;
    //}

    //private void Update()
    //{
    //    //SetPlayBar();
    //}

    //private void SetPlayBar()
    //{
    //    //progress = 현재 노래 시간 / 전체 노래 시간
    //    float progress = _audioSource.time / _audioSource.clip.length;
    //    //print($"_audioSource.time : {_audioSource.time}, _audioSource.clip.length : {_audioSource.clip.length}");
    //    float yOffset = progress * _spriteRenderer.size.y * 100;
    //    transform.position = new Vector3(0, yPos + yOffset);
    //}
}
