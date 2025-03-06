using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGameManager : MonoBehaviour
{
    public double musicStartTime;
    public double MusicTime
    {
        get
        {
            return AudioSettings.dspTime - musicStartTime;
        }
    }

    [SerializeField] public AudioSource bgSound;

    void Start()
    {
        bgSound.Play();

        musicStartTime = AudioSettings.dspTime;
    }
}
