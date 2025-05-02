using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    public AudioSource audioSource;

    private double startDSPTime;
    private double startTime;


    void Start()
    {
        StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        audioSource.Play();
        startDSPTime = AudioSettings.dspTime;
        startTime = Time.time;
        yield return new WaitForSeconds(1f);
        while (true)
        {
            yield return new WaitForSeconds(1f);
        }
    }

}
