using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceManager : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _audioDuration;
    public AudioSource AudioSource => _audioSource;
    public float AudioDuration => _audioDuration; 

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioDuration = _audioSource.clip.length;
    }
}
