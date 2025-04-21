using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.InteractiveTutorials;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.UI;

public class Project : MonoBehaviour
{
    [SerializeField] private ProjectLoader loader;
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI projectName;
    [SerializeField] private Button delete_btn;

    private Sprite sprite;
    private string tempName;
    private string tempArtist;
    private string tempThumbnail;
    private byte[] tempThumbnailData;
    private string tempBgm;
    private string tempBpm;
    private string tempBeatNum;
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
    public Sprite Sprite => sprite;
    public ProjectData projectData = new ProjectData();

    public string TempName
    {
        get { return tempName; }
    }

    private void Awake()
    {
        loader = GetComponentInParent<ProjectLoader>();
        delete_btn.onClick.AddListener(loader.DeleteUIOpen);
        toggle.group = loader.projects_Group;
        toggle.onValueChanged.AddListener(ChangeFocus());
    }

    private UnityAction<bool> ChangeFocus()
    {
        UnityAction<bool> action = isOn =>
        {
            if (false == isOn)
            {
                loader.currProject = null;
                loader.SetProjectTMP = "";
                loader.SetArtistTMP = "";
                loader.SetBpm = "";
                loader.SetBeatNum = "";
                loader.SetThumbnailTMP = "";
                loader.SetThumbnail = null;
                toggle.interactable = true;
            }
            if (true == isOn)
            {
                loader.currProject = this;
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
                    loader.SetBeatNum = projectData.beatNum.ToString();
                    loader.SetThumbnailTMP = projectData.thumbnailName;
                    sprite = loader.ByteToSprite(projectData.thumbnailData);
                    loader.SetThumbnail = sprite;
                    loader.EditBtn = true;
                }
                toggle.interactable = false;
            }
        };
        return action;
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
        tempBgm = projectData.bgmPath;
        tempBpm = projectData.bpm.ToString();
        tempBeatNum = projectData.beatNum.ToString();
        tempKeySoundPath = projectData.m_KeysoundPath;
    }


    public void SetName(string text)
    {
        if (this != loader.currProject) return;
        tempName = text;
    }

    public void SetArtist(string text)
    {
        if (this != loader.currProject) return;
        tempArtist = text;
    }
    public void SetBPM(string text)
    {
        if (this != loader.currProject) return;
        if (string.IsNullOrEmpty(text)) return;
        tempBpm = text;
    }
    public void SetBeatNum(string text)
    {
        if (this != loader.currProject) return;
        if (string.IsNullOrEmpty(text)) return;
        tempBeatNum = text;
    }
    public void SetThumbnail(string text)
    {
        if (this != loader.currProject) return;
        tempThumbnail = text;
    }
    public void SetThumbnailData(byte[] bytes)
    {
        if (this != loader.currProject) return;
        tempThumbnailData = bytes;
    }

    public void SetProjectData()
    {
        projectData.projectName = tempName;
        projectData.artistName = tempArtist;
        projectData.bpm = int.Parse(tempBpm);
        projectData.beatNum = int.Parse(tempBeatNum);
        projectData.thumbnailName = tempThumbnail;
        projectData.thumbnailData = tempThumbnailData;
        sprite = loader.ByteToSprite(projectData.thumbnailData);
        projectData.m_KeysoundPath = tempKeySoundPath;
    }
}
