using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
[Serializable]
public struct ProjectData
{
    public string projectName;
    public string artistName;
    public string thumbnailName;
    public string highlightPath;
    public string bgmPath;
    public int bpm;
    public int beatNum;
    public string m_Path;
    public string phase1KeysoundPath;
    public string phase2KeysoundPath;
    public string phase3KeysoundPath;
    public byte[] thumbnailData;
    public Enums.ModeDiff modeDiff;

}

public class EditorDataManager : Singleton<EditorDataManager>
{
    private ProjectData currentProjectData = new ProjectData();
    private BeatMapManager beatMapManager;
    private string savefolderName = "BeatMapData";
    private string curKeySoundName;
    private bool isSaved;

    public BeatMapData beatMap = new BeatMapData();
    public Sprite thumbnail_sprite;
    public AudioClip bgmClip;

    public Action<BeatMapData> beatMapLoadAction;
    public Action saveTrackingAction;
    public Action phaseDataAction;

    public bool isLoadCompelete = false;

    //TODO 복사할 때를 위한 캐싱...? 아직 확정아님

    public ProjectData ProjectData
    { get { return currentProjectData; } set { currentProjectData = value; } }

    public string CurKeySoundName
    { get { return curKeySoundName; } set { curKeySoundName = value; } }

    private void Start()
    {
        SceneManager.sceneLoaded += (x, y) =>
        {
            if (SceneManager.GetActiveScene().name == "SongEditorScene")
            {
                print("SceneManager 로드");
                LoadBeatMapData();
                beatMapManager = FindObjectOfType<BeatMapManager>();
                beatMapLoadAction += beatMapManager.LoadBeatMapData;
                beatMapLoadAction?.Invoke(beatMap);
            }
        };
    }

    public void LoadBeatMapData()
    {
        string filePath;
        print(ProjectData.m_Path);
        print(savefolderName);
        print(ProjectData.modeDiff.ToString());

        filePath = Path.Combine(ProjectData.m_Path, savefolderName, ProjectData.modeDiff.ToString());

        if (true == File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            beatMap = JsonUtility.FromJson<BeatMapData>(jsonFile);
        }
        else
        {
            beatMap = new BeatMapData();
        }
    }

    private void SaveDataLocal()
    {
        string folderPath;
        string jsonData;
        string filePath;
        if (string.IsNullOrEmpty(currentProjectData.m_Path))
        {
            Debug.LogError("현재 프로젝트의 설정된 경로가 없습니다.");
            return;
        }
        folderPath = Path.Combine(currentProjectData.m_Path, savefolderName);
        filePath = Path.Combine(folderPath, ProjectData.modeDiff.ToString());
        if (false == Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        jsonData = JsonUtility.ToJson(beatMap);
        File.WriteAllText(filePath, jsonData);
    }

    public void SaveBeatMap()
    {
        Debug.Log("세이브 진입");
        beatMap = beatMapManager.SaveBeatMapData();
        SaveDataLocal();
    }

    public void ProjectInfoSave(ProjectData saveData)
    {
        string combinePath;
        combinePath = Path.Combine(saveData.m_Path, "ProjectInfos");
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(combinePath, json);
    }
}
