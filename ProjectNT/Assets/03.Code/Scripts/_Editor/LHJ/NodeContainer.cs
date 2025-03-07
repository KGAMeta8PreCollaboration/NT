using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeContainer : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform nodeParent;

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
        (int column, int beatIndex) = GetGridPositionFromMouse();
        CreatePreviewNode(column, beatIndex);

        if (Input.GetMouseButtonDown(0))
        {
            PlaceNodeMousePosition();
        }
    }

    private void GridValueChanged()
    {
        InitializeNodeGrid();
    }

    //노드 이차원 배열 생성
    private void InitializeNodeGrid()
    {
        _texture = _gridManager.GridTexture;
        float songDuration = _audioSourceManager.AudioDuration;

        //비트당 초
        float secondsPerBeat = 60f / _gridManager.BPM;
        //초당 픽셀
        float pixelsPerSecond = _texture.height / songDuration;
        //비트당 픽셀
        float pixelsPerBeat = pixelsPerSecond * secondsPerBeat;

        //셀이 날라가면 안되니 올림
        _totalBeats = Mathf.CeilToInt(_texture.height / pixelsPerBeat);
         
        _nodeGrid = new Node[_gridManager.Row, _totalBeats];
        print($"그리드 생성 완료 : {_gridManager.Row} x {_totalBeats}");
    }

    private void PlaceNodeMousePosition()
    {
        (int column, int beatIndex) = GetGridPositionFromMouse();
        print($"행 : {column}, 열 : {beatIndex}");
        if (column >= 0 && column < 4 && beatIndex >= 0 && beatIndex < _totalBeats)
        {
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
            print($"로컬좌표 : {localHit}");

            //Plane의 로컬좌표계인 (-5, 5)를 (0, 10)으로 변환하기 위해 5를 더함
            float normalizedX = (localHit.x + 5f) / 10f * _gridManager.Row;
            int column = Mathf.Clamp(Mathf.FloorToInt(normalizedX), 0, _gridManager.Row - 1);

            float normalizedZ = (localHit.z + 5f) / 10f * _totalBeats;
            int beatIndex = Mathf.Clamp(Mathf.FloorToInt(normalizedZ), 0, _totalBeats - 1);

            return (column, beatIndex);
        }
        return (-1, -1);
    }

    //임시 노드
    private void CreatePreviewNode(int column, int beatIndex) 
    {
        if (column < 0 || beatIndex < 0 || column >= _gridManager.Row || beatIndex >= _totalBeats)
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

        float cellSizeX = 10f / _gridManager.Row;
        float cellSizeZ = 10f / _totalBeats;

        float xPos = -5f + (column * cellSizeX) + (cellSizeX / 2f);
        float zPos = -5f + (beatIndex * cellSizeZ) + (cellSizeZ / 2f);

        _previewNode.transform.position = nodeParent.TransformPoint(new Vector3(xPos, 0.1f, zPos));
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
            float cellSizeX = 10f / _gridManager.Row;
            float cellSizeZ = 10f / _totalBeats;

            //gridIndex를 실제 위치로 변환(0 ~ 10을 -5 ~ 5로)
            float xPos = -5f + (column * cellSizeX) + (cellSizeX / 2f);
            float zPos = -5f + (beatIndex * cellSizeZ) + (cellSizeZ / 2f);

            nodeObj.transform.position = nodeParent.TransformPoint(new Vector3(xPos, 0.1f, zPos));
            node.transform.SetParent(nodeParent, true);

            node.transform.localScale = nodeObj.transform.localScale;
            _nodeGrid[column, beatIndex] = node;
            node.Initialize(column, beatIndex * (60f / _gridManager.BPM));
        }
    }
}
