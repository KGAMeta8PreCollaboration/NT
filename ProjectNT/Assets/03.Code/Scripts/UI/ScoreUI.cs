using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private RectTransform comboCountRect;
    [SerializeField] private TextMeshProUGUI _comboCountText;
    [SerializeField] private TextMeshProUGUI _scoreCountText;
    [SerializeField] private RectTransform judgementRect;
    [SerializeField] private TextMeshProUGUI _judgementText;
    private bool exitJudgement = false;
    public Color perfectColor;
    public Color coolColor;
    public Color goodColor;
    public Color missColor;
    public TextMeshProUGUI _timeText;
    private double _startDspTime;
    public int tempHitCount;
    public float judgementTextalpha;
    private void Start()
    {
        if (_scoreManager == null)
            _scoreManager = FindObjectOfType<ScoreManager>();
        comboCountRect.DOLocalMoveY(0.2f, 0.5f).SetEase(Ease.OutExpo);

        _startDspTime = AudioSettings.dspTime;
        _scoreManager.OnComboChanged += combo => _comboCountText.text = combo.ToString();

        _scoreManager.OnScoreChanged += score =>
        {
            _scoreCountText.text = score.ToString();
            ComboDoTween();
        };
        _scoreManager.OnJudgementChanged += judgementType =>
        {
            _judgementText.text = judgementType.ToString();
            JudgementDoToween(judgementType);
            judgementRect.DOScale(1.0f, 0.5f).SetEase(Ease.OutExpo).OnComplete(Fade());
        };

    }

    private void Update()
    {
        _timeText.text = $"TIME\n{(AudioSettings.dspTime - _startDspTime):F2}";
    }
    private void JudgementDoToween(JudgementType judgementType)
    {
        exitJudgement = false;
        switch (judgementType)
        {
            case JudgementType.PERFECT:
                _judgementText.color = perfectColor;
                break;
            case JudgementType.Cool:
                _judgementText.color = coolColor;
                break;
            case JudgementType.Good:
                _judgementText.color = goodColor;
                break;
            case JudgementType.MISS:
                _judgementText.color = missColor;
                break;
        }
        _judgementText.DOFade(judgementTextalpha, 0);
        judgementRect.DOScale(0.2f, 0);
        exitJudgement = true;
    }

    private void ComboDoTween()
    {
        comboCountRect.DOLocalMoveY(0f, 0f);
        comboCountRect.DOLocalMoveY(0.2f, 0.5f).SetEase(Ease.OutExpo);
    }

    private TweenCallback Fade()
    {
        if (exitJudgement)
            _judgementText.DOFade(0f, 0.5f);
        return null;
    }
}

