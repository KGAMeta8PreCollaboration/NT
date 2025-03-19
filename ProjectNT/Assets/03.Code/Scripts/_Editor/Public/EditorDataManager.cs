using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.Networking;
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

    private Dictionary<Enums.ModeDiff, BeatMapData> beatMapDic =
    new Dictionary<Enums.ModeDiff, BeatMapData>();
    private BeatMapManager beatMapManager;
    private TestLoad testLoad;
    private string bgmDestPath;
    private string curKeySoundName;
    private bool isSaved = true;

    public Action<BeatMapData> beatMapLoadAction;
    public Action phaseDataAction;
    public Sprite thumbnail_sprite;
    public AudioClip bgmClip;
    public ProjectData ProjectData { get { return currentProjectData; } set { currentProjectData = value; } }

    public Enums.ModeDiff CurModeDiff
    {
        get { return currentModeDiff; }
        set
        {
            currentModeDiff = value;
            beatMapLoadAction?.Invoke(CurBeatMap);
            phaseDataAction?.Invoke();
        }
    }
    public BeatMapData CurBeatMap
    {
        get { return beatMapDic[CurModeDiff]; }
        set { beatMapDic[CurModeDiff] = value; }
    }

    public string CurKeySoundName
    { get { return curKeySoundName; } set { curKeySoundName = value; } }

    public bool IsSaved
    { get { return isSaved; } set { isSaved = value; } }

    protected override void Awake()
    {
        base.Awake();

        //TODO 병합 후 주석해제예정
        SceneManager.sceneLoaded += (x, y) =>
        {
            if (SceneManager.GetActiveScene().name == "SongEditorScene")
            {
                testLoad = FindObjectOfType<TestLoad>();
                testLoad.songName = ProjectData.bgmName;
                LoadBeatMapData();
                beatMapManager = FindObjectOfType<BeatMapManager>();
                beatMapLoadAction += beatMapManager.LoadBeatMapData;
                SaveDataLocal();
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
        string path;
        try
        {
            path = Path.Combine(currentProjectData.m_Path, savefileName);
        }
        catch
        {
            return;
        }
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

    public void SaveDataLocal()
    {
        string path;
        try
        {
            path = Path.Combine(currentProjectData.m_Path, savefileName);
        }
        catch
        {
            return;
        }
        string jsonData;
        jsonData = DictionaryJsonUtility.ToJson(beatMapDic, true);
        File.WriteAllText(path, jsonData);
    }
    public void SetBgm()
    {
        string bgmSavePath = Path.Combine(ProjectData.m_Path, "bgmSaveFile");
        string bgmPath = Path.Combine(ProjectData.m_Path, ProjectData.bgmName);

        bgmDestPath = Path.Combine(bgmSavePath, ProjectData.bgmName);
        if (Directory.Exists(bgmSavePath))
        {
            Directory.Delete(bgmSavePath, true);
        }
        Directory.CreateDirectory(bgmSavePath);
        File.Copy(bgmPath, bgmDestPath);
        StartCoroutine(InstantiateBGM());
    }

    private IEnumerator InstantiateBGM()
    {
        AudioClip clip;

        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(bgmDestPath, AudioType.WAV);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error loading audio clip : {request.error}");
        }
        clip = DownloadHandlerAudioClip.GetContent(request);
        clip.name = ProjectData.bgmName;
        bgmClip = clip;
        yield return null;
    }

    public void SaveBeatMap()
    {
        CurBeatMap = beatMapManager.SaveBeatMapData();
        SaveDataLocal();
    }
}
