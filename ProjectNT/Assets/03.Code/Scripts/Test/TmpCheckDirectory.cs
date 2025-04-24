using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TmpCheckDirectory : MonoBehaviour
{
    public ProjectData[] projectList;
    [SerializeField] private MusicChangeAndSelect[] musicChangeAndSelects;


    /// <param name="프로젝트 이름"></param>
    private Dictionary<string, Dictionary<Enums.ModeDiff, BeatMapData>> beatMapDic
        = new Dictionary<string, Dictionary<Enums.ModeDiff, BeatMapData>>();
    
    private void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "Projects");
        projectList = GetLobbySongData(path);

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
        musicChangeAndSelects = FindObjectsOfType<MusicChangeAndSelect>(true);
        MusicChangeAndSelect singleModeSelector = musicChangeAndSelects.ToList().Find(x => x.GetComponent<GamePlayUI>().gameType == UIGameType.Single);
        MusicChangeAndSelect multiModeSelector = musicChangeAndSelects.ToList().Find(x => x.GetComponent<GamePlayUI>().gameType == UIGameType.Muliti);
        // 싱글/멀티 플레이용 데이터 분리
        List<TitleMusicData> singleModeData = new List<TitleMusicData>();
        List<TitleMusicData> multiModeData = new List<TitleMusicData>();
        
        foreach (ProjectData project in projectList)
        {
            TitleMusicData musicData = ProjectDataToTitleMusicData(project);
            if (musicData == null) continue;
            if ((musicData.modeDiff & Enums.SOLO_DIFF_MODES) != 0)
                singleModeData.Add(musicData);
            if ((musicData.modeDiff & Enums.MULTI_DIFF_MODES) != 0)
                multiModeData.Add(musicData);
        }
        singleModeData.Sort();
        multiModeData.Sort();
        singleModeSelector.Init(singleModeData);
        multiModeSelector.Init(multiModeData);
    }

    private TitleMusicData ProjectDataToTitleMusicData(ProjectData projectData)
    {
        TitleMusicData musicDataList = ScriptableObject.CreateInstance<TitleMusicData>();
        musicDataList.Init(projectData);

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
            // if (modeDiff != Enums.ModeDiff.DUO1_EASY && modeDiff != Enums.ModeDiff.SOLO_EASY)
            //     continue;
            // if (musicDataList.modeDiffs.Contains(modeDiff))
            //     continue;
            // musicDataList.modeDiffs.Add(modeDiff);
            musicDataList.modeDiff |= modeDiff;
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
