using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicChangeAndSelect : MonoBehaviour//버튼눌러서 음악넘어가는데 사용
{
    [Header("음악 미리보기 파일")]
    public GameMusicSampleData gameMusicData;//프리팹에 GameMusicSample 프리팹 참조

    public Image musicImage;
    public TextMeshProUGUI musicNameText;
    public TextMeshProUGUI musicDesc;

    public Button changeLeftButton;//이전노래
    public Button changeRightButton;//다음노래
    public Button restartButton;

    private TitleMusicData curMusicData;
    public TitleMusicData CurMusicData { get { return curMusicData; } }

    [Header("게임 음악 샘플 소스")]
    public AudioSource gameMusicAudioSource;
    [Header("배경 음악 소스")]
    public AudioSource backgroundAudioSource;
    private int musicNum = 0;

    private void Awake()
    {
        backgroundAudioSource.loop = true;
    }

    private void Update()
    {
        if (gameMusicAudioSource.isPlaying == true)
        {
            Debug.Log("노래나오는중");
        }
    }

    private void SetMusicData(TitleMusicData data)//음악 데이터들 화면에표시
    {
        Debug.Log($"{data.musicName}");
        musicImage.sprite = data.musicAlbumArtSprit;
        curMusicData = data;
        musicNameText.text = data.musicName;
        musicDesc.text = data.musicDescription;
        if (gameMusicAudioSource.loop == false)
        {
            MusicLoop(true);
        }
        PlayMusic(curMusicData.musicClip);
    }

    //음악 처음부터 다시시작
    public void RestartMusic()
    {
        PlayMusic(curMusicData.musicClip);
    }

    //가장 처음의 노래로 변경
    public void BackToFirstSongMusic(Action action = null)
    {
        musicNum = 0;
        SetMusicData(gameMusicData.titleMusicDatas[musicNum]);
        action?.Invoke();
    }

    //다음노래
    public void NextMusic(Action action = null)
    {
        if (musicNum < gameMusicData.titleMusicDatas.Count - 1)
        {
            musicNum++;
            SetMusicData(gameMusicData.titleMusicDatas[musicNum]);
        }
        else
        {
            BackToFirstSongMusic();
        }
        action?.Invoke();
    }

    //이전노래
    public void PreviousMusic(Action action = null)
    {
        if (musicNum != 0)
        {
            musicNum--;
            SetMusicData(gameMusicData.titleMusicDatas[musicNum]);
        }
        else
        {
            musicNum = gameMusicData.titleMusicDatas.Count - 1;
            SetMusicData(gameMusicData.titleMusicDatas[musicNum]);
        }
        action?.Invoke();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (gameMusicAudioSource.isPlaying)//현재 재생중이면
        {
            gameMusicAudioSource.Stop();//중지시키고
        }
        gameMusicAudioSource.clip = clip;
        gameMusicAudioSource.Play();//받은 노래 다시시작
    }

    public void StopMusic()
    {
        if (gameMusicAudioSource.isPlaying)
        {
            gameMusicAudioSource.Stop();
            Debug.Log("노래 꺼짐");
        }
    }

    public void MusicLoop(bool musicLoop)
    {
        gameMusicAudioSource.loop = musicLoop;
    }
}
