using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
	[SerializeField] private ScoreManager _scoreManager;

	private TextMeshProUGUI _comboCountText;
	private TextMeshProUGUI _scoreCountText;

	private void Start()
	{
		_comboCountText = transform.Find("ComboCount").GetComponent<TextMeshProUGUI>();
		_scoreManager.OnComboChanged += combo => _comboCountText.text = combo.ToString();

		_scoreCountText = transform.Find("ScoreCount").GetComponent<TextMeshProUGUI>();
		_scoreManager.OnScoreChanged += score => _scoreCountText.text = score.ToString();

		
	}
	
}
