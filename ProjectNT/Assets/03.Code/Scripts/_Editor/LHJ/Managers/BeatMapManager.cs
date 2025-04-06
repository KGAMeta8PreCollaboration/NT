using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;
using TMPro;

public class BeatMapManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bpmText;

    public bool isLoaded = false;

    private AudioSourceManager _audioSourceManager;
    private GridManager _gridManager;
    private UpperNodeTest _upperNodeTest;
    private NCT _nct;

    private void Awake()
    {
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
        _gridManager = FindObjectOfType<GridManager>();
        _upperNodeTest = FindObjectOfType<UpperNodeTest>();
        _nct = FindObjectOfType<NCT>();
        isLoaded = false;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //Temp();
        //    SaveBeatMapData();
        //}
    }

    public void Temp()
    {
        if (_nct.NodeGrid == null)
        {
            Debug.LogWarning("NodeGrid에 암것도 없음");
            return;
        }
        print("=== 노드 정보 출력 시작 ===");
        for (int x = 0; x < _gridManager.Column; x++)
        {
            for (int y = 0; y < _nct.RowGridNum; y++)
            {
                if (_nct.NodeGrid[x, y] != null)
                {
                    string nodeType = _nct.NodeGrid[x, y] is LongNode ? "롱노드" : "일반노드";
                    Debug.Log($"위치 [{x}, {y}]: {nodeType}");
                }
            }
        }
        print("=== 노드 정보 출력 끝 ===");
    }

    //BeatMapData로 넘겨줄거임
    public BeatMapData SaveBeatMapData()
    {
        BeatMapData data = new BeatMapData();
        data.songData = new SongData()
        {
            songLength = _audioSourceManager.AudioDuration,
            phase2 = _audioSourceManager.phase2,
            phase3 = _audioSourceManager.phase3,
        };
        data.gridSetting = new GridSetting()
        {
            BPM = _gridManager.BPM,
            Column = _gridManager.Column,
            BeatNum = _gridManager.BeatNum,
        };
        data.nodes = new List<NodeData>();
        for (int x = 0; x < _gridManager.Column; x++)
        {
            for (int y = 0; y < _nct.RowGridNum; y++)
            {
                if (_nct.NodeGrid[x, y] == null) continue;

                if (_nct.NodeGrid[x, y] is LongNode longNode)
                {
                    // 시작 위치가 아닌 경우 스킵
                    if (new Vector2Int(x, y) != longNode.StartIndex) continue;

                    // 롱노드 데이터 저장
                    NodeData nodeData = new NodeData
                    {
                        index = longNode.StartIndex,
                        endIndex = longNode.EndIndex,
                        nodeType = EditorNoteType.LongNote,
                        keySound = longNode._keySound
                    };
                    print($"저장된 롱노드 인덱스 : {longNode.StartIndex} ~ {longNode.EndIndex}");
                    data.nodes.Add(nodeData);
                }
                else
                {
                    // 일반 노드 데이터 저장
                    NodeData nodeData = new NodeData
                    {
                        index = new Vector2Int(x, y),
                        nodeType = EditorNoteType.ShortNote,
                        keySound = _nct.NodeGrid[x, y]._keySound
                    };
                    print($"저장된 하단노드 인덱스 : {x},{y}");
                    data.nodes.Add(nodeData);
                }
            }
        }
        data.upperNodes = new List<UpperNodeData>();
        foreach (var upperNode in _upperNodeTest._upperNodeDic)
        {
            UpperNodeData upperNodeData = new UpperNodeData
            {
                gridIndex = upperNode.Key,
                nodeIndexs = new List<int>(upperNode.Value)
            };
            print($"저장된 상단노드 그리드 인덱스 : {upperNode.Key}, 노드 인덱스 : [{string.Join(", ", upperNode.Value)}]");
            data.upperNodes.Add(upperNodeData);
        }

        return data;
    }

    //얘도 BeatMapData로 받아올거임
    public void LoadBeatMapData(BeatMapData beatMapData)
    {
        if (beatMapData == null)
        {
            Debug.LogWarning("넘어온 비트맵 정보가 없습니다.");
            return;
        }

        StartCoroutine(LoadBeatMapDataCoroutine(beatMapData));
    }

    private IEnumerator LoadBeatMapDataCoroutine(BeatMapData beatMapData)
    {
        isLoaded = false;
        if (_audioSourceManager == null || _gridManager == null || _nct == null || _upperNodeTest == null)
        {
            Debug.LogError("필요한 컴포넌트가 없습니다.");
            yield break;
        }

        AudioClip audioSource = Resources.Load<AudioClip>("_SongEditor/LoadedSongs/Sample1");
        //일단 모두 초기화
        _nct.ClearAllNodes();
        _upperNodeTest._upperNodeDic.Clear();

        //print(audioSource);

        //1. 오디오 Source 초기화
        _audioSourceManager.InitializeFromBeatMapManager(audioSource);
        _audioSourceManager.InitializeFromSongData(beatMapData.songData);
        yield return new WaitUntil(() => _audioSourceManager.AudioSource.clip != null);

        //2. grid 초기화
        _gridManager.InitializeFromBeatMapManager(beatMapData.gridSetting);
        yield return new WaitUntil(() => _nct.isLoaded == true);

        // 3. node 초기화
        if (beatMapData.nodes != null && beatMapData.nodes.Count > 0)
        {
            _nct.InitializeWithNodeData(beatMapData.nodes);
            Debug.Log($"로드된 노드 수: {beatMapData.nodes.Count}");
        }

        // 4. 상단 노드 초기화
        if (beatMapData.upperNodes != null && beatMapData.upperNodes.Count > 0)
        {
            _upperNodeTest.InitializeWithNodeData(beatMapData.upperNodes);
            Debug.Log($"로드된 상단 노드 수: {beatMapData.upperNodes.Count}");
        }

        // 5. 기타 정보 초기화
        bpmText.text = $"BPM : ({beatMapData.gridSetting.BPM.ToString()})";
        isLoaded = true;
    }
}
