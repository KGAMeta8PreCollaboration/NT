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
        _totalNoteCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "TotalNoteCount");
        _perfectCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "PerfectCount");
        _coolCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "CoolCount");
        _goodCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "GoodCount");
        _badCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "BadCount");
        _gradeText = FindDeepChildComponent<TextMeshProUGUI>(transform, "GradeText");
        _gradeSubText = FindDeepChildComponent<TextMeshProUGUI>(transform, "GradeSubText");
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
                case JudgementType.Perfect:
                    _perfectCount.text = _scoreManager.judgeCount[i].ToString();
                    break;
                case JudgementType.Cool:
                    _coolCount.text = _scoreManager.judgeCount[i].ToString();
                    break;
                case JudgementType.Good:
                    _goodCount.text = _scoreManager.judgeCount[i].ToString();
                    break;
                case JudgementType.Bad:
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
}
