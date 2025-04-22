using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSound : MonoBehaviour
{
    [Header("게임 음악 샘플 소스")]
    public AudioSource gameMusicAudioSource;
    [Header("배경 음악 소스")]
    public AudioSource backgroundAudioSource;
    
    private AudioClip _gameMusicClip;

    private void Awake()
    {
        if (gameMusicAudioSource.loop == false)
        {
            gameMusicAudioSource.loop = true;
        }
        if (backgroundAudioSource.loop == false)
        {
            backgroundAudioSource.loop = true;
        }
    }

    public void SetBackgroundSound(bool active)//배경음악 켜기/끄기
    {
        if (active)
        {
            if (!backgroundAudioSource.isPlaying)
            { 
            backgroundAudioSource.Play();
            }
        }
        else
        {
            if (backgroundAudioSource.isPlaying)
            {
                backgroundAudioSource.Stop();
            }
        }
    }

    public void PlayGameSound(AudioClip clip)//게임 음악 켜기
    {
        if (gameMusicAudioSource.isPlaying)//현재 재생중이면
        {
            gameMusicAudioSource.Stop();//중지시키고
        }
        gameMusicAudioSource.clip = clip;
        gameMusicAudioSource.Play();//받은 노래 다시시작
    }
    
    public void SetMusicClip(AudioClip clip)//게임 음악 클립 설정
    {
        _gameMusicClip = clip;
    }
    
    public void PlayGameSound()//게임 음악 켜기
    {
        if (gameMusicAudioSource.isPlaying)//현재 재생중이면
        {
            gameMusicAudioSource.Stop();//중지시키고
        }
        gameMusicAudioSource.clip = _gameMusicClip;
        gameMusicAudioSource.Play();//받은 노래 다시시작
    }

    public void StopGameSound()//게임 음악 끄기
    {
        if (gameMusicAudioSource.isPlaying)
        {
            gameMusicAudioSource.Stop();
            Debug.Log("노래 꺼짐");
        }
    }

}
