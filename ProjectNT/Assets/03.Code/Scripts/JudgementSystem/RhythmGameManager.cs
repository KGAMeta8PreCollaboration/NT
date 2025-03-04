using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGameManager : MonoBehaviour
{
    public double musicStartTime;
    public float musicStartDelay;
    [SerializeField] public AudioSource bgSound;

    void Start()
    {
        bgSound.Play();

        musicStartTime = AudioSettings.dspTime;
        //StartCoroutine(PlayBgSound());
    }

    public IEnumerator PlayBgSound()
    {
        yield return new WaitForSeconds(musicStartDelay);
        bgSound.Play();

        musicStartTime = AudioSettings.dspTime;
    }
}
