using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestLoad : MonoBehaviour
{
    public string songName = "";
    private BeatMapManager _beatMapManager;
    private BeatMapData _beatMapData;
    private string _tempSavePath;

    private void Awake()
    {
        _beatMapManager = FindObjectOfType<BeatMapManager>();
        _beatMapData = new BeatMapData
        {
            songData = new SongData(),
            gridSetting = new GridSetting(),
            nodes = new List<NodeData>()
        };

        _tempSavePath = Path.Combine(Application.dataPath, "tempBeatMap.json");
    }

    private void Start()
    {

        _beatMapData.songData.songName = songName;
        _beatMapData.songData.songLength = 60f;
        _beatMapData.gridSetting.BPM = 128f;
        _beatMapData.gridSetting.Column = 4;
        _beatMapData.gridSetting.BeatNum = 2;
        print(1);
        _beatMapManager.LoadBeatMapData(EditorDataManager.Instance.beatMapDic[Enums.ModeDiff.SOLO_EXTREAM]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Save();
            SaveToJson();
        }

        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFromJson();
        }
    }

    private void SaveToJson()
    {
        BeatMapData currentData = _beatMapManager.SaveBeatMapData();
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(_tempSavePath, json);
        print("저장됨");
    }

    private void LoadFromJson()
    {
        if (File.Exists(_tempSavePath))
        {
            string json = File.ReadAllText(_tempSavePath);
            BeatMapData loadedData = JsonUtility.FromJson<BeatMapData>(json);
            _beatMapManager.LoadBeatMapData(loadedData);
            print("로딩됨");
        }
        else
        {
            Debug.LogWarning("파일 저장 경로가 잘못됨");
        }
    }

    //BeatMapData로 나오니까 이걸 string으로 바꿔야함
    private void Save()
    {
        BeatMapData tempData = _beatMapManager.SaveBeatMapData();
        print($"저장완료");
        print($"노래 이름 : {tempData.songData.songName}");
        print($"BPM : {tempData.gridSetting.BPM}");
        print($"Column : {tempData.gridSetting.Column}");
    }
}
