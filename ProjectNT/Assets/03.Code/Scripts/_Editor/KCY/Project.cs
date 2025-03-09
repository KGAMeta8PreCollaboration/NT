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
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI projectName;
    private Sprite sprite;
    public Toggle Toggle
    {
        get { return toggle; }
        set { Toggle = value; }
    }
    public TextMeshProUGUI ProjectName
    {
        get { return projectName; }
        set { projectName = value; }
    }
    public ProjectData projectData = new ProjectData();
    private void Awake()
    {
        loader = GetComponentInParent<ProjectLoader>();
        toggle.group = loader.projects_Group;

        toggle.onValueChanged.AddListener(ChangeFocus);

    }

    private void Start()
    {
        LoadData();
        toggle.isOn = true;
    }

    private void LoadData()
    {
        if (string.IsNullOrEmpty(projectData.projectName)) return;
        Debug.Log("프로젝트 이름 있음");
        projectName.text = projectData.projectName;
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
                projectName.text = "New Project";
            else
            {
                projectName.text = projectData.projectName;
                loader.SetProjectName = projectData.projectName;
                loader.SetArtistName = projectData.artistName;
                loader.SetBgmName = projectData.bgmName;
                loader.SetThumbnailName = projectData.thumbnailName;
                loader.SetBpm = projectData.bpm.ToString();
                if (sprite == null)
                {
                    string path = Path.Combine(projectData.m_Path, projectData.thumbnailName);
                    sprite = loader.MakeSprite(path, Vector2.zero);
                }
                loader.SetThumbnail = sprite;
            }
            toggle.interactable = false;
            loader.currentProject = this;
        }
        else
        {
            loader.SetProjectName = "";
            loader.SetArtistName = "";
            loader.SetBgmName = "";
            loader.SetThumbnailName = "";
            loader.SetBpm = "";
            loader.SetThumbnail = null;
            toggle.interactable = true;
            loader.currentProject = null;
        }
    }

    public void SetName(string text)
    {
        projectName.text = text;
        projectData.projectName = text;
    }

    internal void SetArtist(string text)
    {
        projectData.artistName = text;
    }
}
