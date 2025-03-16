using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoad : MonoBehaviour
{
    public string songName = "";
    private BeatMapManager _beatMapManager;
    private BeatMapData _beatMapData;

    private void Awake()
    {
        _beatMapManager = FindObjectOfType<BeatMapManager>();
        _beatMapData = new BeatMapData
        {
            songData = new SongData(),
            gridSetting = new GridSetting(),
            nodes = new List<NodeData>()
        };
    }

    private void Start()
    {
        _beatMapData.songData.songName = songName;
        _beatMapData.songData.songLength = 60f;
        _beatMapData.gridSetting.BPM = 128f;
        _beatMapData.gridSetting.Column = 4;
        _beatMapData.gridSetting.BeatNum = 2;
        print(1);
        _beatMapManager.LoadBeatMapData(_beatMapData);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Save();
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
