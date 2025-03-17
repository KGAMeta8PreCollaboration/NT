using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.InteractiveTutorials;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Project : MonoBehaviour
{
    [SerializeField] private ProjectLoader loader;
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI projectName;
    private Sprite sprite;
    private string tempName;
    private string tempArtist;
    private string tempThumbnail;
    private byte[] tempThumbnailData;
    private string tempBgm;
    private string tempBpm;
    private string tempKeySoundPath;
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
        projectName.text = projectData.projectName;
        tempName = projectData.projectName;
        tempArtist = projectData.artistName;
        tempThumbnail = projectData.thumbnailName;
        tempThumbnailData = projectData.thumbnailData;
        tempBgm = projectData.bgmName;
        tempBpm = projectData.bpm.ToString();
        tempKeySoundPath = projectData.m_KeysoundPath;
    }

    private void ChangeFocus(bool isTrue)
    {
        if (isTrue == false)
        {
            loader.currentProject = null;
            loader.SetProjectTMP = "";
            loader.SetArtistTMP = "";
            loader.SetBpm = "";
            loader.SetBgmTMP = "";
            loader.SetThumbnailTMP = "";
            loader.SetKeySoundTMP = "";
            loader.SetThumbnail = null;
            toggle.interactable = true;
        }
        else
        {
            loader.currentProject = this;
            if (string.IsNullOrEmpty(projectData.projectName))
            {
                loader.InputFieldReset();
                projectName.text = "New Project";
                toggle.interactable = false;
                loader.EditBtn = false;
                return;
            }
            else
            {
                loader.InputFieldReset();
                projectName.text = projectData.projectName;
                loader.SetProjectTMP = projectData.projectName;
                loader.SetArtistTMP = projectData.artistName;
                loader.SetBpm = projectData.bpm.ToString();
                loader.SetBgmTMP = projectData.bgmName;
                loader.SetThumbnailTMP = projectData.thumbnailName;
                loader.SetKeySoundTMP = projectData.m_KeysoundPath;
                string path = Path.Combine(projectData.m_Path, projectData.thumbnailName);
                sprite = loader.MakeSprite(path);
                loader.SetThumbnail = sprite;
                loader.EditBtn = true;
            }
            toggle.interactable = false;
        }
    }

    public void SetName(string text)
    {
        if (this != loader.currentProject) return;
        // projectData.projectName = text;
        // loader.currentProject.projectName.text = text;
        tempName = text;
    }

    public void SetArtist(string text)
    {
        if (this != loader.currentProject) return;
        // projectData.artistName = text;
        tempArtist = text;
    }
    public void SetBPM(string text)
    {
        if (this != loader.currentProject) return;
        if (string.IsNullOrEmpty(text)) return;
        // projectData.bpm = int.Parse(text);
        tempBpm = text;
    }
    public void SetThumbnail(string text)
    {
        if (this != loader.currentProject) return;
        tempThumbnail = text;
    }
    public void SetThumbnailData(byte[] bytes)
    {
        if (this != loader.currentProject) return;
        tempThumbnailData = bytes;
    }
    public void SetBgm(string text)
    {
        if (this != loader.currentProject) return;
        tempBgm = text;
    }
    public void SetKeySoundPath(string text)
    {
        if (this != loader.currentProject) return;
        tempKeySoundPath = text;
    }
    public void SetProjectData()
    {
        projectData.projectName = tempName;
        projectData.artistName = tempArtist;
        projectData.bpm = int.Parse(tempBpm);
        projectData.thumbnailName = tempThumbnail;
        projectData.thumbnailData = tempThumbnailData;
        projectData.bgmName = tempBgm;
        projectData.m_KeysoundPath = tempKeySoundPath;
    }
}
