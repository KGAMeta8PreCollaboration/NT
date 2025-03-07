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
    }
}
