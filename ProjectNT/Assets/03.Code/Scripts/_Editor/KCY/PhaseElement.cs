using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhaseElement : MonoBehaviour
{
    [SerializeField] private PhaseDriver phaseDriver;
    [SerializeField] private Button m_Load;
    [SerializeField] private Button m_Delete;
    [SerializeField] private TextMeshProUGUI m_NameTMP;
    [SerializeField] private TextMeshProUGUI m_TimeTMP;
    [SerializeField] private string m_SourceName;
    [SerializeField] private float m_Time;
    public Button m_Up;
    public Button m_Down;
    private void Awake()
    {
        phaseDriver = GetComponentInParent<PhaseDriver>();
        m_Up.onClick.AddListener(() => phaseDriver.SwapPhaseUp(this));
        m_Down.onClick.AddListener(() => phaseDriver.SwapPhaseDown(this));
    }
}
