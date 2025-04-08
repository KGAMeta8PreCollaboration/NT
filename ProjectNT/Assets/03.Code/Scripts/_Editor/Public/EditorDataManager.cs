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
    public string bgmPath;
    public int bpm;
    public int beatNum;
    public string m_Path;
    public string m_KeysoundPath;
    public byte[] thumbnailData;

    public void Print()
    {
        Debug.Log("========ProjectData========");
        Debug.Log($"ProjectName : {projectName}");
        Debug.Log($"ArtistName : {artistName}");
        Debug.Log($"ThumbnailName : {thumbnailName}");
        Debug.Log($"BGMPath : {bgmPath}");
        Debug.Log($"BPM : {bpm}");
        Debug.Log($"BeatNum : {beatNum}");
        Debug.Log($"ProjectPath : {m_Path}");
        Debug.Log($"KeySoundPath : {m_KeysoundPath}");
        Debug.Log("==========================");
    }

}

public class EditorDataManager : Singleton<EditorDataManager>
{
    private ProjectData currentProjectData;
    private Enums.ModeDiff currentModeDiff;
    private Dictionary<Enums.ModeDiff, BeatMapData> beatMapDic =
    new Dictionary<Enums.ModeDiff, BeatMapData>();
    private BeatMapManager beatMapManager;
    private string savefolderName = "BeatMapData";
    private string curKeySoundName;
    private string bgmDestPath;
    private bool isSaved;

    public BeatMapData beatMapCache = new BeatMapData();
    public Sprite thumbnail_sprite;
    public AudioClip bgmClip;

    public Action<BeatMapData> beatMapLoadAction;
    public Action saveTrackingAction;
    public Action phaseDataAction;

    public bool isLoadCompelete = false;

    //TODO 복사할 때를 위한 캐싱...? 아직 확정아님

    public ProjectData ProjectData
    { get { return currentProjectData; } set { currentProjectData = value; } }

    public Enums.ModeDiff CurModeDiff
    {
        get { return currentModeDiff; }
        set
        {
            currentModeDiff = value;
            // TODO 저장관련 메서드 새로 전달받아야함.
            if (beatMapManager != null)
            {
                CurBeatMap = beatMapDic[CurModeDiff];
                beatMapLoadAction?.Invoke(CurBeatMap);
                phaseDataAction?.Invoke();
            }
        }
    }
    public BeatMapData CurBeatMap
    {
        get { return beatMapDic[CurModeDiff]; }
        set
        {
            beatMapDic[CurModeDiff] = value;
        }
    }

    public string CurKeySoundName
    { get { return curKeySoundName; } set { curKeySoundName = value; } }
    public bool IsSaved
    {
        get { return isSaved; }
        set
        {
            isSaved = value;
            if (isSaved == false)
            {
                saveTrackingAction?.Invoke();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += (x, y) =>
        {
            if (SceneManager.GetActiveScene().name == "SongEditorScene")
            {
                print("SceneManager 로드");
                LoadBeatMapData();
                beatMapManager = FindObjectOfType<BeatMapManager>();
                beatMapLoadAction += beatMapManager.LoadBeatMapData;
            }
        };

    }
    public void LoadBeatMapData()
    {
        if (string.IsNullOrEmpty(currentProjectData.m_Path))
        {
            Debug.LogError("현재 프로젝트의 설정된 경로가 없습니다.");
            return;
        }
        for (int i = 0; i < Enums.MODEDIFF_COUNT; i++)
        {
            if (false == beatMapDic.ContainsKey(Enums.ModeDiff.SOLO_EASY + i))
            {
                BeatMapData beatMapData = new BeatMapData();
                beatMapDic.Add(Enums.ModeDiff.SOLO_EASY + i, beatMapData);
            }
        }

        string folderPath;
        folderPath = Path.Combine(currentProjectData.m_Path, savefolderName);
        if (Directory.Exists(folderPath))
        {

            string[] filesPath = Directory.GetFiles(folderPath);
            string fileName;
            string jsonData;

            foreach (string filePath in filesPath)
            {
                fileName = Path.GetFileName(filePath);
                jsonData = File.ReadAllText(filePath);

                Debug.Log($"filename {fileName}");
                Debug.Log($"FilePath {filePath}");

                beatMapDic[(Enums.ModeDiff)Enum.Parse(typeof(Enums.ModeDiff), fileName)] =
                JsonUtility.FromJson<BeatMapData>(jsonData);
            }
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
        if (false == Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        Debug.LogError($"{currentModeDiff}난이도 세이브중");
        jsonData = JsonUtility.ToJson(CurBeatMap, true);
        filePath = Path.Combine(folderPath, currentModeDiff.ToString());
        File.WriteAllText(filePath, jsonData);

    }
    public void SetBgm()
    {
        string bgmSavePath = Path.Combine(ProjectData.m_Path, "bgmSaveFile");
        string fileName = Path.GetFileName(ProjectData.bgmPath);
        string[] extension = fileName.Split('.');
        bgmDestPath = Path.Combine(bgmSavePath, "MainTheme" + '.' + extension[1]);
        if (Directory.Exists(bgmSavePath))
        {
            Directory.Delete(bgmSavePath, true);
        }
        Directory.CreateDirectory(bgmSavePath);
        File.Copy(ProjectData.bgmPath, bgmDestPath);
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
        clip.name = ProjectData.bgmPath;
        bgmClip = clip;
        yield return null;
        isLoadCompelete = true;
    }

    public void SaveBeatMap()
    {
        Debug.Log("세이브 진입");
        CurBeatMap = beatMapManager.SaveBeatMapData();
        SaveDataLocal();
    }
}
