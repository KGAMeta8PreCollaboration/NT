using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class EditorDataManager : Singleton<EditorDataManager>
{

    public ProjectData currentProjectData;

    protected override void Awake()
    {
        base.Awake();
    }
}
