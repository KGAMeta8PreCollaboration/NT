using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DifficutyCtrl : MonoBehaviour
{
    [SerializeField] private List<Toggle> diff_Toggles;
    [SerializeField] private List<PhaseDriver> phaseDrivers = new List<PhaseDriver>();
    private Enums.ModeDiff currentModeDiff;
    public Enums.ModeDiff modeDiff { get; set; }
    private void Awake()
    {
        Initialize();
    }
    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            //세이브로드 이벤트 구독
            ResourceIO.Instance.saveDelegate += phaseDrivers[i].AddDataList;
            ResourceIO.Instance.loadDelegate += phaseDrivers[i].LoadData;
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < 4; i++)
        {

            //세이브로드 이벤트 구독 해제
            ResourceIO.Instance.saveDelegate -= phaseDrivers[i].AddDataList;
            ResourceIO.Instance.loadDelegate -= phaseDrivers[i].LoadData;
        }
    }
    private void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            diff_Toggles[i].onValueChanged.AddListener(phaseDrivers[i].gameObject.SetActive);
            phaseDrivers[i].modeDiff = currentModeDiff + i;

            phaseDrivers[i].Initialize();

        }
        diff_Toggles[0].isOn = true;
        diff_Toggles[0].interactable = false;
        phaseDrivers[0].gameObject.SetActive(true);
    }
}
