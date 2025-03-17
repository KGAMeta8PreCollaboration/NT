using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public byte[] thumbnailData;
}

public class EditorDataManager : Singleton<EditorDataManager>
{

    private ProjectData currentProjectData;
    private string savefileName = "BeatMapData";
    private Enums.ModeDiff currentModeDiff;
    private NodeContainer nodeContainer;

    private Dictionary<Enums.ModeDiff, BeatMapData> beatMapDic =
    new Dictionary<Enums.ModeDiff, BeatMapData>();

    private TestLoad testLoad;

    public ProjectData ProjectData { get { return currentProjectData; } set { currentProjectData = value; } }

    public Enums.ModeDiff CurModeDiff { get { return currentModeDiff; } set { currentModeDiff = value; } }

    public Sprite thumbnail_sprite;
    public AudioClip bgmClip;

    public BeatMapData CurBeatMap
    {
        get { return beatMapDic[CurModeDiff]; }
        set { beatMapDic[CurModeDiff] = value; }
    }

    protected override void Awake()
    {
        base.Awake();

        //TODO 병합 후 주석해제예정
        SceneManager.sceneLoaded += (x, y) =>
        {
            if (SceneManager.GetActiveScene().name == "SongEditorScene")
            {
                nodeContainer = FindObjectOfType<NodeContainer>();
                testLoad = FindObjectOfType<TestLoad>();
                testLoad.songName = ProjectData.bgmName;
                LoadBeatMapData();
            }
        };

        for (int i = 0; i < Enums.MODEDIFF_COUNT; i++)
        {
            BeatMapData beatMapData = new BeatMapData();
            beatMapDic.Add(Enums.ModeDiff.SOLO_EASY + i, beatMapData);
        }
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
        //TODO 병합 후 주석해제 예정
        string jsonData;
        jsonData = DictionaryJsonUtility.ToJson(beatMapDic, true);
        File.WriteAllText(path, jsonData);
    }
}
