using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Grade
{
    SPlus,
    S,
    A,
    B,
    C,
    D,
    F
}

public class ResultPanel : MonoBehaviour
{
    private TextMeshProUGUI _totalNoteCount;
    private TextMeshProUGUI _perfectCount;
    private TextMeshProUGUI _coolCount;
    private TextMeshProUGUI _goodCount;
    private TextMeshProUGUI _badCount;
    private TextMeshProUGUI _gradeText;
    private TextMeshProUGUI _gradeSubText;
    private ScoreManager _scoreManager;

    private void Awake()
    {
        _totalNoteCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "TotalNoteCount");
        _perfectCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "PerfectCount");
        _coolCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "CoolCount");
        _goodCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "GoodCount");
        _badCount = FindDeepChildComponent<TextMeshProUGUI>(transform, "BadCount");
        _gradeText = FindDeepChildComponent<TextMeshProUGUI>(transform, "GradeText");
        _gradeSubText = FindDeepChildComponent<TextMeshProUGUI>(transform, "GradeSubText");
        _scoreManager = FindObjectOfType<ScoreManager>(true);
    }

    private void OnEnable()
    {
        DisplayPanel();
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
        Grade grade = CalculateGrade();
        if (grade == Grade.SPlus)
        {
            _gradeText.text = "S";
            _gradeSubText.text = "+";
        }
        else
            _gradeText.text = grade.ToString();
    }

    // TODO: 연산은 ScoreManager의 역할임
    private Grade CalculateGrade()
    {
        float perfect = _scoreManager.judgeCount[(int)JudgementType.Perfect];
        float cool = _scoreManager.judgeCount[(int)JudgementType.Cool];
        float good = _scoreManager.judgeCount[(int)JudgementType.Good];
        float bad = _scoreManager.judgeCount[(int)JudgementType.Bad];
        float total = perfect + cool + good + bad;
        float grade = (perfect + cool) / total * 100;
        if (grade >= 95)
            return Grade.SPlus;
        if (grade >= 90)
            return Grade.S;
        else if (grade >= 80)
            return Grade.A;
        else if (grade >= 70)
            return Grade.B;
        else if (grade >= 60)
            return Grade.C;
        else if (grade >= 50)
            return Grade.D;
        else
            return Grade.F;
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
}
