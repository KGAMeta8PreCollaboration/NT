using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeDriver : MonoBehaviour
{
    [SerializeField] private List<Toggle> mode_Toggles;

    private void Awake()
    {
        foreach (Toggle t in mode_Toggles)
        {

        }
        mode_Toggles[0].isOn = true;
        mode_Toggles[0].interactable = false;

    }

    private void Start()
    {

    }
}
