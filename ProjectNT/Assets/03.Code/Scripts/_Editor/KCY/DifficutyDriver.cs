using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficutyDriver : MonoBehaviour
{
    [SerializeField] private List<Toggle> diff_Toggles;
    [SerializeField] private List<PhaseDriver> phaseDrivers;

    private void Awake()
    {
        Initialize();
    }
    private void Start()
    {

    }

    private void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            diff_Toggles[i].onValueChanged.AddListener(phaseDrivers[i].gameObject.SetActive);
        }
        diff_Toggles[0].isOn = true;
        diff_Toggles[0].interactable = false;
        phaseDrivers[0].gameObject.SetActive(true);
    }
}
