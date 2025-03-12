using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIGameType
{
    Single,
    Muliti
}

public class GamePlayUI : BaseTitleUI
{
    public UIGameType gameType;

    [Header("음악 미리보기 파일")]
    [SerializeField]
    private GameMusicSampleData gameMusicData;//프리팹에 GameMusicSample 프리팹 참조

    public Image musicImage;
    public TextMeshProUGUI musicNameText;
    public TextMeshProUGUI musicDescriptionText;

    public Button gameStartButton;
    public Button songChangeLeftButton;
    public Button songChangeRightButton;
    public Button songResetButton;

    [SerializeField, Header("게임 음악 샘플 소스")]
    private AudioSource gameMusicAudioSource;
    [SerializeField, Header("배경 음악 소스")]
    private AudioSource backgroundAudioSource;

    private int musicNum;
    private TitleMusicData curMusicData;

    public override void Awake()
    {
        songChangeRightButton.onClick.AddListener(NextMusicButton);
        songChangeLeftButton.onClick.AddListener(PreviousMusicButton);
        gameStartButton.onClick.AddListener(StartGame);
        songResetButton.onClick.AddListener(MusicSoundReset);
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
        base.Awake();
    }

    private void OnEnable()
    {
        ResetMusicSet();
    }

    private void OnDisable()
    {
        StopMusic();
    }

    private void SetMusicData(TitleMusicData data)
    {
        //musicImage.sprite = data.musicAlbumArtImage.sprite;
        musicNameText.text = data.musicName;
        musicDescriptionText.text = data.musicDescription;
        curMusicData = data;
        Debug.Log($"{data.musicName}");
        MusicLoop(true);
        //TitleManager.instance.PlayMusic(curMusicData.musicClip);
    }

    public void StartGame()
    {
        //curMusicData 로 노래가지고 게임시작 로직
        MusicLoop(false);
        if (gameType == UIGameType.Muliti)
        {
            //멀티플레이어시 노래시작
        }
        else
        {
            //싱글플레이시 노래시작
        }
    }

    //음악 재시작
    public void MusicSoundReset()
    {
        PlayMusic(curMusicData.musicClip);
        if (gameType == UIGameType.Muliti)
        {
            //멀티플레이어시 음악 재시작 동기화
        }
    }

    //인덱스 0번음악으로 변경(시작)
    public void ResetMusicSet()
    {
        musicNum = 0;
        SetMusicData(gameMusicData.titleMusicDatas[musicNum]);
        if (gameType == UIGameType.Muliti)
        {
            //멀티플레이어시 노래 넘어가는거 동기화
        }
    }

    //다음 노래로 넘어감 (RightButton)
    public void NextMusicButton()
    {
        if (musicNum < gameMusicData.titleMusicDatas.Count - 1)
        {
            musicNum++;
            SetMusicData(gameMusicData.titleMusicDatas[musicNum]);
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
            SetMusicData(gameMusicData.titleMusicDatas[musicNum]);
        }
        else
        {
            musicNum = gameMusicData.titleMusicDatas.Count - 1;
            SetMusicData(gameMusicData.titleMusicDatas[musicNum]);
        }
    }

    public override void CloseUIButtonClick()
    {
        if (gameType == UIGameType.Single)
        {
            base.CloseUIButtonClick();
        }
        else
        {
            //멀티플레이시 닫기 버튼을 누르면 원래 위치로 다시 이동
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (gameMusicAudioSource.isPlaying)
        {
            gameMusicAudioSource.Stop();
        }
        gameMusicAudioSource.clip = clip;
        gameMusicAudioSource.Play();
    }

    public void StopMusic()
    {
        if (gameMusicAudioSource.isPlaying)
        {
            gameMusicAudioSource.Stop();
        }
    }

    public void MusicLoop(bool musicLoop)
    {
        gameMusicAudioSource.loop = musicLoop;
    }
}
