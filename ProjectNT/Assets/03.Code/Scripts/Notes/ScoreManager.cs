using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score { get; private set; } = 0;
    public int currentCombo { get; private set; } = 0;
    public int maxCombo { get; private set; } = 0;

    public Action<int> OnComboChanged;
    public Action<int> OnScoreChanged;
    public Action<JudgementType> OnJudgementChanged;


    public void AddScore(int index)
    {
        score += index;
        OnScoreChanged?.Invoke(score);
    }

    public void AddScore(JudgementType noteType)
    {
        int index = noteType == JudgementType.Perfect ? 100 :
            noteType == JudgementType.Good ? 50 :
            noteType == JudgementType.Bad ? 0 : 0;
        score += index;
        OnScoreChanged?.Invoke(score);
    }

    public void IncreaseCombo()
    {
        currentCombo++;
        if (currentCombo > maxCombo)
            maxCombo = currentCombo;
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
}
