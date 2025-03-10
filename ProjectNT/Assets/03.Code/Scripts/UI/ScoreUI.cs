using System;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
	[SerializeField] private ScoreManager _scoreManager;

	private TextMeshProUGUI _comboCountText;
	private TextMeshProUGUI _scoreCountText;

	public TextMeshProUGUI _timeText;
	private double _time;

	private void Start()
	{
		_comboCountText = transform.Find("ComboCount").GetComponent<TextMeshProUGUI>();
		_scoreManager.OnComboChanged += combo => _comboCountText.text = combo.ToString();

		_scoreCountText = transform.Find("ScoreCount").GetComponent<TextMeshProUGUI>();
		_scoreManager.OnScoreChanged += score => _scoreCountText.text = score.ToString();
		
		_timeText = transform.Find("TimeText").GetComponent<TextMeshProUGUI>();
		_time = AudioSettings.dspTime;
	}

	private void Update()
	{
		_timeText.text = $"TIME\n{(AudioSettings.dspTime - _time):F2}";
	}

}
