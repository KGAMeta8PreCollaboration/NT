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
        if (!isTrue)
        {
            loader.currentProject = null;
            loader.SetProjectName = "";
            loader.SetArtistName = "";
            loader.SetBgmName = "";
            loader.SetThumbnailName = "";
            loader.SetBpm = "";
            loader.SetThumbnail = null;
            toggle.interactable = true;
        }
        else
        {
            loader.currentProject = this;
            if (string.IsNullOrEmpty(projectData.projectName))
            {
                projectName.text = "New Project";
                toggle.interactable = false;
                loader.EditBtn = false;
                return;
            }
            else
            {
                loader.InputFieldReset();
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
                loader.EditBtn = true;
            }
            toggle.interactable = false;
        }
    }

    public void SetName(string text)
    {
        Debug.Log("RE");
        Debug.Log(this.GetInstanceID());
        Debug.Log(loader.currentProject.GetInstanceID());
        if (this != loader.currentProject) return;
        Debug.Log("SET");
        projectData.projectName = text;
        loader.currentProject.projectName.text = text;
    }

    public void SetArtist(string text)
    {
        if (this != loader.currentProject) return;
        projectData.artistName = text;
    }
    public void SetBPM(string text)
    {
        if (this != loader.currentProject) return;
        if (string.IsNullOrEmpty(text)) return;
        projectData.bpm = int.Parse(text);
    }
}
