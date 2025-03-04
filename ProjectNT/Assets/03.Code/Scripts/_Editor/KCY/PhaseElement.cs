using System;
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
    [SerializeField] private Button m_Up;
    [SerializeField] private Button m_Down;
    private Enums.ModeDiff m_ModeDiff;
    private int m_PhaseNum;
    public SongData m_SongData = new SongData();
    public Enums.ModeDiff modeDiff
    {
        get { return m_ModeDiff; }
        set { m_ModeDiff = value; }
    }
    public int phaseNum
    {
        get { return m_PhaseNum; }
        set { m_PhaseNum = value; }
    }

    private void Awake()
    {
        Initialize();
    }

    public void SaveAction()
    {
        MusicDriver.Instance.Save(m_SongData, m_ModeDiff, m_PhaseNum.ToString());
    }

    private void Initialize()
    {
        phaseDriver = GetComponentInParent<PhaseDriver>();
        m_Up.onClick.AddListener(() => phaseDriver.SwapPhaseUp(this));
        m_Down.onClick.AddListener(() => phaseDriver.SwapPhaseDown(this));
    }
}
