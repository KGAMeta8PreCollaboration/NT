using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class TmpCheckDirectory : Singleton<TmpCheckDirectory>
{
    public ProjectData[] projectList;
    public BeatMapData beatMapData;
    [SerializeField] private MusicChangeAndSelect[] musicChangeAndSelects;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="프로젝트 이름"></param>
    /// 
    public Dictionary<string, Dictionary<Enums.ModeDiff, BeatMapData>> beatMapDic
        = new Dictionary<string, Dictionary<Enums.ModeDiff, BeatMapData>>();
    
    private void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "Projects");
        projectList = GetLobbySongData(path);
        if (projectList.Length == 0)
            return;
        SetProjectPanel(projectList);

        for (int i = 0; i < projectList.Length; i++)
            beatMapDic.Add(projectList[i].projectName, SetBeatMapData(projectList[i], path));
    }

    public Dictionary<Enums.ModeDiff, BeatMapData> SetBeatMapData(ProjectData projectData, string path)
    {
        string beatMapPath = Path.Combine(path, projectData.projectName, "BeatMapData");
        print("경로 : " + beatMapPath);
        return LoadBeatMapData(beatMapPath);
    }

    public void SetProjectPanel(ProjectData[] projectList)
    {
        List<TitleMusicData> titleMusicData = 
            projectList.Select(ProjectDataToTitleMusicData).ToList();

        foreach (MusicChangeAndSelect t in musicChangeAndSelects)
            t.Init(titleMusicData);
    }

    private TitleMusicData ProjectDataToTitleMusicData(ProjectData projectData)
    {
        TitleMusicData data = new TitleMusicData();
        data.musicName = projectData.projectName;
        data.musicAlbumArtSprit = ByteToSprite(projectData.thumbnailData);
        data.musicDescription = projectData.artistName;
        data.projectName = projectData.projectName;
        return data;
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
