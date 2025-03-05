using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public struct ProjectData
{
    public string songName;
    public string artistName;
    public string thumbnailName;
    public string bgmName;
}
public class Project : MonoBehaviour
{
    [SerializeField] private ProjectLoader loader;
    [SerializeField] private Toggle m_Toggle;
    [SerializeField] private TextMeshProUGUI m_ProjectName;
    public ProjectData projectData = new ProjectData();
    private void Awake()
    {

        loader = GetComponentInParent<ProjectLoader>();
        m_Toggle.group = loader.projects_Group;
        m_ProjectName.text = "New Project";

    }
}
