using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioTailController : MonoBehaviour
{
    public AudioMixer audioMixer;

    private void Awake()
    {
        // SetReverbTail(1f);
    }

    public void SetReverbTail(float value)
    {
        // Reverb 테일 지속 시간 조정
        audioMixer.SetFloat("ReverbDecayTime", Mathf.Lerp(0.1f, 5f, value)); 
    }
}
