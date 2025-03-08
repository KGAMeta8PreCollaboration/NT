using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called before the first frame update
    
    
    void Start()
    {
        print($"현재 시간 : {Time.time} , 오디오 시간 : {audioSource.time}, ");
        StartCoroutine(Test());
    }
    
    private IEnumerator Test()
    {
        yield return new WaitForSeconds(5f);
        print($"현재 시간 : {Time.time} , 오디오 시간 : {audioSource.time}");
        yield return new WaitForSeconds(5f);
        print($"현재 시간 : {Time.time} , 오디오 시간 : {audioSource.time}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
