using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class NodeContainer : MonoBehaviour
{
    // [SerializeField] private TextMeshProUGUI phase1Text;
    // [SerializeField] private TextMeshProUGUI phase2Text;
    [SerializeField] private TextMeshProUGUI songLengthText;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform nodeParent;

    public BeatMapData CurrentBeatMapData => _currentBeatMapData;

    private Camera _editorCamera;
    private GridManager _gridManager;
    private AudioSourceManager _audioSourceManager;
    private Texture2D _texture;
    private Node[,] _nodeGrid;
    private int _totalBeats;
    private GameObject _previewNode;
    private Color _previewNodeColor = new Color(1, 0, 0, 0.5f);
    private Material myMaterial;
    private Material myMaterialPrefab;
    private BeatMapData _currentBeatMapData;

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _editorCamera = FindObjectOfType<Camera>();
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
        //_texture = GetComponent<Renderer>().material.mainTexture as Texture2D;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => _gridManager.GridTexture != null);
        InitializeNodeGrid();
        _gridManager.gridInfoCallback += GridValueChanged;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Slider로 Plane 위치 변경이 안됨
            //Ray ray = _editorCamera.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out RaycastHit hit))
            //{
            //    if (EventSystem.current.IsPointerOverGameObject() == false)
            //    {
            //        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);
            //    }
            //}
            //if (EventSystem.current.IsPointerOverGameObject())
            //{
            //    return;
            //}
            PlaceNodeMousePosition();
        }

        (int column, int beatIndex) = GetGridPositionFromMouse();
        CreatePreviewNode(column, beatIndex);

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    SaveBeatMap();
        //}
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (_currentBeatMapData == null)
            {
                print("정보 없음");
                return;
            }
            PrintAllNode();
        }
    }

    private void GridValueChanged()
    {
        InitializeNodeGrid();
    }

    //노드 이차원 배열 생성
    private void InitializeNodeGrid()
    {
        _totalBeats = _gridManager.TotalBeats;
        _nodeGrid = new Node[_gridManager.Column, _totalBeats];
        print($"그리드 생성 완료 : {_gridManager.Column} x {_totalBeats}");
    }

    private void PlaceNodeMousePosition()
    {
        (int column, int beatIndex) = GetGridPositionFromMouse();
        if (column >= 0 && column < 4 && beatIndex >= 0 && beatIndex < _totalBeats)
        {
            print($"행 : {column}, 열 : {beatIndex}");
            CreateNode(column, beatIndex);
        }
    }

    //여기에서 그리드로 좌표 변환
    private (int column, int beatIndex) GetGridPositionFromMouse()
    {
        Ray ray = _editorCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        {
            Vector3 localHit = transform.InverseTransformPoint(hit.point);

            // cell의 실제 크기 계산
            float cellWidth = 10f / _gridManager.Column;
            float cellHeight = 10f / _gridManager.TotalBeats;

            //-5~5 범위의 hit 좌표를 0~10 범위로 변환
            float posX = localHit.x + 5f;
            float posZ = localHit.z + 5f;

            //실제 위치를 cell 크기로 나누어 grid 인덱스 계산
            int column = (int)(posX / cellWidth);
            int beatIndex = (int)(posZ / cellHeight);

            // 범위 체크
            if (column < 0) column = 0;
            if (column >= _gridManager.Column) column = _gridManager.Column - 1;
            if (beatIndex < 0) beatIndex = 0;
            if (beatIndex >= _totalBeats) beatIndex = _totalBeats - 1;

            return (column, beatIndex);
        }
        return (-1, -1);
    }

    //임시 노드 생성
    private void CreatePreviewNode(int column, int beatIndex)
    {
        if (column < 0 || beatIndex < 0 || column >= _gridManager.Column || beatIndex >= _totalBeats)
        {
            if (_previewNode != null)
            {
                Destroy(_previewNode);
                _previewNode = null;
            }
            return;
        }

        if (_nodeGrid[column, beatIndex] != null)
        {
            if (_previewNode != null)
            {
                Destroy(_previewNode);
                _previewNode = null;
            }
            return;
        }

        if (_previewNode == null)
        {
            _previewNode = Instantiate(nodePrefab);
            Material previewNodeMaterial = _previewNode.GetComponent<MeshRenderer>().material;
            previewNodeMaterial.color = _previewNodeColor;
            _previewNode.transform.SetParent(nodeParent, true);
            _previewNode.transform.localScale = _previewNode.transform.localScale;
        }

        Vector2 gridPoint = _gridManager.GridPoint[column, beatIndex];
        _previewNode.transform.position = nodeParent.TransformPoint(new Vector3(gridPoint.x, 0.1f, gridPoint.y));
    }

    private void CreateNode(int column, int beatIndex)
    {
        if (_nodeGrid[column, beatIndex] != null)
        {
            Debug.LogWarning($"이미 노드가 있습니다");
            return;
        }

        GameObject nodeObj = Instantiate(nodePrefab);
        Node node = nodeObj.GetComponent<Node>();

        if (node != null)
        {
            node.TestPrint();
            Vector2 gridPoint = _gridManager.GridPoint[column, beatIndex];
            nodeObj.transform.position = nodeParent.TransformPoint(new Vector3(gridPoint.x, 0.1f, gridPoint.y));
            node.transform.SetParent(nodeParent, true);
            node.transform.localScale = nodeObj.transform.localScale;
            node.InitializeNode(new Vector2Int(column, beatIndex));
            _nodeGrid[column, beatIndex] = node;
            //node.Initialize(column, beatIndex * (60f / _gridManager.BPM));
        }
    }

    private List<NodeData> GetAllNodeData()
    {
        List<NodeData> nodeDataList = new List<NodeData>();

        for (int i = 0; i < _gridManager.Column; i++)
        {
            for (int j = 0; j < _totalBeats; j++)
            {
                if (_nodeGrid[i, j] != null)
                {
                    NodeData nodeData = _nodeGrid[i, j].GetNodeData();
                    nodeDataList.Add(nodeData);
                }
            }
        }

        return nodeDataList;
    }

    public void InitializeWithNodeData(List<NodeData> nodeDataList)
    {
        ClearAllNodes();
        InitializeNodeGrid();

        foreach (var nodeData in nodeDataList)
        {
            CreatNodeFromData(nodeData);
        }
    }

    //모든 노드 정보 파괴
    private void ClearAllNodes()
    {
        if (_nodeGrid == null)
        {
            print($"그리드에 아무것도 없습니다");
            return;
        }

        for (int i = 0; i < _gridManager.Column; i++)
        {
            for (int j = 0; j < _totalBeats; j++)
            {
                if (_nodeGrid[i, j] != null)
                {
                    Destroy(_nodeGrid[i, j].gameObject);
                    _nodeGrid[i, j] = null;
                }
            }
        }
    }

    private void CreatNodeFromData(NodeData nodeData)
    {
        if (nodeData.index.x >= _gridManager.Column || nodeData.index.y >= _totalBeats)
        {
            Debug.LogWarning($"그리드가 이상한 곳에 찍혀있음 : ({nodeData.index.x}, {nodeData.index.y})");
            return;
        }

        //if (_nodeGrid[nodeData.index.x, nodeData.index.y] != null)
        //{
        //    print("이미 그리드 위에 노드가 있습니다");
        //    Destroy(_nodeGrid[nodeData.index.x, nodeData.index.y].gameObject);
        //    _nodeGrid[nodeData.index.x, nodeData.index.y] = null;
        //}

        GameObject nodeObj = Instantiate(nodePrefab);
        Node node = nodeObj.GetComponent<Node>();

        if (node != null)
        {
            Vector2 gridPoint = _gridManager.GridPoint[nodeData.index.x, nodeData.index.y];
            nodeObj.transform.position = nodeParent.TransformPoint(new Vector3(gridPoint.x, 0.1f, gridPoint.y));
            node.transform.SetParent(nodeParent, true);
            node.transform.localScale = nodeObj.transform.localScale;

            _nodeGrid[nodeData.index.x, nodeData.index.y] = node;
            //node.InitializeNode(nodeData.index);
            node.SetNodeData(nodeData);
        }
    }

    public void InitializeWithSongData(SongData songData)
    {
        songData ??= new SongData { songLength = 0, phase2 = 0, phase3 = 0 };
        songLengthText.text = songData.songLength.ToString();

    }

    //비트맵 저장하는 함수
    public void SaveBeatMap()
    {
        _currentBeatMapData = new BeatMapData();

        _currentBeatMapData.songData = new SongData
        {
            songName = _audioSourceManager.AudioSource.clip.name,
            songLength = _audioSourceManager.AudioDuration,
            phase2 = EditorDataManager.Instance.CurBeatMap.songData.phase2,
            phase3 = EditorDataManager.Instance.CurBeatMap.songData.phase3,
        };

        _currentBeatMapData.gridSetting = new GridSetting
        {
            BPM = _gridManager.BPM,
            Column = _gridManager.Column,
            BeatNum = _gridManager.BeatNum,
        };

        _currentBeatMapData.nodes = GetAllNodeData();
        foreach (var node in _currentBeatMapData.nodes)
        {
            print($"저장된 노드의 위치 : {node.index}");
        }
        //for (int i = 0; i < _gridManager.Column; i++)
        //{
        //    for (int j = 0; j < _gridManager.TotalBeats; j++)
        //    {
        //        if (_nodeGrid != null)
        //        {
        //            //_currentBeatMapData.nodes.Add(_nodeGrid[i, j])
        //        }
        //    }
        //}
    }

    private void PrintAllNode()
    {
        print($"노드의 양 : {_currentBeatMapData.nodes.Count()}");
    }
}
