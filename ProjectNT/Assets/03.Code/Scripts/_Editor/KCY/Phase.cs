using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phase : MonoBehaviour
{
    [SerializeField] private PhaseDriver phaseDriver;
    [SerializeField] private Button m_Load;
    [SerializeField] private Button m_Delete;
    [SerializeField] private TextMeshProUGUI m_NameTMP;
    [SerializeField] private TextMeshProUGUI m_TimeTMP;
    [SerializeField] private string m_SourceName;
    [SerializeField] private float m_Time;
    [SerializeField] private Button m_Up;
    [SerializeField] private Button m_Down;
    private Enums.ModeDiff m_ModeDiff;
    private SongData songData = new SongData();

    private int m_PhaseNum;

    public SongData m_SongData { get; set; }
    public Enums.ModeDiff modeDiff { get; set; }
    public int phaseNum { get; set; }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        phaseDriver = GetComponentInParent<PhaseDriver>();
        m_Up.onClick.AddListener(() => phaseDriver.SwapPhaseUp(this));
        m_Down.onClick.AddListener(() => phaseDriver.SwapPhaseDown(this));

    }
}
