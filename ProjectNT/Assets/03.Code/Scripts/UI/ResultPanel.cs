using Game;
using System;
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
    [SerializeField] private TextMeshProUGUI _musicNameText;//음악 이름 텍스트
    [SerializeField] private TextMeshProUGUI _musicArtistText;//음악 아티스트 이름 텍스트
    [SerializeField] private Image musicImage;//음악 이미지
    [SerializeField] private Button restartButton;//재시작 버튼
    [SerializeField] private Button musicSelectButton;//곡 선택 이동 버튼

    private void Awake()
    {
        _totalNoteCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "TotalNoteCount");
        _perfectCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "PerfectCount");
        _coolCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "CoolCount");
        _goodCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "GoodCount");
        _badCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "BadCount");
        _gradeText = FindDeepChildComponent<TextMeshProUGUI>(transform, "GradeText");
        _gradeSubText = FindDeepChildComponent<TextMeshProUGUI>(transform, "GradeSubText");

        //추가
        _scoreCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "ScoreText");
        restartButton.onClick.AddListener(Restart);//재시작 버튼
        musicSelectButton.onClick.AddListener(MusicSelect);//곡 선택 이동 버튼
    }

    private void OnEnable()
    {
        //DisplayPanel();
    }


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
        //musicImage.sprite = //음악 이미지
        //_musicNameText.text = //음악 이름 텍스트
        //_musicArtistText.text = //음악 아티스트 이름 텍스트
    }

    public T FindDeepChildComponent<T>(Transform parent, string name) where T : Component
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child.GetComponent<T>();
            }
            T result = FindDeepChildComponent<T>(child, name);
            if (result != null)
                return result;
        }
        return null;
    }

    public override void Init(PopupManager popupManager)
    {
        base.Init(popupManager);

        GameManager.Instance.OnGameEnd += () =>
        {
            popupManager.OpenPopup(this);
            DisplayPanel();
        };
    }

    //추가
    public void Restart()//재시작으로 이동
    {

    }

    public void MusicSelect()//곡 선택창으로 이동
    {

    }
}
