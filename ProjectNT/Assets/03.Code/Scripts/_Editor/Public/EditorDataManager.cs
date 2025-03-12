using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
[Serializable]
public struct ProjectData
{
    public string projectName;
    public string artistName;
    public string thumbnailName;
    public string bgmName;
    public int bpm;
    public string m_Path;
    public string m_KeysoundPath;
}

public class EditorDataManager : Singleton<EditorDataManager>
{

    private ProjectData currentProjectData;
    private string savefileName = "BeatMapData";
    public ProjectData ProjectData
    { get { return currentProjectData; } set { currentProjectData = value; } }

    public Dictionary<Enums.ModeDiff, BeatMapData> beatMapDic =
    new Dictionary<Enums.ModeDiff, BeatMapData>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadBeatMapData()
    {

        string path = Path.Combine(currentProjectData.m_Path, savefileName);
        string jsonData;
        if (!File.Exists(path))
        {
            jsonData = DictionaryJsonUtility.ToJson(beatMapDic, true);
            File.WriteAllText(path, jsonData);
            return;
        }
        jsonData = File.ReadAllText(path);
        beatMapDic = DictionaryJsonUtility.FromJson<Enums.ModeDiff, BeatMapData>(jsonData);
    }

    public void SaveBeatMapData()
    {
        string path = Path.Combine(currentProjectData.m_Path, savefileName);

    }
}
