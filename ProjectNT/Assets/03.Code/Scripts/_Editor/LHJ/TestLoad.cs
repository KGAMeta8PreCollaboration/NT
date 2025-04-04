using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestLoad : MonoBehaviour
{
    private BeatMapManager _beatMapManager;
    private BeatMapData data = new BeatMapData();

    private string _tempSavePath;
    private void Awake()
    {
        _beatMapManager = FindObjectOfType<BeatMapManager>();
    }

    private void Start()
    {
        AudioClip audioClip = Resources.Load<AudioClip>("_SongEditor/LoadedSongs/Sample1");
        if (audioClip == null)
        {
            Debug.LogError("오디오 클립을 찾을 수 없습니다.");
            return;
        }

        data = new BeatMapData();
        data.songData = new SongData()
        {
            songLength = audioClip.length,  // 실제 오디오 길이 사용
            phase2 = 0,
            phase3 = 0,
        };
        data.gridSetting = new GridSetting()
        {
            BPM = 128,
            Column = 4,
            BeatNum = 8  // 0이 아닌 값으로 설정 (예: 4박자)
        };
        data.nodes = new List<NodeData>();
        data.upperNodes = new List<UpperNodeData>();

        _tempSavePath = Path.Combine(Application.persistentDataPath, "tempBeatMap.json");
        _beatMapManager.LoadBeatMapData(data);  // 주석 해제
        //_beatMapManager.LoadBeatMapData(data);
    }
    //    public string songName = "";
    //    private BeatMapManager _beatMapManager;
    //    private BeatMapData _beatMapData;
    //    private string _tempSavePath;

    //    private void Awake()
    //    {
    //        _beatMapManager = FindObjectOfType<BeatMapManager>();
    //        _beatMapData = new BeatMapData
    //        {
    //            songData = new SongData(),
    //            gridSetting = new GridSetting(),
    //            nodes = new List<NodeData>()
    //        };

    //        _tempSavePath = Path.Combine(Application.persistentDataPath, "tempBeatMap.json");
    //    }

    //    private void Start()
    //    {

    //        _beatMapData.songData.songName = songName;
    //        _beatMapData.songData.songLength = 60f;
    //        _beatMapData.gridSetting.BPM = 128f;
    //        _beatMapData.gridSetting.Column = 4;
    //        _beatMapData.gridSetting.BeatNum = 2;
    //        print(1);
    //        // _beatMapManager.LoadBeatMapData(EditorDataManager.Instance.beatMapDic[Enums.ModeDiff.SOLO_EXTREAM]);
    //    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Save();
            SaveToJson();
            //EditorDataManager.Instance.CurBeatMap = _beatMapManager.SaveBeatMapData();
        }

        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFromJson();
            //_beatMapManager.LoadBeatMapData(EditorDataManager.Instance.CurBeatMap);
        }
    }

    private void SaveToJson()
    {
        BeatMapData currentData = _beatMapManager.SaveBeatMapData();
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(_tempSavePath, json);
        Debug.Log($"저장 경로: {_tempSavePath}");
        Debug.Log($"저장된 노드 수: {currentData.nodes.Count}");
        Debug.Log($"저장된 상단 노드 수: {currentData.upperNodes.Count}");
    }

    private void LoadFromJson()
    {
        if (File.Exists(_tempSavePath))
        {
            string json = File.ReadAllText(_tempSavePath);
            BeatMapData loadedData = JsonUtility.FromJson<BeatMapData>(json);
            Debug.Log($"로드된 파일 경로: {_tempSavePath}");
            Debug.Log($"로드된 노드 수: {loadedData.nodes.Count}");
            Debug.Log($"로드된 상단 노드 수: {loadedData.upperNodes.Count}");

            if (loadedData.nodes.Count == 0)
            {
                Debug.LogWarning("로드된 노드 데이터가 비어있습니다!");
                return;
            }

            _beatMapManager.LoadBeatMapData(loadedData);
        }
        else
        {
            Debug.LogError($"파일을 찾을 수 없습니다: {_tempSavePath}");
        }
    }

    //BeatMapData로 나오니까 이걸 string으로 바꿔야함
    //private void Save()
    //{
    //    BeatMapData tempData = _beatMapManager.SaveBeatMapData();
    //    print($"저장완료");
    //    print($"노래 이름 : {tempData.songData.songName}");
    //    print($"BPM : {tempData.gridSetting.BPM}");
    //    print($"Column : {tempData.gridSetting.Column}");
    //}
}
