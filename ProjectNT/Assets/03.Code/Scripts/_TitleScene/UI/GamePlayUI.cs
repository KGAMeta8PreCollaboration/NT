using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UIGameType
{
    Single,
    Muliti
}

public enum Difficulty
{
    Easy,
    Normal,
    Hard,
    SuperHard
}

public class GamePlayUI : BaseTitleUI
{
    public UIGameType gameType;

    [Header("음악 미리보기 파일")]
    [SerializeField]
    public MusicChangeAndSelect musicChangeSelect;

    public Button gameStartButton;
    [Header("난이도 선택")]
    public Toggle easy;
    public Toggle normal;
    public Toggle hard;
    public Toggle superHade;
    //public Button randomDifficulty;

    private Toggle curSelectDifficulty = null;
    private bool isSettingDifficulty = false;

    public override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        if (musicChangeSelect != null)
        { 
            // musicChangeAndSelect의 gameMusicData를 만들어서줘야할거같은데..
            // TmpCheckDirectory.Instance.musicChangeAndSelect = musicChangeSelect;
            AddEventListeners();
        }
        else
        {
            Debug.LogError("musicChangeSelect is not assigned.");
        }
    }

    private void OnDisable()
    {
        if (musicChangeSelect != null)
        {
            RemoveEventListeners();
        }
    }

    public override void AddEventListeners()//켜질때 버튼 등록
    {
        print("AddEventListeners 1");
        base.AddEventListeners();
        ResetMusicSet();//0번 음악 세팅, 동시에 게임 미리듣기 음악 재생
        SetDifficulty(easy, 1);//난이도 토글 1로 세팅
        print("AddEventListeners 2");

        gameStartButton.onClick.AddListener(StartGame);
        musicChangeSelect.changeRightButton.onClick.AddListener(NextMusicButton);
        musicChangeSelect.changeLeftButton.onClick.AddListener(PreviousMusicButton);
        //musicChangeSelect.musicReplayButton.onClick.AddListener(MusicSoundReplay);
        print("AddEventListeners 3");

        easy.onValueChanged.AddListener((value) => OnDifficultyChanged(easy, 1));
        normal.onValueChanged.AddListener((value) => OnDifficultyChanged(normal, 2));
        hard.onValueChanged.AddListener((value) => OnDifficultyChanged(hard, 3));
        superHade.onValueChanged.AddListener((value) => OnDifficultyChanged(superHade, 4));
        //randomDifficulty.onClick.AddListener(SelectRandomDifficulty);
        print("AddEventListeners 4");
    }

    public override void RemoveEventListeners()//꺼질때 버튼 해제
    {
        base.RemoveEventListeners();

        gameStartButton.onClick.RemoveListener(StartGame);
        musicChangeSelect.changeRightButton.onClick.RemoveListener(NextMusicButton);
        musicChangeSelect.changeLeftButton.onClick.RemoveListener(PreviousMusicButton);
        //musicChangeSelect.musicReplayButton.onClick.RemoveListener(MusicSoundReplay);

        //등록할때 람다식으로 넣어서 개별적으로 해제가 안됨, 그래서 RemoveAll로 없애기
        easy.onValueChanged.RemoveAllListeners();
        normal.onValueChanged.RemoveAllListeners();
        hard.onValueChanged.RemoveAllListeners();
        superHade.onValueChanged.RemoveAllListeners();
        //randomDifficulty.onClick.RemoveListener(SelectRandomDifficulty);
    }

    public override void CloseUIButtonClick()
    {
        base.CloseUIButtonClick();
    }

    public void StartGame()
    {
        //curMusicData 로 노래가지고 게임시작 로직
        if (gameType == UIGameType.Muliti)
        {
            //멀티플레이시 노래시작
        }
        else
        {
            Debug.Log($"{TestStartGameData.Instance.musicName}");
            Debug.Log($"{TestStartGameData.Instance.difficulty}");
            Difficulty difficulty = GetCurrentDifficulty();
            BeatMapData beatMapData = new BeatMapData();

            
            string projectName = musicChangeSelect.currentMusicNode.Value.projectName;
            string projectPath = Path.Combine(Application.persistentDataPath, "Projects", projectName);
            TmpCheckDirectory tmpCheckDirectory = FindObjectOfType<TmpCheckDirectory>();
            Dictionary<Enums.ModeDiff, BeatMapData> beatMapDataDictionary 
                = tmpCheckDirectory.beatMapDic[projectName];

            beatMapData = difficulty switch
            {
                Difficulty.Easy => beatMapDataDictionary[Enums.ModeDiff.SOLO_EASY],
                Difficulty.Normal => beatMapDataDictionary[Enums.ModeDiff.SOLO_NORMAL],
                Difficulty.Hard => beatMapDataDictionary[Enums.ModeDiff.SOLO_HARD],
                Difficulty.SuperHard => beatMapDataDictionary[Enums.ModeDiff.SOLO_EXTREAM],
                _ => beatMapData
            };
            // 프로젝트 이름 어디서 받아올까..? 
            GameManager.Instance.SingleGameStart(difficulty, beatMapData, projectPath);
        }
    }
    
    private Difficulty GetCurrentDifficulty()
    {
        if (easy.isOn)
            return Difficulty.Easy;
        if (normal.isOn)
            return Difficulty.Normal;
        if (hard.isOn)
            return Difficulty.Hard;
        if (superHade.isOn)
            return Difficulty.SuperHard;
        return Difficulty.Easy;
        
    }

    //음악 재시작
    public void MusicSoundReplay()
    {
        Debug.Log($"노래 Restart : {musicChangeSelect.CurMusicData.musicName}");
        musicChangeSelect.ReplayMusic();
        if (gameType == UIGameType.Muliti)
        {
            //멀티플레이어시 음악 재시작 동기화
        }
    }

    //인덱스 0번음악으로 변경(시작)
    public void ResetMusicSet()
    {
        // TODO: 임의로 하는게 아니라 알아서 셋팅된 MusicChangeAndSelect가 알아서 호출해야함
        // musicChangeSelect.ChangeMusic("first");
        //TestStartGameData.Instance.musicName = musicChangeSelect.CurMusicData.musicName;
        //TestStartGameData.Instance.difficulty = 1;
        if (gameType == UIGameType.Muliti)
        {
            //멀티플레이어시 노래 넘어가는거 동기화
        }
    }

    //다음 노래로 넘어감 (RightButton)
    public void NextMusicButton()
    {
        SetDifficulty(easy, 1);
        musicChangeSelect.ChangeMusic("next");
        //TestStartGameData.Instance.musicName = musicChangeSelect.CurMusicData.musicName;
    }

    //이전 노래로 넘어감 (LeftButton)
    public void PreviousMusicButton()
    {
        SetDifficulty(easy, 1);
        musicChangeSelect.ChangeMusic("previous");
        //TestStartGameData.Instance.musicName = musicChangeSelect.CurMusicData.musicName;
    }

    private void SetDifficulty(Toggle select, int difficulty)
    {
        //TestStartGameData.Instance.difficulty = difficulty;

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
    }

    private void OnDifficultyChanged(Toggle select, int difficulty)
    {
        if (isSettingDifficulty) return;
        isSettingDifficulty = true;

        //현재 토글이 선택되지 않은 경우에만
        if (select.isOn && curSelectDifficulty != select)
        {
            SetDifficulty(select, difficulty);
        }
        isSettingDifficulty = false;
    }

    //private void SelectRandomDifficulty()
    //{
    //    if (isSettingDifficulty) return;
    //    isSettingDifficulty = true;
    //    //랜덤으로 난이도 설정
    //    Toggle[] difficulties = new Toggle[] { easy, normal, hard, superHade };
    //    int randomIndex = UnityEngine.Random.Range(0, difficulties.Length);
    //    SetDifficulty(difficulties[randomIndex], randomIndex + 1);
    //    isSettingDifficulty = false;
    //}
}
