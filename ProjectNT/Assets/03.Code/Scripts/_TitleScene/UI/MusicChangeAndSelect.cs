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
    public Button musicReplayButton;

    private TitleMusicData curMusicData;
    public TitleMusicData CurMusicData { get { return curMusicData; } }

    private int musicNum = 0;

    private void OnEnable()
    {
        TitleManager.instance.BackgroundMusicPlay(false);
    }

    private void OnDisable()
    {
        TitleManager.instance.BackgroundMusicPlay(true);
    }

    private void SetMusicData(TitleMusicData data)//음악 데이터들 화면에표시
    {
        Debug.Log($"{data.musicName}");
        musicImage.sprite = data.musicAlbumArtSprit;
        curMusicData = data;
        musicNameText.text = data.musicName;
        musicDesc.text = data.musicDescription;
        if (TitleManager.instance.gameMusicAudioSource.loop == false)
        {
            TitleManager.instance.MusicLoop(true);//루프가 안켜져있으면
        }
        TitleManager.instance.PlayMusic(curMusicData.musicClip);
    }

    //음악 처음부터 다시시작
    public void ReplayMusic()
    {
        Debug.Log("Music Replay 버튼 클릭");
        TitleManager.instance.PlayMusic(curMusicData.musicClip);
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
}
