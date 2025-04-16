using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UIElements;

[RequireComponent(typeof(SpriteRenderer))]
public class NCT : MonoBehaviour
{
    [SerializeField] GameObject bpmLinePrefab;
    [SerializeField] GameObject beatLinePrefab;
    [SerializeField] GameObject columnLinePrefab;
    [SerializeField] GameObject previewLowNodePrefab;
    [SerializeField] GameObject previewLongNodePrefab;
    [SerializeField] GameObject lowNodePrefab;
    [SerializeField] GameObject longNodePrefab;
    [SerializeField] Transform nodeParent;
    [SerializeField] int width = 128; //넓이
    [SerializeField] int pixelPerSecond = 100; //높이
    [SerializeField] Camera cam;

    [SerializeField] TextMeshProUGUI stateTest;

    [SerializeField] private GameObject upperGridMarkPrefab;
    private Dictionary<int, Dictionary<bool, GameObject>> _upperGridMarks = new Dictionary<int, Dictionary<bool, GameObject>>();
    private UpperNodeHandler _upperNodeHandler;

    public int RowGridNum;

    public double cellHeight = 0;

    public float bpmLineLength = 0;

    private GridManager _gridManager;
    private Waveform _waveform;
    private AudioSourceManager _audioSourceManager;
    private SpriteRenderer _spriteRenderer;
    private Texture2D _texture;
    private AudioSource _audioSource;

    public float xOffset;
    private float bpmPrefabLineScale;
    private float beatPrefabLineScale;
    private float columnLineScale;

    private List<GameObject> heightGrid = new List<GameObject>();
    private List<GameObject> widthGrid = new List<GameObject>();

    private Node[,] _nodeGrid;
    public Node[,] NodeGrid { get { return _nodeGrid; } }

    private GameObject _previewLowNode;
    private GameObject _previewLongNode;

    private float _bpm;
    private int _column;
    private int _beatNum;

    private INodeState _currentState;

    private Plane tempPlane = new Plane();

