using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMapPlane : MonoBehaviour
{
    private AudioSourceManager _audioSourceManager;

    private void Start()
    {
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();    
    }

    public void HandleBeatMapPosZ(float sliderValue)
    {
        transform.position = Vector3.back * (_audioSourceManager.AudioSource.clip.length * sliderValue);
        print($"슬라이더 값 : {sliderValue}");
        print($"포지션 값 : {transform.position.z}");
        print($"오디오 길이 값: {_audioSourceManager.AudioSource.clip.length}");
    }
}
