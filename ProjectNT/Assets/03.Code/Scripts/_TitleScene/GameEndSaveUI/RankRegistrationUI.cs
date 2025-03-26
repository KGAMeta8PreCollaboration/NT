using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankRegistrationUI : MonoBehaviour
{
    [Header("점수 등록 UI")]
    public TextMeshProUGUI slot_1;
    public Button slot_1UPButton;
    public Button slot_1DownButton;
    public TextMeshProUGUI slot_2;
    public Button slot_2UPButton;
    public Button slot_2DownButton;
    public TextMeshProUGUI slot_3;
    public Button slot_3UPButton;
    public Button slot_3DownButton;

    public Button registrationButton;
    public Image rankRegistrationtimer;
    public float rankRegistrationtimerTime;

    public Action registrationActuon = null;

    private string playerName = "";
    private string[] textSlot =
        { "A","B","C","D","E",
        "F","G","H","I","J",
        "K","L","M","N","O",
        "P","Q","R","S","T",
        "U","V","W","X","Y",
        "Z","_" };
    private int slot_1Number = 0;
    private int slot_2Number = 0;
    private int slot_3Number = 0;

    private Coroutine timerCorutine = null;

    private void OnEnable()
    {
        AddEventListeners();
    }

    private void OnDisable()
    {
        RemoveEventListeners();
    }

    private void AddEventListeners()//활성화시 초기화
    {
        //각슬롯 A로 초기화
        StartSlotText(slot_1, ref slot_1Number);
        StartSlotText(slot_2, ref slot_2Number);
        StartSlotText(slot_3, ref slot_3Number);
        //↑버튼에 다음 text로 넘어가는 이벤트 설정
        slot_1UPButton.onClick.AddListener(() => SlotUpButton(slot_1, ref slot_1Number));
        slot_2UPButton.onClick.AddListener(() => SlotUpButton(slot_2, ref slot_2Number));
        slot_3UPButton.onClick.AddListener(() => SlotUpButton(slot_3, ref slot_3Number));
        //↓버튼에 이전 text로 넘어가는 이벤트 설정
        slot_1DownButton.onClick.AddListener(() => SlotDownButton(slot_1, ref slot_1Number));
        slot_2DownButton.onClick.AddListener(() => SlotDownButton(slot_2, ref slot_2Number));
        slot_3DownButton.onClick.AddListener(() => SlotDownButton(slot_3, ref slot_3Number));

        registrationButton.onClick.AddListener(RegistrationButtonClick);
        StartTimer();//타이머 시작
    }

    private void RemoveEventListeners()//비활성화시 이벤트 전부 제거
    {
        //↑버튼에 이벤트 모두 삭제
        slot_1UPButton.onClick.RemoveAllListeners();
        slot_2UPButton.onClick.RemoveAllListeners();
        slot_3UPButton.onClick.RemoveAllListeners();
        //↓버튼에 이벤트 모두 삭제
        slot_1DownButton.onClick.RemoveAllListeners();
        slot_2DownButton.onClick.RemoveAllListeners();
        slot_3DownButton.onClick.RemoveAllListeners();

        registrationButton.onClick.RemoveListener(RegistrationButtonClick);
        StopTimer();//타이머 종료
        registrationActuon = null;//액션안에 있는거 제거(혹시 모를 중복 방지)
    }

    public void RegistrationButtonClick()
    {
        registrationActuon?.Invoke();
    }

    public string SetPlayerName()
    {
        playerName = slot_1.text + slot_2.text + slot_3.text;
        return playerName;
    }

    public void StartSlotText(TextMeshProUGUI slot, ref int slotNumber)
    {
        slotNumber = 0;
        slot.text = textSlot[slotNumber].ToString();
    }

    public void SlotUpButton(TextMeshProUGUI slot, ref int slotNumber)
    {
        slotNumber++;
        if (slotNumber >= textSlot.Length)//_면 A로 다시 돌아감
        {
            slotNumber = 0; 
        }
        slot.text = textSlot[slotNumber];
    }

    public void SlotDownButton(TextMeshProUGUI slot, ref int slotNumber)
    {
        slotNumber--;
        if (slotNumber < 0)//A면 _로 넘어감
        {
            slotNumber = textSlot.Length - 1;
        }
        slot.text = textSlot[slotNumber];
    }

    public void StartTimer()
    {
        if (timerCorutine == null)
        {
            timerCorutine = StartCoroutine(Timer(rankRegistrationtimerTime, 
                () => registrationActuon?.Invoke()));//타이머 끝나면 순위표 UI 오픈
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
            rankRegistrationtimer.fillAmount = 1 - (elapsedTime / timer);
            yield return null;
        }
        rankRegistrationtimer.fillAmount = 0;
        timerEndAction?.Invoke();
    }
}
