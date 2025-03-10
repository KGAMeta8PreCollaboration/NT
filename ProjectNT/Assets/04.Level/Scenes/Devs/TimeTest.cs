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
        print($"현재 시간 : {Time.time} , 오디오 시간 : {audioSource.time}, ");
        StartCoroutine(Test());
    }

    private void PrintState()
    {
        print($"현재 시간 : {Time.time:F3} , 오디오 시간 : {audioSource.time:F3}, dspTime : {AudioSettings.dspTime:F3}, 오디오 소스 - Time.time {audioSource.time - (Time.time - startTime):F3},dsp - sdsp : {(AudioSettings.dspTime - startDSPTime):F3}, 오디오 소스 - dspTime {audioSource.time - (AudioSettings.dspTime - startDSPTime):F3}");
    }
    
    private IEnumerator Test()
    {
        audioSource.Play();
        startDSPTime = AudioSettings.dspTime;
        startTime = Time.time;
        PrintState();
        yield return new WaitForSeconds(1f);
        while (true)
        {
            PrintState();
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
