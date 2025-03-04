using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficutyDriver : MonoBehaviour
{
    [SerializeField] private List<Toggle> toggles;
    [SerializeField] private List<PhaseDriver> phaseDrivers;

    private void Awake()
    {

        for (int i = 0; i < 4; i++)
        {
            toggles[i].onValueChanged.AddListener(phaseDrivers[i].gameObject.SetActive);
        }
    }

}
