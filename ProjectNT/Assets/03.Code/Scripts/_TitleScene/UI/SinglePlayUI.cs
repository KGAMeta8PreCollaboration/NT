using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TitleMusicData
{
    [Header("음악 이름")]
    public string musicName;
    [Header("음악 앨범아트 이미지")]
    public Image musicAlbumArtImage;
    [Header("음악 파일")]
    public AudioClip musicClip;
}

public class SinglePlayUI : BaseTitleUI
{
    [Header("음악 미리보기 파일")]
    public TitleMusicData[] titleMusicDatas;

    public Image musicImage;
    public TextMeshProUGUI musicNameText;

    public Button gameStartButton;
    public Button songChangeLeftButton;
    public Button songChangeRightButton;
    public Button songResetButton;

    private int musicNum;
    private TitleMusicData curMusicData;

    public override void Awake()
    {
        songChangeRightButton.onClick.AddListener(NextMusicButton);
        songChangeLeftButton.onClick.AddListener(PreviousMusicButton);
        gameStartButton.onClick.AddListener(StartGame);
        songResetButton.onClick.AddListener(MusicSoundReset);
        base.Awake();
    }

    private void SetMusicData(TitleMusicData data)
    {
        musicImage.sprite = data.musicAlbumArtImage.sprite;
        musicNameText.text = data.musicName;
        curMusicData = data;
        TitleManager.instance.PlayMusic(curMusicData.musicClip);
    }

    public void StartGame()
    {
        //curMusicData 로 노래가지고 게임시작 로직
    }

    //음악 재시작
    public void MusicSoundReset()
    {
        TitleManager.instance.PlayMusic(curMusicData.musicClip);
    }

    //인덱스 0번음악으로 변경(시작)
    public void ResetMusicSet()
    {
        musicNum = 0;
        SetMusicData(titleMusicDatas[musicNum]);
    }

    //다음 노래로 넘어감 (RightButton)
    public void NextMusicButton()
    {
        if (musicNum < titleMusicDatas.Length)
        {
            musicNum++;
            SetMusicData(titleMusicDatas[musicNum]);
        }
        else
        {
            ResetMusicSet();
        }
    }

    //이전 노래로 넘어감 (LeftButton)
    public void PreviousMusicButton()
    {
        if (musicNum != 0)
        {
            musicNum--;
            SetMusicData(titleMusicDatas[musicNum]);
        }
        else
        {
            musicNum = titleMusicDatas.Length - 1;
            SetMusicData(titleMusicDatas[musicNum]);
        }
    }
}
