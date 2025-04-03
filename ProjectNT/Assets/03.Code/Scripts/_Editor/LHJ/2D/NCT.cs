using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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

    public double cellHeight = 0;

    private GridManager _gridManager;
    private AudioSourceManager _audioSourceManager;
    private SpriteRenderer _spriteRenderer;
    private Texture2D _texture;
    private AudioSource _audioSource;

    private float xOffset;
    private float bpmLineScale;
    private float beatLineScale;
    private float columnLineScale;

    private List<GameObject> heightGrid = new List<GameObject>();
    private List<GameObject> widthGrid = new List<GameObject>();

    public Node[,] _nodeGrid;

    private GameObject _previewLowNode;
    private GameObject _previewLongNode;

    private float _bpm;
    private int _column;
    private int _beatNum;

    private INodeState _currentState;

    private Plane tempPlane = new Plane();

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _gridManager.InitBeatMap += CreateNodeContainer;

        _currentState = new LowNodeState(this);
        UpdateStateText();
    }

    Vector2Int currentIndex = new Vector2Int();
    private void Update()
    {
        currentIndex = GetGridPositionFromMouse(); 

        if (Input.GetMouseButtonDown(0))
            _currentState.OnLeftClick(currentIndex);
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

    public Action<double> callback;
    private void CreateNodeContainer(float bpm, int column, int beatNum)
    {
        if (bpm == 0)
        {
            Debug.LogWarning("BPM이 0입니다.");
            return;
        }

        _bpm = bpm;
        _column = column;
        _beatNum = beatNum;
        print($"bpm : {_bpm}, column : {_column}, beatNum : {_beatNum}");

        _audioSource = _audioSourceManager.AudioSource;

        width = 128;
        int height = Mathf.CeilToInt(_audioSource.clip.length) * pixelPerSecond;
        print(_audioSource.clip.length);
        _texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        Rect rect = new Rect(Vector2.zero, new Vector2(width, height));
        _spriteRenderer.sprite = Sprite.Create(_texture, rect, Vector2.zero);
        print($"그리드 생성");
        xOffset = _spriteRenderer.size.x / 2;
        bpmLineScale = _spriteRenderer.size.x * 1.2f;
        beatLineScale = _spriteRenderer.size.x;

        //테스트 용도
        float temp1 = _spriteRenderer.sprite.texture.width;
        float temp2 = _spriteRenderer.sprite.texture.height;

        print($"tmp1 : {temp1}, temp2 : {temp2}");

        //노래의 너비 = 텍스쳐 높이
        float songDuration = _audioSource.clip.length;
        float heightPerSecond = _spriteRenderer.size.y / songDuration;
        float secondsPerBPM = 60 / bpm;

        float bpmHeight = secondsPerBPM * heightPerSecond;
        print("=====================================\n" +
            $"_spriteRenderer.size.y  : {_spriteRenderer.size.y}\n" +
            $"songDuration : {songDuration}\n" +
            $"heightPerSecond : {heightPerSecond}\n" + 
            $"secondsPerBeat : {secondsPerBPM}\n"+
            $"beatHeight : {bpmHeight}\n");

        for (int i = 0; i * bpmHeight < _spriteRenderer.size.y; i ++)
        {
            float yPos = i * bpmHeight;
            GameObject bpmLineObj = Instantiate(bpmLinePrefab, new Vector3(xOffset, yPos, 0), Quaternion.identity);
            bpmLineObj.transform.localScale = new Vector3(bpmLineScale, bpmLinePrefab.transform.localScale.y);
            bpmLineObj.transform.SetParent(transform);
            BPMLine bpmLine = bpmLineObj.GetComponent<BPMLine>();
            bpmLine.SetBPMText(i, secondsPerBPM);

            //가로 grid에 BPM 추가
            heightGrid.Add(bpmLineObj);
            //bpmLine.Test(heightGrid.Count);

            if (beatNum != 0)
            {
                float beatHeight = bpmHeight / beatNum;

                for (int j = 1; j < beatNum; j++)
                {
                    float y = yPos + j * beatHeight;
                    if (y >= _spriteRenderer.size.y)
                    {
                        break;
                    }
                    GameObject beatLine = Instantiate(beatLinePrefab, new Vector3(xOffset, y, 0), Quaternion.identity);
                    beatLine.transform.localScale = new Vector3(beatLineScale, beatLinePrefab.transform.localScale.y);
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

        _nodeGrid = new LowNode[_column, heightGrid.Count];
        cellHeight = (double)(heightGrid[1].transform.position.y - heightGrid[0].transform.position.y);

        //NodeContainer가 SpriteRenderer로 생성되므로, 임시의 Plane을 생성해서 비교
        tempPlane = new Plane(Vector3.forward, transform.position);

        callback?.Invoke(cellHeight);
        //double temp = (double)(heightGrid[1].transform.position.y - heightGrid[0].transform.position.y);
        print($"한칸의 넓이 : {cellHeight}");
    }

    private Vector2Int GetGridPositionFromMouse()
    {
        Vector2Int index = new Vector2Int(-1, -1);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (tempPlane.Raycast(ray, out distance) == false)
        {
            print($"플레인 밖이다 : {index}");
            return index;
        }

        Vector3 worldPoint = ray.GetPoint(distance);
        if (_spriteRenderer.bounds.Contains(worldPoint) == false)
        {
            print($"플레인 밖이다 : {index}");
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
            print($"하단 노드 생성됨");
        }

        if (currentIndex.x < 0 ||  currentIndex.y < 0 || currentIndex.x >= _column || currentIndex.y >= heightGrid.Count)
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
        print($"현재 좌표 : {currentIndex.x} X {currentIndex.y}");

        float columnSize = _spriteRenderer.size.x / _column;
        float rowSize = _spriteRenderer.size.y / (heightGrid.Count - 1); //0번째 grid는 포함하면 안되므로 1빼줌

        float xPos = columnSize * currentIndex.x + columnSize / 2;  // 왼쪽 끝에서부터 시작
        float yPos = rowSize * currentIndex.y;     // 아래쪽 끝에서부터 시작

        // 중앙 정렬을 위해 offset 적용
        Vector3 worldPos = new Vector3(xPos, yPos, 0);

        _previewLowNode.transform.position = worldPos;
        //_previewLowNode.transform.localScale = previewLowNodePrefab.transform.localScale;
    }

    public void CreateLowNode(Vector2Int currentIndex)
    {
        if (currentIndex.x < 0 || currentIndex.y < 0 ||
        currentIndex.x >= _column || currentIndex.y >= heightGrid.Count)
        {
            Debug.LogWarning("노드 생성 위치가 범위를 벗어났습니다.");
            return;
        }

        if (_nodeGrid[currentIndex.x, currentIndex.y] != null)
        {
            Debug.LogWarning("이미 노드가 존재합니다.");
            return;
        }

        GameObject nodeObj = Instantiate(lowNodePrefab);
        LowNode node = nodeObj.AddComponent<LowNode>();

        if (node != null)
        {
            node.transform.SetParent(nodeParent, true);
            node.transform.localScale = lowNodePrefab.transform.localScale;
            print("노드 생성 완료");

            float columnSize = _spriteRenderer.size.x / _column;
            float rowSize = _spriteRenderer.size.y / (heightGrid.Count - 1); //0번째 grid는 포함하면 안되므로 1빼줌

            float xPos = columnSize * currentIndex.x + columnSize / 2;  // 왼쪽 끝에서부터 시작
            float yPos = rowSize * currentIndex.y;     // 아래쪽 끝에서부터 시작

            // 중앙 정렬을 위해 offset 적용
            Vector3 worldPos = new Vector3(xPos, yPos, 0);

            node.transform.position = worldPos;
            node.transform.localScale = lowNodePrefab.transform.localScale;

            //노드 위치 및 키음 초기화
            node.InitializeNode(currentIndex);

            _nodeGrid[currentIndex.x, currentIndex.y] = node as LowNode;

            HideLowNodePreview();
        }
    }

    public void RemoveLowNode(Vector2Int currentIndex)
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
    }

    public void RemoveLongNode(Vector2Int currentIndex)
    {
        if (_nodeGrid[currentIndex.x, currentIndex.y] == null)
        {
            Debug.LogWarning("제거할 노드가 없음");
            return;
        }

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

        if (start.x < 0 ||  start.y < 0 || start.x >= _column || start.y >= heightGrid.Count)
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
        float startY = rowSize * start.y - (rowSize / 2);
        float endY = rowSize * end.y + (rowSize / 2);

        lineRenderer.SetPosition(0, new Vector3(xPos, startY, -0.01f));
        lineRenderer.SetPosition(1, new Vector3(xPos, endY, -0.01f));
        lineRenderer.startWidth = columnSize * 0.75f;
        lineRenderer.endWidth = columnSize * 0.75f;
        print($"롱노트 시작 점 : {start.y} ~ {end.y}");
    }

    private Dictionary<Vector2Int, Vector2Int> _longNodePosition = new Dictionary<Vector2Int, Vector2Int>();
    public void CreateLongNode(Vector2Int start, Vector2Int end)
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

        for (int y = minY; y <= maxY; y++)
        {
            _nodeGrid[start.x, y] = node as Node;
            print($"롱 노드 생성 y값 : {start.x} ~ {y}");
        }
        //lineRenderer.material = _previewLongNode.GetComponent<LineRenderer>().material;
        //lineRenderer.startColor = Color.yellow;
        //lineRenderer.endColor = Color.cyan;

        float columnSize = _spriteRenderer.size.x / _column;
        float rowSize = _spriteRenderer.size.y / (heightGrid.Count - 1);

        float xPos = columnSize * start.x + columnSize / 2;
        float startY = rowSize * start.y - (rowSize / 2);
        float endY = rowSize * end.y + (rowSize / 2);

        lineRenderer.SetPosition(0, new Vector3(xPos, startY, -0.01f));
        lineRenderer.SetPosition(1, new Vector3(xPos, endY, -0.01f));
        lineRenderer.startWidth = columnSize * 0.75f;
        lineRenderer.endWidth = columnSize * 0.75f;
        lineRenderer.positionCount = 2;

        for (int y = minY; y <= maxY; y++)
        {
            _nodeGrid[start.x, y] = node as Node;
        }

        //노드 위치 및 키음 초기화
        node.InitializeLongNode(start, end);

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
}
