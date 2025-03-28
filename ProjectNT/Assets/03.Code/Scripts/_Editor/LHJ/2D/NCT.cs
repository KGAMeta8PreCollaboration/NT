using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

[RequireComponent(typeof(SpriteRenderer))]
public class NCT : MonoBehaviour
{
    [SerializeField] GameObject bpmLinePrefab;
    [SerializeField] GameObject beatLinePrefab;
    [SerializeField] GameObject columnLinePrefab;
    [SerializeField] GameObject previewNodePrefab;
    [SerializeField] GameObject nodePrefab;
    [SerializeField] Transform nodeParent;
    [SerializeField] int width = 128; //넓이
    [SerializeField] int pixelPerSecond = 100; //높이
    [SerializeField] Camera cam;

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

    private Node[,] _nodeGrid;
    private GameObject _previewNode;

    private float _bpm;
    private int _column;
    private int _beatNum;

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _gridManager.InitBeatMap += CreateNodeContainer;
    }

    Vector2Int currentIndex = new Vector2Int();
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateNode(GetGridPositionFromMouse());
        }

        if (Input.GetMouseButtonDown(1))
        {
            RemoveNode(GetGridPositionFromMouse());
        }

        CreatePreviewNode(GetGridPositionFromMouse());
    }

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
        print($"spriteRenderer.x : {_spriteRenderer.size.x}");
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

        _nodeGrid = new Node[_column, heightGrid.Count];
        cellHeight = (double)(heightGrid[1].transform.position.y - heightGrid[0].transform.position.y);
        //double temp = (double)(heightGrid[1].transform.position.y - heightGrid[0].transform.position.y);
        print($"한칸의 넓이 : {cellHeight}");
    }

    private Vector2Int GetGridPositionFromMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //NodeContainer가 SpriteRenderer로 생성되므로, 임시의 Plane을 생성해서 비교
        Plane plane = new Plane(Vector3.forward, transform.position);

        int column = -1;
        int row = -1;
        Vector2Int index = new Vector2Int(column, row);

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            if (_spriteRenderer.bounds.Contains(worldPoint))
            {
                //print($"worldPoint : {worldPoint}");

                //행
                column = (int)(worldPoint.x / (_spriteRenderer.size.x / _column));
                //열
                row = (int)(worldPoint.y / (_spriteRenderer.size.y / heightGrid.Count));

                index.x = column;
                index.y = row;
                index = new Vector2Int(column, row);
                return index;
            }
        }
        index = new Vector2Int(column, row);
        return index;

    }

    private void CreatePreviewNode(Vector2Int currentIndex)
    {
        if (currentIndex.x < 0 ||  currentIndex.y < 0 || currentIndex.x >= _column || currentIndex.y >= heightGrid.Count)
        {
            if (_previewNode != null)
            {
                Destroy(_previewNode);
                _previewNode = null;
            }
            return;
        }

        if (_nodeGrid[currentIndex.x, currentIndex.y] != null)
        {
            if (_previewNode != null)
            {
                Destroy(_previewNode);
                _previewNode = null;
            }
            return;
        }

        print($"현재 좌표 : {currentIndex.x} X {currentIndex.y}");
        if (_previewNode == null)
        {
            _previewNode = Instantiate(previewNodePrefab);
            _previewNode.transform.SetParent(nodeParent, true);
            _previewNode.transform.localScale = previewNodePrefab.transform.localScale;
        }

        float columnSize = _spriteRenderer.size.x / _column;
        float rowSize = _spriteRenderer.size.y / (heightGrid.Count - 1); //0번째 grid는 포함하면 안되므로 1빼줌

        float xPos = columnSize * currentIndex.x + columnSize / 2;  // 왼쪽 끝에서부터 시작
        float yPos = rowSize * currentIndex.y;     // 아래쪽 끝에서부터 시작

        // 중앙 정렬을 위해 offset 적용
        Vector3 worldPos = new Vector3(xPos, yPos, 0);

        _previewNode.transform.position = worldPos;
        _previewNode.transform.localScale = previewNodePrefab.transform.localScale;
    }

    private void CreateNode(Vector2Int currentIndex)
    {
        if (_nodeGrid[currentIndex.x, currentIndex.y] != null)
        {
            Debug.LogWarning("이미 노드가 존재합니다.");
            return;
        }

        GameObject nodeObj = Instantiate(nodePrefab);
        Node node = nodeObj.AddComponent<Node>();

        if (node != null)
        {
            node.transform.SetParent(nodeParent, true);
            node.transform.localScale = nodePrefab.transform.localScale;
            print("노드 생성 완료");

            float columnSize = _spriteRenderer.size.x / _column;
            float rowSize = _spriteRenderer.size.y / (heightGrid.Count - 1); //0번째 grid는 포함하면 안되므로 1빼줌

            float xPos = columnSize * currentIndex.x + columnSize / 2;  // 왼쪽 끝에서부터 시작
            float yPos = rowSize * currentIndex.y;     // 아래쪽 끝에서부터 시작

            // 중앙 정렬을 위해 offset 적용
            Vector3 worldPos = new Vector3(xPos, yPos, 0);

            node.transform.position = worldPos;
            node.transform.localScale = nodePrefab.transform.localScale;
            _nodeGrid[currentIndex.x, currentIndex.y] = node;
        }
    }

    private void RemoveNode(Vector2Int currentIndex)
    {
        if (_nodeGrid[currentIndex.x, currentIndex.y] == null)
        {
            Debug.LogWarning("제거할 노드가 없음");
            return;
        }

        Destroy(_nodeGrid[currentIndex.x, currentIndex.y].gameObject);
        _nodeGrid[currentIndex.x, currentIndex.y] = null;
    }
}