    private bool _isMouseInUI = false;

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
        _waveform = FindObjectOfType<Waveform>();
        _upperNodeHandler = FindObjectOfType<UpperNodeHandler>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _currentState = new LowNodeState(this);
        UpdateStateText();
    }

    private void Start()
    {
        _gridManager.InitBeatMap += CreateNodeContainer;

        //_upperNodeHandler.onUpperNodeAdded += CreateUpperGridMark;
        //_upperNodeHandler.onUpperNodeRemoved += RemoveUpperGridMark;
        Debug.Log("NCT START");
    }

    Vector2Int currentIndex = new Vector2Int();
    private void Update()
    {
        //오브젝트 위에서만 상태 변경 가능
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    return;
        //}

        if (IsPointerOverUI())
        {
            HideLowNodePreview();
            HideLongNodePreview();
            return;
        }

        currentIndex = GetGridPositionFromMouse();

        if (Input.GetMouseButtonDown(0))
        {
            _currentState.OnLeftClick(currentIndex);
        }
        if (Input.GetMouseButtonDown(1))
            _currentState.OnRightClick(currentIndex);
        if (Input.GetMouseButtonDown(2))
            _currentState.OnMiddleClick(currentIndex);

        _currentState.UpdatePreview(currentIndex);
    }

    public void ChangeState(INodeState newState)
    {
        _currentState = newState;
        UpdateStateText();
    }

    private void UpdateStateText()
    {
        stateTest.text = _currentState.GetStateName();
    }

    public void InitializeWithNodeData(List<NodeData> nodeDatas)
    {
        if (nodeDatas == null) return;

        // 기존 노드들 제거
        if (_nodeGrid != null)
        {
            for (int x = 0; x < _column; x++)
            {
                for (int y = 0; y < RowGridNum; y++)
                {
                    if (_nodeGrid[x, y] != null)
                    {
                        Destroy(_nodeGrid[x, y].gameObject);
                        _nodeGrid[x, y] = null;
                    }
                }
            }
        }
        _longNodePosition.Clear();

        // 새로운 노드들 생성
        foreach (var nodeData in nodeDatas)
        {
            if (nodeData.nodeType == EditorNoteType.ShortNote)
            {
                CreateLowNode(nodeData.index, nodeData.keySound);
            }
            else if (nodeData.nodeType == EditorNoteType.LongNote)
            {
                _longNodePosition[nodeData.index] = nodeData.endIndex;
                CreateLongNode(nodeData.index, nodeData.endIndex, nodeData.keySound);
            }
        }
    }

    public Action<double> callback;
    public bool isLoaded = false;
    public Action loadComplete;
    private void CreateNodeContainer(float bpm, int column, int beatNum)
    {
        // if (EventSystem.current.IsPointerOverGameObject())
        // {
        //     return;
        // }
        if (bpm == 0)
        {
            Debug.LogWarning("BPM이 0입니다.");
            return;
        }

        print("NCT생성시작");
        isLoaded = false;
        _bpm = bpm;
        _column = column;
        _beatNum = beatNum;
        print($"bpm : {_bpm}, column : {_column}, beatNum : {_beatNum}");

        _audioSource = _audioSourceManager.AudioSource;

        width = 128;
        pixelPerSecond = _waveform.maxNum;

        print($"pixelPerSecond : {pixelPerSecond}");
        int height = Mathf.CeilToInt(_audioSource.clip.length) * pixelPerSecond;
        print(_audioSource.clip.length);
        _texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        Rect rect = new Rect(Vector2.zero, new Vector2(width, height));
        _spriteRenderer.sprite = Sprite.Create(_texture, rect, Vector2.zero);
        print($"그리드 생성");
        xOffset = _spriteRenderer.size.x / 2;
        bpmPrefabLineScale = _spriteRenderer.size.x * 1.2f;
        beatPrefabLineScale = _spriteRenderer.size.x;

        //테스트 용도
        float temp1 = _spriteRenderer.sprite.texture.width;
        float temp2 = _spriteRenderer.sprite.texture.height;

        print($"tmp1 : {temp1}, temp2 : {temp2}");

        //노래의 너비 = 텍스쳐 높이
        float songDuration = _audioSource.clip.length;
        double heightPerSecond = _spriteRenderer.size.y / songDuration;
        //double heightPerSecondd = _spriteRenderer.size.y / songDuration;
        //print($"double일때 값 : {heightPerSecondd}");
        float secondsPerBPM = 60 / bpm;

        double bpmHeight = secondsPerBPM * heightPerSecond;
        print("=====================================\n" +
            $"_spriteRenderer.size.y  : {_spriteRenderer.size.y}\n" +
            $"songDuration : {songDuration}\n" +
            $"heightPerSecond : {heightPerSecond}\n" +
            $"secondsPerBeat : {secondsPerBPM}\n" +
            $"beatHeight : {bpmHeight}\n");

        for (int i = 0; i * bpmHeight < _spriteRenderer.size.y; i++)
        {
            double yPos = i * bpmHeight;
            GameObject bpmLineObj = Instantiate(bpmLinePrefab, new Vector3(xOffset, (float)yPos, 0), Quaternion.identity);
            bpmLineObj.transform.localScale = new Vector3(bpmPrefabLineScale, bpmLinePrefab.transform.localScale.y, bpmLinePrefab.transform.localScale.z);
            bpmLineObj.transform.SetParent(transform);
            BPMLine bpmLine = bpmLineObj.GetComponent<BPMLine>();
            bpmLine.SetBPMText(i, secondsPerBPM);

            bpmLineLength = bpmLineObj.transform.localScale.x;

            //가로 grid에 BPM 추가
            heightGrid.Add(bpmLineObj);
            //bpmLine.Test(heightGrid.Count);

            if (beatNum != 0)
            {
                double beatHeight = bpmHeight / beatNum;

                for (int j = 1; j < beatNum; j++)
                {
                    double y = yPos + j * beatHeight;
                    if (y >= _spriteRenderer.size.y)
                    {
                        break;
                    }
                    GameObject beatLine = Instantiate(beatLinePrefab, new Vector3(xOffset, (float)y, 0), Quaternion.identity);
                    beatLine.transform.localScale = new Vector3(beatPrefabLineScale, beatLinePrefab.transform.localScale.y);
                    beatLine.transform.SetParent(transform);

                    //가로 grid에 Beat 추가
                    heightGrid.Add(beatLine);
                }
            }
        }
        print($"새로 라인 개수 : {heightGrid.Count}");

        if (column <= 0)
        {
            Debug.LogWarning("column 재설정 필요.");
            return;
        }

        float columnSize = _spriteRenderer.size.x / column;
        float yOffset = _spriteRenderer.size.y / 2;
        columnLineScale = _spriteRenderer.size.y;
        for (int j = 1; j < column; j++)
        {
            float xPos = j * columnSize;

            //z축 -0.1은 해결이 필요
            GameObject columnLine = Instantiate(columnLinePrefab, new Vector3(xPos, yOffset, -0.1f), Quaternion.identity);
            columnLine.transform.localScale = new Vector3(columnLinePrefab.transform.localScale.x, columnLineScale);
            columnLine.transform.SetParent(transform);

            //새로 grid에 행 추가
            widthGrid.Add(columnLine);
            //print($"widthGrid.Count : {widthGrid.Count}" );
        }

        _nodeGrid = new Node[_column, heightGrid.Count];
        cellHeight = (double)(heightGrid[1].transform.position.y - heightGrid[0].transform.position.y);

        //NodeContainer가 SpriteRenderer로 생성되므로, 임시의 Plane을 생성해서 비교
        tempPlane = new Plane(Vector3.forward, transform.position);

        callback?.Invoke(cellHeight);
        loadComplete?.Invoke();
        //double temp = (double)(heightGrid[1].transform.position.y - heightGrid[0].transform.position.y);

        RowGridNum = heightGrid.Count;
        isLoaded = true;
        print($"한칸의 넓이 : {cellHeight}");
    }

    private Vector2Int GetGridPositionFromMouse()
    {
        Vector2Int index = new Vector2Int(-1, -1);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (tempPlane.Raycast(ray, out distance) == false)
        {
            // print($"플레인 밖이다 : {index}");
            return index;
        }

        Vector3 worldPoint = ray.GetPoint(distance);
        if (_spriteRenderer.bounds.Contains(worldPoint) == false)
        {
            // print($"플레인 밖이다 : {index}");
            return index;
        }

        int column = (int)(worldPoint.x / (_spriteRenderer.size.x / _column));
        //열
        int row = (int)(worldPoint.y / (_spriteRenderer.size.y / heightGrid.Count));

        return new Vector2Int(column, row);
    }

    public void CreatePreviewLowNode(Vector2Int currentIndex)
    {
        if (_previewLowNode == null)
        {
            _previewLowNode = Instantiate(previewLowNodePrefab);
            _previewLowNode.transform.SetParent(nodeParent, true);
            _previewLowNode.transform.localScale = previewLowNodePrefab.transform.localScale;
            _previewLowNode.SetActive(false);
            // print($"하단 노드 생성됨");
        }

        if (currentIndex.x < 0 || currentIndex.y < 0 || currentIndex.x >= _column || currentIndex.y >= heightGrid.Count)
        {
            if (_previewLowNode != null)
            {
                _previewLowNode.SetActive(false);
            }
            return;
        }

        if (_nodeGrid[currentIndex.x, currentIndex.y] != null)
        {
            if (_previewLowNode != null)
            {
                _previewLowNode.SetActive(false);
            }
            return;
        }

        _previewLowNode.SetActive(true);
        // print($"현재 좌표 : {currentIndex.x} X {currentIndex.y}");

        float columnSize = _spriteRenderer.size.x / _column;
        float rowSize = _spriteRenderer.size.y / (heightGrid.Count - 1); //0번째 grid는 포함하면 안되므로 1빼줌=

        float yPos = heightGrid[currentIndex.y].transform.position.y;
        float xPos = columnSize * currentIndex.x + columnSize / 2;

        // 중앙 정렬을 위해 offset 적용
        Vector3 worldPos = new Vector3(xPos, yPos, 0);

        _previewLowNode.transform.position = worldPos;
        _previewLowNode.transform.localScale = new Vector3(
            previewLowNodePrefab.transform.localScale.x,
            rowSize * 0.5f,
            previewLowNodePrefab.transform.localScale.z);
    }

    public void CreateLowNode(Vector2Int currentIndex, string keySound = null)
    {
        if (currentIndex.x < 0 || currentIndex.y < 0 ||
        currentIndex.x >= _column || currentIndex.y >= heightGrid.Count)
        {
            // Debug.LogWarning("노드 생성 위치가 범위를 벗어났습니다.");
            return;
        }

        if (_nodeGrid[currentIndex.x, currentIndex.y] != null)
        {
            // Debug.LogWarning("이미 노드가 존재합니다.");
            return;
        }

        GameObject nodeObj = Instantiate(lowNodePrefab);
        LowNode node = nodeObj.AddComponent<LowNode>();

        if (node != null)
        {
            node.transform.SetParent(nodeParent, true);
            node.transform.localScale = lowNodePrefab.transform.localScale;
            // print("노드 생성 완료");

            float columnSize = _spriteRenderer.size.x / _column;
            float rowSize = _spriteRenderer.size.y / (heightGrid.Count - 1); //0번째 grid는 포함하면 안되므로 1빼줌

            float yPos = heightGrid[currentIndex.y].transform.position.y;
            float xPos = columnSize * currentIndex.x + columnSize / 2;

            // 중앙 정렬을 위해 offset 적용
            Vector3 worldPos = new Vector3(xPos, yPos, 0);

            //float yScale = lowNodePrefab.transform.localScale.y;
            node.transform.position = worldPos;
            node.transform.localScale = new Vector3(
                lowNodePrefab.transform.localScale.x,
                rowSize * 0.5f,
                lowNodePrefab.transform.localScale.z);

            print($"현재 columnSize : {rowSize}");
            //노드 위치 및 키음 초기화
            node.InitializeNode(currentIndex, keySound);

            _nodeGrid[currentIndex.x, currentIndex.y] = node as LowNode;

            HideLowNodePreview();
        }
    }

    public void RemoveNode(Vector2Int currentIndex)
    {
        if (_nodeGrid[currentIndex.x, currentIndex.y] == null)
        {
            Debug.LogWarning("제거할 노드가 없음");
            return;
        }

        if (_nodeGrid[currentIndex.x, currentIndex.y] is LowNode)
        {
            Destroy(_nodeGrid[currentIndex.x, currentIndex.y].gameObject);
            _nodeGrid[currentIndex.x, currentIndex.y] = null;
            print($"일반 노드 제거 완료 : {currentIndex}");
        }

        else if (_nodeGrid[currentIndex.x, currentIndex.y] is LongNode)
        {
            LongNode clickedNode = _nodeGrid[currentIndex.x, currentIndex.y] as LongNode;
            if (clickedNode == null) return;
            Vector2Int? startPos = null;
            foreach (var kvp in _longNodePosition)
            {
                if (currentIndex.x == kvp.Key.x &&
                    currentIndex.y >= kvp.Key.y &&
                    currentIndex.y <= kvp.Value.y)
                {
                    startPos = kvp.Key;
                    break;
                }
            }

            if (startPos.HasValue)
            {
                Vector2Int endPos = _longNodePosition[startPos.Value];
                // 그리드에서 노드 참조 제거
                for (int y = startPos.Value.y; y <= endPos.y; y++)
                {
                    _nodeGrid[startPos.Value.x, y] = null;
                }

                // 게임오브젝트 삭제
                Destroy(clickedNode.gameObject);
                // Dictionary에서 제거
                _longNodePosition.Remove(startPos.Value);
                print($"롱노드 제거 완료");
            }
        }
    }

    public bool _makingPreviewNode = false;
    public void CreatePreviewLongNode(Vector2Int start, Vector2Int end)
    {
        if (_previewLongNode == null)
        {
            _previewLongNode = Instantiate(previewLongNodePrefab, nodeParent);
            _previewLongNode.SetActive(false);
            print($"롱 노드 생성됨");
        }

        if (start.x < 0 || start.y < 0 || start.x >= _column || start.y >= heightGrid.Count)
        {
            return;
        }
        //_makingPreviewNode = false;

        //같은 행에서만 생성 가능
        if (start.x != end.x)
        {
            _previewLongNode.SetActive(false);
            return;
        }

        _makingPreviewNode = true;
        _previewLongNode.SetActive(true);

        LineRenderer lineRenderer = _previewLongNode.GetComponent<LineRenderer>();

        float columnSize = _spriteRenderer.size.x / _column;
        float rowSize = _spriteRenderer.size.y / (heightGrid.Count - 1);

        float xPos = columnSize * start.x + columnSize / 2;

        float startY = heightGrid[start.y].transform.position.y - ((float)cellHeight / 2);
        float endY = heightGrid[end.y].transform.position.y + ((float)cellHeight / 2);

        lineRenderer.SetPosition(0, new Vector3(xPos, startY, -0.01f));
        lineRenderer.SetPosition(1, new Vector3(xPos, endY, -0.01f));
        lineRenderer.startWidth = columnSize * 0.75f;
        lineRenderer.endWidth = columnSize * 0.75f;
        print($"롱노트 시작 점 : {start.y} ~ {end.y}");
    }

    private Dictionary<Vector2Int, Vector2Int> _longNodePosition = new Dictionary<Vector2Int, Vector2Int>();
    public void CreateLongNode(Vector2Int start, Vector2Int end, string keySound = null)
    {
        if (start.x != end.x || start.y >= end.y)
        {
            HideLongNodePreview();
            return;
        }

        _makingPreviewNode = false;
        int minY = Mathf.Min(start.y, end.y);
        int maxY = Mathf.Max(start.y, end.y);

        for (int y = minY; y <= maxY; y++)
        {
            if (_nodeGrid[start.x, y] != null)
            {
                Debug.LogWarning("이미 노드가 존재합니다");
                HideLongNodePreview();
                return;
            }
        }

        GameObject longNode = Instantiate(longNodePrefab, nodeParent);
        LongNode node = longNode.GetComponent<LongNode>();
        LineRenderer lineRenderer = longNode.GetComponent<LineRenderer>();

        //삭제시 dic으로 찾음
        _longNodePosition[start] = end;

        //;lineRenderer.material = _previewLongNode.GetComponent<LineRenderer>().material;
        //lineRenderer.startColor = Color.yellow;
        //lineRenderer.endColor = Color.cyan;

        float columnSize = _spriteRenderer.size.x / _column;
        float rowSize = _spriteRenderer.size.y / (heightGrid.Count - 1);

        float xPos = columnSize * start.x + columnSize / 2;

        float startY = heightGrid[start.y].transform.position.y - ((float)cellHeight / 2);
        float endY = heightGrid[end.y].transform.position.y + ((float)cellHeight / 2);

        lineRenderer.SetPosition(0, new Vector3(xPos, startY, -0.01f));
        lineRenderer.SetPosition(1, new Vector3(xPos, endY, -0.01f));
        lineRenderer.startWidth = columnSize * 0.75f;
        lineRenderer.endWidth = columnSize * 0.75f;
        lineRenderer.positionCount = 2;

        for (int y = minY; y <= maxY; y++)
        {
            _nodeGrid[start.x, y] = node as LongNode;
            print($"롱 노드 생성 y값 : {start.x} ~ {y}");
        }

        //노드 위치 및 키음 초기화
        node.InitializeLongNode(start, end, keySound);

        HideLongNodePreview();
    }

    public void HideLongNodePreview()
    {
        if (_previewLongNode != null)
        {
            _previewLongNode.SetActive(false);
        }
    }

    public void HideLowNodePreview()
    {
        if (_previewLowNode != null)
        {
            _previewLowNode.SetActive(false);
        }
    }

    public void Temp()
    {
        if (_nodeGrid == null)
        {
            Debug.LogWarning("NodeGrid에 암것도 없음");
            return;
        }
        print("=== 노드 정보 출력 시작 ===");
        for (int x = 0; x < _column; x++)
        {
            for (int y = 0; y < heightGrid.Count; y++)
            {
                if (_nodeGrid[x, y] != null)
                {
                    string nodeType = _nodeGrid[x, y] is LongNode ? "롱노드" : "일반노드";
                    Debug.Log($"위치 [{x}, {y}]: {nodeType}");
                }
            }
        }
        print("=== 노드 정보 출력 끝 ===");
    }

    public void CreateUpperGridMark(int grid, int index)
    {
        bool isLeft = false;

        if (!_upperGridMarks.ContainsKey(grid))
        {
            _upperGridMarks[grid] = new Dictionary<bool, GameObject>();
        }

        if (index >= 0 && index <= 3)
        {
            isLeft = true;
        }

        //이미 있다면 생성 안함
        if (_upperGridMarks.ContainsKey(grid) && _upperGridMarks[grid].ContainsKey(isLeft))
        {
            return;
        }

        float rowSize = _spriteRenderer.size.y / (heightGrid.Count - 1);

        float xPos = (isLeft == true) ? transform.position.x : transform.position.x + _spriteRenderer.size.x;
        float yPos = rowSize * grid;

        GameObject upperNodeObj = Instantiate(upperGridMarkPrefab);
        upperNodeObj.transform.position = new Vector3(xPos, yPos);

        if (isLeft == true)
        {
            upperNodeObj.transform.localScale = new Vector3(-upperGridMarkPrefab.transform.localScale.x, upperGridMarkPrefab.transform.localScale.y);
        }

        _upperGridMarks[grid][isLeft] = upperNodeObj;

    }

    public void RemoveUpperGridMark(int grid, int index)
    {
        bool isLeft = false;

        if (index >= 0 && index <= 3)
        {
            isLeft = true;
        }

        //grid에 없다면 리턴
        if (!_upperGridMarks.ContainsKey(grid))
        {
            return;
        }


        // 마커 제거
        if (_upperGridMarks[grid][isLeft] != null)
        {
            Destroy(_upperGridMarks[grid][isLeft]);
        }

        // Dictionary 정리
        _upperGridMarks[grid].Remove(isLeft);

        //if (!_upperGridMarks.ContainsKey(grid))
        //{
        //    _upperGridMarks[grid] = new Dictionary<bool, GameObject>();
        //}
    }

    public void ClearAllNodes()
    {
        // 기존 노드들 제거
        if (nodeParent != null)
        {
            foreach (Transform child in nodeParent)
            {
                Destroy(child.gameObject);
            }
        }

        _longNodePosition.Clear();
        // 그리드 라인들 제거
        heightGrid.ForEach(x => Destroy(x));
        widthGrid.ForEach(x => Destroy(x));
        heightGrid.Clear();
        widthGrid.Clear();

        _nodeGrid = null;
    }

    private bool IsPointerOverUI()
    {
        // UI 레이어 체크
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return _isMouseInUI;
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    print("마우스가 UI위에 있음");
    //    _isMouseInUI = true;
    //    HideLowNodePreview();
    //    HideLongNodePreview();
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    _isMouseInUI = false;
    //}
}
