using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewHighScroeUI : MonoBehaviour
{
    [Header("최고점수 갱신 UI")]
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public Button yesButton;
    public Button noButton;
    public Image newHighScroetimer;
    public float newHighScroetimerTime;

    private Coroutine timerCorutine = null;
    
    public Action noButtonAction = null;//곡 선택으로 이동
    public Action yesButtonAction = null;//등록 화면으로 이동

    private void OnEnable()
    {
        yesButton.onClick.AddListener(YesButton);
        noButton.onClick.AddListener(NoButton);
        StartTimer();//타이머 시작
    }

    private void OnDisable()
    {
        yesButton.onClick.RemoveListener(YesButton);
        noButton.onClick.RemoveListener(NoButton);
        StopTimer();//타이머 종료
        yesButtonAction = null;//액션안에 있는거 제거(혹시 모를 중복 방지)
        noButtonAction = null;//액션안에 있는거 제거(혹시 모를 중복 방지)
    }

    public void SetNewHighScroeUI(int rank, PlayerLocalSaveData newData)//새로운 데이터 표시
    {
        rankText.text = rank.ToString();
        scoreText.text = int.Parse(newData.score.ToString()).ToString("N0");
        comboText.text = int.Parse(newData.combo.ToString()).ToString("N0");
    }

    public void YesButton()
    {
        yesButtonAction?.Invoke();//등록UI 오픈
    }

    public void NoButton()
    {
        noButtonAction?.Invoke();//곡 선택창으로 이동
    }

    public void StartTimer()
    {
        if (timerCorutine == null)
        {
            timerCorutine = StartCoroutine(Timer(newHighScroetimerTime, 
                () => noButtonAction?.Invoke()));//타이머 끝나면 곡 선택창으로 이동
        }
    }

    public void StopTimer()
    {
        StopCoroutine(timerCorutine);
        timerCorutine = null;
    }

    public IEnumerator Timer(float timer, Action timerEndAction)
    {
        float elapsedTime = 0f;
        while (elapsedTime < timer)
        {
            elapsedTime += Time.deltaTime;
            newHighScroetimer.fillAmount = 1 - (elapsedTime / timer);
            yield return null;
        }
        newHighScroetimer.fillAmount = 0;
        timerEndAction?.Invoke();//타이머 끝나면 자동으로 곡 선택 화면으로 이동
    }
}
