using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BeatMapManager : MonoBehaviour
{
    public bool isLoaded = false;

    private AudioSourceManager _audioSourceManager;
    private GridManager _gridManager;
    private NodeContainer _nodeContainer;

    private void Awake()
    {
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
        _gridManager = FindObjectOfType<GridManager>();
        _nodeContainer = FindObjectOfType<NodeContainer>();
        isLoaded = false;
    }

    //BeatMapData로 넘겨줄거임
    public BeatMapData SaveBeatMapData()
    {
        _nodeContainer.SaveBeatMap();
        return _nodeContainer.CurrentBeatMapData;
    }

    //얘도 BeatMapData로 받아올거임
    public void LoadBeatMapData(BeatMapData beatMapData)
    {
        if (beatMapData == null)
        {
            Debug.LogWarning("넘어온 비트맵 정보가 없습니다.");
            return;
        }
        print(2);
        _audioSourceManager.InitializeFromBeatMapManager(beatMapData.songData);
        _gridManager.InitializeFromBeatMapManager(beatMapData.gridSetting);

        isLoaded = true;
    }
}
