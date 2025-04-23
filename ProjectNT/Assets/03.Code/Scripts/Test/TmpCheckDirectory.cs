using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class TmpCheckDirectory : MonoBehaviour
{
    public ProjectData[] projectList;
    public BeatMapData beatMapData;
    [SerializeField] private MusicChangeAndSelect[] musicChangeAndSelects;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="프로젝트 이름"></param>
    /// 
    private Dictionary<string, Dictionary<Enums.ModeDiff, BeatMapData>> beatMapDic
        = new Dictionary<string, Dictionary<Enums.ModeDiff, BeatMapData>>();

    private void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "Projects");
        projectList = GetLobbySongData(path);

        Debug.Log("프로젝트 리스트 갯수 : " + projectList.Length);
        if (projectList.Length == 0)
            return;
        for (int i = 0; i < projectList.Length; i++)
            beatMapDic.Add(projectList[i].projectName, SetBeatMapData(projectList[i], path));
        SetProjectPanel(projectList);
    }

    public Dictionary<Enums.ModeDiff, BeatMapData> SetBeatMapData(ProjectData projectData, string path)
    {
        string beatMapPath = Path.Combine(path, projectData.projectName, "BeatMapData");
        return LoadBeatMapData(beatMapPath);
    }

    public void SetProjectPanel(ProjectData[] projectList)
    {
        print("SetProjectPanel 1");
        musicChangeAndSelects = FindObjectsOfType<MusicChangeAndSelect>(true);
        string path = Path.Combine(Application.persistentDataPath, "Projects");

        // 모든 곡 데이터 로드

        // 싱글/멀티 플레이용 데이터 분리
        List<TitleMusicData> singleModeData = new List<TitleMusicData>();
        List<TitleMusicData> multiModeData = new List<TitleMusicData>();

        foreach (ProjectData project in projectList)
        {
            List<TitleMusicData> musicData = ProjectDataToTitleMusicData(project);

            if (musicData == null) continue;
            for (int i = 0; i < musicData.Count; i++)
            {
                switch (musicData[i].modeDiff)
                {
                    case Enums.ModeDiff.SOLO_EASY:
                    case Enums.ModeDiff.SOLO_HARD:
                    case Enums.ModeDiff.SOLO_NORMAL:
                    case Enums.ModeDiff.SOLO_EXTREAM:
                        singleModeData.Add(musicData[i]);
                        break;
                    default:
                        multiModeData.Add(musicData[i]);
                        break;
                }
            }
        }
        print("멀티 로드 카운트 : "+multiModeData.Count);
        // 게임 타입에 맞는 데이터만 전달
        foreach (MusicChangeAndSelect selector in musicChangeAndSelects)
        {
            if (selector.GetComponent<GamePlayUI>().gameType == UIGameType.Single)
            {
                if (singleModeData.Count != 0)
                    selector.Init(singleModeData);
            }
            else
            {
                if (multiModeData.Count != 0)
                    selector.Init(multiModeData);
            }
        }
    }

    private List<TitleMusicData> ProjectDataToTitleMusicData(ProjectData projectData)
    {
        List<TitleMusicData> musicDataList = new List<TitleMusicData>();

        // data.musicName = projectData.projectName;
        // data.musicAlbumArtSprit = ByteToSprite(projectData.thumbnailData);
        // data.musicArtist = projectData.artistName;
        // data.projectName = projectData.projectName;
        // data.musicClip = GameManager.Instance.projectToLoadedData.GetBgmAudioClip(projectData.projectName, "BGM_Highlight.wav");
        string path = Path.Combine(Application.persistentDataPath, "Projects", projectData.projectName, "BeatMapData");
        if (!Directory.Exists(path))
        {
            Debug.LogError("BeatMapData 경로가 존재하지 않습니다.");
            return null;
        }
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            Enums.ModeDiff modeDiff = (Enums.ModeDiff)Enum.Parse(typeof(Enums.ModeDiff), Path.GetFileName(file));
            if (modeDiff != Enums.ModeDiff.DUO1_EASY && modeDiff != Enums.ModeDiff.SOLO_EASY)
                continue;
            TitleMusicData data = ScriptableObject.CreateInstance<TitleMusicData>();
            data.Init(projectData);
            data.modeDiff = modeDiff;
            musicDataList.Add(data);
        }
        return musicDataList;
    }

    private ProjectData[] GetLobbySongData(string path)
    {
        if (!Directory.Exists(path)) return null;
        string[] directories = Directory.GetDirectories(path);
        List<ProjectData> res = new List<ProjectData>();
        foreach (string directory in directories)
        {
            string projectInfoPath = Path.Combine(directory, "ProjectInfos");
            if (!File.Exists(projectInfoPath))
                continue;
            ProjectData projectData = JsonUtility.FromJson<ProjectData>(File.ReadAllText(projectInfoPath));
            res.Add(projectData);
        }
        return res.ToArray();
    }

    private BeatMapData GetBeatMapData(string directory)
    {
        string filePath = Path.Combine(directory, "BeatMapData");
        if (!File.Exists(filePath))
            return null;
        string json = File.ReadAllText(filePath);
        BeatMapData loadedData = JsonUtility.FromJson<BeatMapData>(json);
        return loadedData;
    }

    public Dictionary<Enums.ModeDiff, BeatMapData> LoadBeatMapData(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }
        string jsonData = File.ReadAllText(path);
        return DictionaryJsonUtility.FromJson<Enums.ModeDiff, BeatMapData>(jsonData);
    }

    private Sprite ByteToSprite(byte[] bytes, string filePath = null)
    {
        Texture2D texture = new Texture2D(100, 100);
        texture.LoadImage(bytes);
        //스프라이트 만들기
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        sprite.name = texture.name;
        return sprite;
    }
}
