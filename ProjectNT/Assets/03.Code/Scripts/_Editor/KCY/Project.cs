using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.InteractiveTutorials;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[Serializable]
public struct ProjectData
{
    public string projectName;
    public string artistName;
    public string thumbnailName;
    public string bgmName;
    public int bpm;
    public string m_Path;
}
public class Project : MonoBehaviour
{
    [SerializeField] private ProjectLoader loader;
    [SerializeField] private Toggle m_Toggle;
    [SerializeField] private TextMeshProUGUI m_ProjectName;
    private Sprite m_Sprite;
    public Toggle Toggle
    {
        get { return m_Toggle; }
        set { Toggle = value; }
    }
    public TextMeshProUGUI ProjectName
    {
        get { return m_ProjectName; }
        set { m_ProjectName = value; }
    }
    public ProjectData projectData = new ProjectData();
    private void Awake()
    {
        loader = GetComponentInParent<ProjectLoader>();
        m_Toggle.group = loader.projects_Group;

        m_Toggle.onValueChanged.AddListener(ChangeFocus);

    }

    private void Start()
    {
        LoadData();
        m_Toggle.isOn = true;
    }

    private void LoadData()
    {
        if (string.IsNullOrEmpty(projectData.projectName)) return;
        Debug.Log("프로젝트 이름 있음");
        m_ProjectName.text = projectData.projectName;
        if (string.IsNullOrEmpty(projectData.artistName)) return;
        Debug.Log("아티스트 이름 있음");
        if (string.IsNullOrEmpty(projectData.bgmName)) return;
        Debug.Log("bgm 이름 있음");
        if (string.IsNullOrEmpty(projectData.thumbnailName)) return;
        Debug.Log("썸네일 이름 있음");

    }

    private void ChangeFocus(bool isTrue)
    {
        if (isTrue)
        {
            if (string.IsNullOrEmpty(projectData.projectName))
                m_ProjectName.text = "New Project";
            else
            {
                m_ProjectName.text = projectData.projectName;
                loader.SetProjectName = projectData.projectName;
                loader.SetArtistName = projectData.artistName;
                loader.SetBgmName = projectData.bgmName;
                loader.SetThumbnailName = projectData.thumbnailName;
                if (m_Sprite == null)
                {
                    string path = Path.Combine(projectData.m_Path, projectData.thumbnailName);
                    m_Sprite = loader.MakeSprite(path, Vector2.zero);
                }
                loader.SetThumbnail = m_Sprite;
            }
            m_Toggle.interactable = false;
            loader.currentProject = this;
        }
        else
        {
            loader.SetProjectName = "";
            loader.SetArtistName = "";
            loader.SetBgmName = "";
            loader.SetThumbnailName = "";
            loader.SetThumbnail = null;
            m_Toggle.interactable = true;
            loader.currentProject = null;
        }
    }

    public void SetName(string text)
    {
        m_ProjectName.text = text;
        projectData.projectName = text;
    }

    internal void SetArtist(string text)
    {
        projectData.artistName = text;
    }
}
