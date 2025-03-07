using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public int score { get; private set; } = 0;
	public int currentCombo { get; private set; } = 0;
	public int maxCombo { get; private set; } = 0;
	
	public Action<int> OnComboChanged;
	public Action<int> OnScoreChanged;
	

	public void AddScore(int index)
	{
		score += index;
		OnScoreChanged?.Invoke(score);
	}
	
	public void AddScore(NoteType noteType)
	{
		int index = noteType == NoteType.Perfect ? 100 : 
			noteType == NoteType.Good ? 50 : 
			noteType == NoteType.Bad ? 0 : 0;
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

	// 게임 끝날때 combo * 100을 score에 더해줌
	public void EndGame()
	{
		score += maxCombo * 100;
	}
}
