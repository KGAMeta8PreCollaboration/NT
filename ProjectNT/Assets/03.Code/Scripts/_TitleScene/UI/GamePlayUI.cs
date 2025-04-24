using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIGameType
{
    Single,
    Muliti
}

[Flags]
public enum Difficulty
{
    None = 0,
    Easy = 1,
    Normal = 2,
    Hard = 4,
    SuperHard = 8,
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

    private Toggle curSelectDifficulty = null;
    private bool isSettingDifficulty = false;

    
    private TextMeshProUGUI _easyText;
    private TextMeshProUGUI _normalText;
    private TextMeshProUGUI _hardText;
    private TextMeshProUGUI _superhardText;
    
    
    private TitleSound _titleSound;

    public override void Awake()
    {
        base.Awake();
        _easyText = easy.GetComponentInChildren<TextMeshProUGUI>();
        _normalText = normal.GetComponentInChildren<TextMeshProUGUI>();
        _hardText = hard.GetComponentInChildren<TextMeshProUGUI>();
        _superhardText = superHade.GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void Start()
    {
        base.Start();
        easy.Select();
    }


    private void OnEnable()
    {
        _titleSound = FindObjectOfType<TitleSound>(true);
        if (musicChangeSelect != null)
        {
            _titleSound.PlayGameSound();
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
        base.AddEventListeners();
        SetDifficulty(easy, 1);//난이도 토글 1로 세팅

        gameStartButton.onClick.AddListener(StartGame);
        musicChangeSelect.changeRightButton.onClick.AddListener(NextMusicButton);
        musicChangeSelect.changeLeftButton.onClick.AddListener(PreviousMusicButton);

        easy.onValueChanged.AddListener((value) => OnDifficultyChanged(easy, 1));
        normal.onValueChanged.AddListener((value) => OnDifficultyChanged(normal, 2));
        hard.onValueChanged.AddListener((value) => OnDifficultyChanged(hard, 3));
        superHade.onValueChanged.AddListener((value) => OnDifficultyChanged(superHade, 4));

    }

    public override void RemoveEventListeners()//꺼질때 버튼 해제
    {
        base.RemoveEventListeners();

        gameStartButton.onClick.RemoveListener(StartGame);
        musicChangeSelect.changeRightButton.onClick.RemoveListener(NextMusicButton);
        musicChangeSelect.changeLeftButton.onClick.RemoveListener(PreviousMusicButton);

        //등록할때 람다식으로 넣어서 개별적으로 해제가 안됨, 그래서 RemoveAll로 없애기
        easy.onValueChanged.RemoveAllListeners();
        normal.onValueChanged.RemoveAllListeners();
        hard.onValueChanged.RemoveAllListeners();
        superHade.onValueChanged.RemoveAllListeners();
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
            GameManager.Instance.musicImage = musicChangeSelect.CurMusicData.musicAlbumArtSprite;
            (BeatMapData beatMapData, string projectPath, string musicName) data = GetGameStartData();
            GameManager.Instance.difficulty = GetCurrentDifficulty();
            GameManager.Instance.musicName = data.musicName;
            GameManager.Instance.SingleGameStart(data.beatMapData, data.projectPath, data.musicName);
        }
    }
    
    public (BeatMapData beatMapData, string projectPath, string musicName) GetGameStartData()
    {
        string projectName = musicChangeSelect.currentMusicNode.Value.projectName;
        string projectPath = Path.Combine(Application.persistentDataPath, "Projects", projectName);
        
        Enums.ModeDiff modeDiff = GetCurrentDifficulty() switch
        {
            Difficulty.Easy => Enums.ModeDiff.SOLO_EASY,
            Difficulty.Normal => Enums.ModeDiff.SOLO_NORMAL,
            Difficulty.Hard => Enums.ModeDiff.SOLO_HARD,
            Difficulty.SuperHard => Enums.ModeDiff.SOLO_EXTREAM,
            _ => Enums.ModeDiff.SOLO_EASY,
        };
        BeatMapData beatMapData = GetBeatMapData(projectPath, modeDiff);
        string musicName = musicChangeSelect.currentMusicNode.Value.musicName;
        return (beatMapData, projectPath, musicName);
    }
    
    public (BeatMapData beatMapData1, BeatMapData beatMapData2, string projectPath, string musicName) GetMultiGameStartData()
    {
        string projectName = musicChangeSelect.currentMusicNode.Value.projectName;
        string projectPath = Path.Combine(Application.persistentDataPath, "Projects", projectName);
        
        Enums.ModeDiff modeDiff1 = GetCurrentDifficulty() switch
        {
            Difficulty.Easy => Enums.ModeDiff.DUO1_EASY,
            Difficulty.Normal => Enums.ModeDiff.DUO1_NORMAL,
            Difficulty.Hard => Enums.ModeDiff.DUO1_HARD,
            Difficulty.SuperHard => Enums.ModeDiff.DUO1_EXTREAM,
            _ => Enums.ModeDiff.DUO1_EASY,
        };
        Enums.ModeDiff modeDiff2 = GetCurrentDifficulty() switch
        {
            Difficulty.Easy => Enums.ModeDiff.DUO2_EASY,
            Difficulty.Normal => Enums.ModeDiff.DUO2_NORMAL,
            Difficulty.Hard => Enums.ModeDiff.DUO2_HARD,
            Difficulty.SuperHard => Enums.ModeDiff.DUO2_EXTREAM,
            _ => Enums.ModeDiff.DUO2_EASY,
        };
        
        GameManager.Instance.musicImage = musicChangeSelect.CurMusicData.musicAlbumArtSprite;
        BeatMapData beatMapData1 = GetBeatMapData(projectPath, modeDiff1);
        BeatMapData beatMapData2 = GetBeatMapData(projectPath, modeDiff2);
        string musicName = musicChangeSelect.currentMusicNode.Value.musicName;
        return (beatMapData1, beatMapData2, projectPath, musicName);
    }
    
    private BeatMapData GetBeatMapData(string projectPath, Enums.ModeDiff mode)
    {
        string difficultyPath = Path.Combine(projectPath,"BeatMapData", mode.ToString());
        if (!File.Exists(difficultyPath))
        {
            Debug.LogError("BeatMapData not found at path: " + difficultyPath);
            return null;
        }
        return JsonUtility.FromJson<BeatMapData>(File.ReadAllText(difficultyPath));
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

    //다음 노래로 넘어감 (RightButton)
    public void NextMusicButton()
    {
        SetDifficulty(easy, 1);//다음 곡으로 넘어가도 이전에 선택한 난이도 유지
        musicChangeSelect.ChangeMusic("next");
        //TestStartGameData.Instance.musicName = musicChangeSelect.CurMusicData.musicName;
    }

    //이전 노래로 넘어감 (LeftButton)
    public void PreviousMusicButton()
    {
        SetDifficulty(easy, 1);//이전 곡으로 넘어가도 이전에 선택한 난이도 유지
        musicChangeSelect.ChangeMusic("previous");
        //TestStartGameData.Instance.musicName = musicChangeSelect.CurMusicData.musicName;
    }
    
    private void SetDifficulty(Toggle select, int difficulty)
    {
        //TestStartGameData.Instance.difficulty = difficulty;

        if (easy != select && easy.interactable)
        {
            easy.isOn = false;
            _easyText.color = Color.white;
        }
        
        if (normal != select && normal.interactable)
        {
            normal.isOn = false;
            _normalText.color = Color.white;
        }
        
        if (hard != select && hard.interactable)
        {
            hard.isOn = false;
            _hardText.color = Color.white;
        }
        if (superHade != select && superHade.interactable)
        {
            superHade.isOn = false;
            _superhardText.color = Color.white;
        }
        
        select.GetComponentInChildren<TextMeshProUGUI>().color = select.colors.selectedColor;
        select.isOn = true;//선택한 토글만 활성화
        if (curSelectDifficulty != null)
        {
            curSelectDifficulty.interactable = true;
        }
        curSelectDifficulty = select;
        curSelectDifficulty.Select();
    }

    public void SetToggleInteractable(Difficulty difficulty)
    {
        if (!difficulty.HasFlag(Difficulty.Easy))
        {
            easy.interactable = false;
            _easyText.color = easy.colors.disabledColor;
        }
        if (!difficulty.HasFlag(Difficulty.Normal))
        {
            normal.interactable = false;
            _normalText.color = normal.colors.disabledColor;
        }
        if (!difficulty.HasFlag(Difficulty.Hard))
        {
            hard.interactable = false;
            _hardText.color = hard.colors.disabledColor;
        }
        if (!difficulty.HasFlag(Difficulty.SuperHard))
        {
            superHade.interactable = false;
            _superhardText.color = superHade.colors.disabledColor;
        }
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
}
