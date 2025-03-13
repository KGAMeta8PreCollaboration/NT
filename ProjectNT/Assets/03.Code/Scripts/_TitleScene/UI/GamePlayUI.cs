using System;
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
    public MusicChangeAndSelect musicChangeSelect;

    public Button gameStartButton;
    [Header("난이도 선택")]
    public Toggle easy;
    public Toggle normal;
    public Toggle hard;
    public Toggle superHade;
    public Button randomDifficulty;

    private Toggle curSelectDifficulty = null;

    public override void Awake()
    {
        Initialize();
        base.Awake();
    }

    private void OnEnable()
    {
        ResetMusicSet();
    }

    private void OnDisable()
    {
        musicChangeSelect.StopMusic();
    }

    private void Initialize()
    {
        gameStartButton.onClick.AddListener(StartGame);
        musicChangeSelect.changeRightButton.onClick.AddListener(NextMusicButton);
        musicChangeSelect.changeLeftButton.onClick.AddListener(PreviousMusicButton);
        musicChangeSelect.restartButton.onClick.AddListener(MusicSoundReset);
        musicChangeSelect.backgroundAudioSource.loop = true;
        musicChangeSelect.backgroundAudioSource.Play();

        SetDifficulty(easy);
        easy.onValueChanged.AddListener((value) => OnToggleChanged(easy));
        normal.onValueChanged.AddListener((value) => OnToggleChanged(normal));
        hard.onValueChanged.AddListener((value) => OnToggleChanged(hard));
        superHade.onValueChanged.AddListener((value) => OnToggleChanged(superHade));
        randomDifficulty.onClick.AddListener(SelectRandomDifficulty);
    }

    public void StartGame()
    {
        //curMusicData 로 노래가지고 게임시작 로직
        musicChangeSelect.MusicLoop(false);
        if (gameType == UIGameType.Muliti)
        {
            //멀티플레이시 노래시작
        }
        else
        {
            //싱글플레이시 노래시작
        }
    }

    //음악 재시작
    public void MusicSoundReset()
    {
        musicChangeSelect.PlayMusic(musicChangeSelect.CurMusicData.musicClip);
        if (gameType == UIGameType.Muliti)
        {
            //멀티플레이어시 음악 재시작 동기화
        }
    }

    //인덱스 0번음악으로 변경(시작)
    public void ResetMusicSet()
    {
        musicChangeSelect.BackToFirstSongMusic();
        if (gameType == UIGameType.Muliti)
        {
            //멀티플레이어시 노래 넘어가는거 동기화
        }
    }

    //다음 노래로 넘어감 (RightButton)
    public void NextMusicButton()
    {
        musicChangeSelect.NextMusic();
    }

    //이전 노래로 넘어감 (LeftButton)
    public void PreviousMusicButton()
    {
        musicChangeSelect.PreviousMusic();
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

    private void SetDifficulty(Toggle select)
    {
        //이벤트 리스너 비활성화
        easy.onValueChanged.RemoveAllListeners();
        normal.onValueChanged.RemoveAllListeners();
        hard.onValueChanged.RemoveAllListeners();
        superHade.onValueChanged.RemoveAllListeners();

        easy.isOn = false;
        normal.isOn = false;
        hard.isOn = false;
        superHade.isOn = false;

        select.isOn = true;//선택한 토글만 활성화
        if (curSelectDifficulty != null)
        {
            curSelectDifficulty.interactable = true;
        }
        curSelectDifficulty = select;
        curSelectDifficulty.interactable = false;

        //이벤트 트리거 다시 등록
        easy.onValueChanged.AddListener((value) => OnToggleChanged(easy));
        normal.onValueChanged.AddListener((value) => OnToggleChanged(normal));
        hard.onValueChanged.AddListener((value) => OnToggleChanged(hard));
        superHade.onValueChanged.AddListener((value) => OnToggleChanged(superHade));
    }

    private void OnToggleChanged(Toggle select)
    {
        //현재 토글이 선택되지 않은 경우에만
        if (select.isOn && curSelectDifficulty != select)
        {
            SetDifficulty(select);
        }
    }

    private void SelectRandomDifficulty()
    {
        //랜덤으로 난이도 설정
        Toggle[] difficulties = new Toggle[] { easy, normal, hard, superHade };
        int randomIndex = UnityEngine.Random.Range(0, difficulties.Length);
        SetDifficulty(difficulties[randomIndex]);
    }
}
