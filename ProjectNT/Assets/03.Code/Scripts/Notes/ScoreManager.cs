using System;
using System.Linq;
using UnityEngine;

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

public class ScoreManager : MonoBehaviour
{
    public int score { get; private set; } = 0;
    public int currentCombo { get; private set; } = 0;
    public int maxCombo { get; private set; } = 0;
    public int[] judgeCount { get; private set; } = new int[typeof(JudgementType).GetEnumValues().Length];

    public Action<int> OnComboChanged;
    public Action<int> OnScoreChanged;
    public Action<JudgementType> OnJudgementChanged;

    // TODO: 프로토타입 임시 할당. gameManager나 다른 객체가 해야함
    private GameSceneLightController _gameSceneLightController;

    private void Awake()
    {
        Array.Clear(judgeCount, 0, judgeCount.Length);
    }

    private void Start()
    {
        _gameSceneLightController = FindObjectOfType<GameSceneLightController>();
    }

    public void AddScore(int index)
    {
        score += index;
        OnScoreChanged?.Invoke(score);
    }

    public void AddScore(JudgementType noteType)
    {
        int index = noteType == JudgementType.PERFECT ? 100 :
            noteType == JudgementType.Good ? 50 :
            noteType == JudgementType.MISS ? 0 : 0;
        score += index;
        print($"AddScore : {noteType} : total score : {score}");
        OnScoreChanged?.Invoke(score);
    }

    public void AddJudgeCount(JudgementType noteType)
    {
        judgeCount[(int)noteType]++;
    }

    public void IncreaseCombo()
    {
        currentCombo++;
        if (currentCombo > maxCombo)
            maxCombo = currentCombo;

        if (currentCombo != 0 && currentCombo % 10 == 0)
            _gameSceneLightController?.OnLight();

        OnComboChanged?.Invoke(currentCombo);
    }

    // Bad 판정이 나올때 호출
    public void ResetCombo()
    {
        currentCombo = 0;
        OnComboChanged?.Invoke(currentCombo);
    }

    public void ShowJudgementType(JudgementType noteType)
    {
        OnJudgementChanged?.Invoke(noteType);
    }

    // 게임 끝날때 combo * 100을 score에 더해줌
    public void EndGame()
    {
        score += maxCombo * 100;
    }

    public Grade CalculateGrade()
    {
        float perfect = judgeCount[(int)JudgementType.PERFECT];
        float cool = judgeCount[(int)JudgementType.Cool];
        float good = judgeCount[(int)JudgementType.Good];
        float bad = judgeCount[(int)JudgementType.MISS];
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
}
