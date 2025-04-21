using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : Popup
{
    private TextMeshProUGUI _totalNoteCount;
    private TextMeshProUGUI _perfectCount;
    private TextMeshProUGUI _coolCount;
    private TextMeshProUGUI _goodCount;
    private TextMeshProUGUI _badCount;
    private TextMeshProUGUI _gradeText;
    private TextMeshProUGUI _gradeSubText;
    [SerializeField] private ScoreManager _scoreManager;

    //추가
    private TextMeshProUGUI _scoreCount;//결과창에 표시될 최종 점수 텍스트
    private TextMeshProUGUI _maxComboCount;//결과창에 표시될 최고 콤보 수 텍스트
    [SerializeField] private TextMeshProUGUI _musicNameText;//음악 이름 텍스트
    [SerializeField] private TextMeshProUGUI _musicArtistText;//음악 아티스트 이름 텍스트
    [SerializeField] private Image musicImage;//음악 이미지
    [SerializeField] private Button restartButton;//재시작 버튼
    [SerializeField] private Button musicSelectButton;//곡 선택 이동 버튼
    [SerializeField] private GameEndPanel endPanel;//최고점수 갱신시 활성화될 UI

    public void DisplayPanel()
    {
        int totalNotes = 0;
        for (int i = 0; i < _scoreManager.judgeCount.Length; i++)
        {
            totalNotes += _scoreManager.judgeCount[i];
            switch ((JudgementType)i)
            {
                case JudgementType.PERFECT:
                    _perfectCount.text = _scoreManager.judgeCount[i].ToString();
                    break;
                case JudgementType.Cool:
                    _coolCount.text = _scoreManager.judgeCount[i].ToString();
                    break;
                case JudgementType.Good:
                    _goodCount.text = _scoreManager.judgeCount[i].ToString();
                    break;
                case JudgementType.MISS:
                    _badCount.text = _scoreManager.judgeCount[i].ToString();
                    break;
            }
        }
        _totalNoteCount.text = totalNotes.ToString();
        Grade grade = _scoreManager.CalculateGrade();
        if (grade == Grade.SPlus)
        {
            _gradeText.text = "S";
            _gradeSubText.text = "+";
        }
        else
            _gradeText.text = grade.ToString();

        //추가
        _scoreCount.text = _scoreManager.score.ToString();
        _maxComboCount.text = _scoreManager.maxCombo.ToString();
        //musicImage.sprite = //음악 이미지
        //_musicNameText.text = //음악 이름 텍스트
        //_musicArtistText.text = //음악 아티스트 이름 텍스트
        
        string musicName = GameManager.Instance.musicName;
        string difficulty = GameManager.Instance.difficulty.ToString();
        
        endPanel.Open();
        endPanel.SetGameEndData(_scoreManager.score, _scoreManager.maxCombo, musicName, difficulty);//음악이름, 난이도 추가
        StartCoroutine(endPanel.NewHighScoreCheck());
    }

    public void SetResult(PlayerResultContainer result)
    {
        int totalNotes = 0;
        for (int i = 0; i < result.judgeCount.Length; i++)
        {
            totalNotes += result.judgeCount[i];
            switch ((JudgementType)i)
            {
                case JudgementType.PERFECT:
                    _perfectCount.text = result.judgeCount[i].ToString();
                    break;
                case JudgementType.Cool:
                    _coolCount.text = result.judgeCount[i].ToString();
                    break;
                case JudgementType.Good:
                    _goodCount.text = result.judgeCount[i].ToString();
                    break;
                case JudgementType.MISS:
                    _badCount.text = result.judgeCount[i].ToString();
                    break;
            }
        }
        _totalNoteCount.text = totalNotes.ToString();
        Grade grade = _scoreManager.CalculateGrade(result);
        if (grade == Grade.SPlus)
        {
            _gradeText.text = "S";
            _gradeSubText.text = "+";
        }
        else
            _gradeText.text = grade.ToString();

        //추가
        _scoreCount.text = result.score.ToString();
        _maxComboCount.text = result.maxCombo.ToString();
        //musicImage.sprite = //음악 이미지
        //_musicNameText.text = //음악 이름 텍스트
        //_musicArtistText.text = //음악 아티스트 이름 텍스트
    }

    public override void Init(PopupManager popupManager)
    {
        base.Init(popupManager);

        _totalNoteCount = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "TotalNoteCount");
        _perfectCount = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "PerfectCount");
        _coolCount = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "CoolCount");
        _goodCount = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "GoodCount");
        _badCount = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "BadCount");
        _gradeText = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "GradeText");
        _gradeSubText = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "GradeSubText");

        //추가
        _scoreCount = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "ScoreText");
        _maxComboCount = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "MaxComboText");
        restartButton.onClick.AddListener(Restart);//재시작 버튼
        musicSelectButton.onClick.AddListener(MusicSelect);//곡 선택 이동 버튼

        if (!GameManager.Instance.IsMulti)
        {
            GameManager.Instance.OnGameEnd += () =>
            {
                popupManager.OpenPopup(this);
                DisplayPanel();
            };
        }
    }

    //추가
    public void Restart()//재시작으로 이동
    {

    }

    public void MusicSelect()//곡 선택창으로 이동
    {

    }

    public override void CloseButtonClick()
    {
        base.CloseButtonClick();
        GameManager.Instance.GoToLobby();
    }
}
