using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phase : MonoBehaviour
{
    // [SerializeField] private Button m_Load;
    // [SerializeField] private Button m_Delete;
    [SerializeField] private TextMeshProUGUI m_NameTMP;
    [SerializeField] private TextMeshProUGUI m_TimeTMP;
    [SerializeField] private string m_SourceName;
    [SerializeField] private float m_Time;
    // [SerializeField] private Button m_Up;
    // [SerializeField] private Button m_Down;
    private Enums.ModeDiff m_ModeDiff;

    private int m_PhaseNum;

    public PhaseDriver phaseDriver;
    public Enums.ModeDiff modeDiff { get; set; }
    public int phaseNum { get; set; }

    private void Awake()
    {
        // Initialize();
    }

    private void Initialize()
    {
        // m_Up.onClick.AddListener(() => phaseDriver.SwapPhaseUp(this));
        // m_Down.onClick.AddListener(() => phaseDriver.SwapPhaseDown(this));
        // m_Delete.onClick.AddListener(Delete_BTN);
    }

    private void Delete_BTN()
    {
        // phaseDriver.linkedPhase.Remove(this);
        // Destroy(gameObject);
    }
}
