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

    private void Awake()
    {
        _totalNoteCount = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "TotalNoteCount");
        _perfectCount = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "PerfectCount");
        _coolCount = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "CoolCount");
        _goodCount = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "GoodCount");
        _badCount = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "BadCount");
        _gradeText = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "GradeText");
        _gradeSubText = TransformUtil.FindDeepChildComponent<TextMeshProUGUI>(transform, "GradeSubText");
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
}
