using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TmpCheckDirectory : Singleton<TmpCheckDirectory>
{
    public ProjectData[] projectList;
    public BeatMapData beatMapData;

    [SerializeField] private MusicChangeAndSelect[] musicChangeAndSelects;
    public Dictionary<Enums.ModeDiff, BeatMapData> beatMapDic;
    public Dictionary<string, Dictionary<Enums.ModeDiff, BeatMapData>> beatMapDicc = new Dictionary<string, Dictionary<Enums.ModeDiff, BeatMapData>>();

    private void Start()
    {
        // musicChangeAndSelects = FindObjectsOfType<MusicChangeAndSelect>();
        // print("musicChangeAndSelects size : " + musicChangeAndSelects.Length);

        //string path = Application.persistentDataPath + "/Projects";
        string path = Path.Combine(Application.persistentDataPath, "Projects");
        projectList = GetLobbySongData(path);
        print("projectList size : " + projectList.Length);
        if (projectList.Length == 0)
            return;
        SetProjectPanel(projectList);

        for (int i = 0; i < projectList.Length; i++)
        {
            print($"프로젝트 이름 : {projectList[i].projectName}");
            beatMapDicc.Add(projectList[i].projectName, SetBeatMapData(projectList[i], path));
        }

        // beatMapDic = SetBeatMapData(projectList[0], path);
    }


    public Dictionary<Enums.ModeDiff, BeatMapData> SetBeatMapData(ProjectData projectData, string path)
    {
        print("경로 : " + path + "/" + projectData.projectName + "/BeatMapData");
        return LoadBeatMapData(path + "/" + projectData.projectName + "/BeatMapData");
    }

    public void SetProjectPanel(ProjectData[] projectList)
    {
        List<TitleMusicData> titleMusicData =
            projectList.Select(ProjectDataToTitleMusicData).ToList();

        // musicChangeAndSelects = FindObjectsOfType<MusicChangeAndSelect>();
        // print("musicChangeAndSelects size : " + musicChangeAndSelects.Length);

        print("SetProjectPanel : " + titleMusicData.Count);
        foreach (MusicChangeAndSelect t in musicChangeAndSelects)
            t.Init(titleMusicData);
    }

    private TitleMusicData ProjectDataToTitleMusicData(ProjectData projectData)
    {
        TitleMusicData data = new TitleMusicData();
        data.musicName = projectData.projectName;
        data.musicAlbumArtSprit = ByteToSprite(projectData.thumbnailData);
        data.musicDescription = projectData.artistName;
        return data;
    }

    private ProjectData[] GetLobbySongData(string path)
    {
        if (!Directory.Exists(path)) return null;
        string[] strings = Directory.GetDirectories(path);
        List<ProjectData> res = new List<ProjectData>();
        foreach (string item in strings)
        {
            if (!File.Exists(item + "/ProjectInfos"))
                continue;
            ProjectData projectData = JsonUtility.FromJson<ProjectData>(File.ReadAllText(item + "/ProjectInfos"));
            res.Add(projectData);
        }
        return res.ToArray();
    }

    private BeatMapData GetBeatMapData(string directory)
    {
        string filePath = $"{directory}/BeatMapData";
        if (!File.Exists(filePath))
            return null;
        string json = File.ReadAllText(filePath);
        BeatMapData loadedData = JsonUtility.FromJson<BeatMapData>(json);
        return loadedData;
    }

    public Dictionary<Enums.ModeDiff, BeatMapData> LoadBeatMapData(string path)
    {
        string jsonData;
        if (!File.Exists(path))
        {
            return null;
        }
        jsonData = File.ReadAllText(path);
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
