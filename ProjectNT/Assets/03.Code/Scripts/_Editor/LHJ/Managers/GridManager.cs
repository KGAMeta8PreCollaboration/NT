using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("그리드를 그릴 오브젝트")]
    [SerializeField] private GameObject targetObject;
    [Header("가로 길이(클수록 가로로 길어짐)")]
    [SerializeField] private float widthScale = 1f;
    [Header("세로 길이(클수록 세로로 길어짐)")]
    [SerializeField] private float heightScale = 1f;
    [Header("Texture해상도")]
    [SerializeField] private float texturePerSecond = 2048f; // 텍스처 해상도 증가
    [Header("Grid 설정")]
    [SerializeField] private float bpm = 120;
    [SerializeField] private int beatNum = 4; //박자의 수
    //[SerializeField] private int subdivision = 4; // 박자
    [SerializeField] private int nodesPerBeat = 1; //비트당 노드 수
    private int row;    // 열(가로줄)
    [SerializeField] private int column = 4; // 행(세로줄)
    [SerializeField] private Color gridColor = Color.black;
    [SerializeField] private Color subGridColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // 서브그리드 색상
    [SerializeField] private Color backgroundColor = Color.white;
    [SerializeField] private float lineThickness = 2f;

    public float BPM => bpm;
    //public int Row => row;
    public int Column => column;
    public Vector2 CellSize => _cellSize;
    public Vector2[,] GridPoint => _gridPoint;
    public int TotalBeats => _totalBeats;
    public int BeatNum => beatNum;

    public Texture2D GridTexture => _gridTexture;
    public Action gridInfoCallback;

    private AudioSourceManager _audioSourceManager;
    private Texture2D _gridTexture;
    private Material _targetMaterial;
    private Vector2 _cellSize;
    private Vector2[,] _gridPoint;
    private int _totalBeats;
    private const float BASE_BPM = 120f; //기준이 되는 BPM
    private const int BASE_BEAT = 1; //기준이 되는 박자 수

    private void Awake()
    {
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => _audioSourceManager.AudioSource != null);
        InitGrid();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateGrid();
            gridInfoCallback?.Invoke();
        }
    }

    private void InitGrid()
    {
        if (targetObject != null)
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            _targetMaterial = new Material(renderer.material);

            CreateGridTexture();
            GenerateGrid();

            _targetMaterial.mainTexture = _gridTexture;
            renderer.material = _targetMaterial;

            float duration = _audioSourceManager.AudioDuration;
            float height = duration * heightScale;
            targetObject.transform.localScale = new Vector3(widthScale / 10f, 1, height / 10f);
        }
    }

    private void CreateGridTexture()
    {
        int duration = _audioSourceManager.AudioDuration;

        int height = duration * (int)texturePerSecond;

        if (height > AudioVisualizable.MAX_TEXTUREWIDTH)
        {
            float ratio = AudioVisualizable.MAX_TEXTUREWIDTH / duration;
            height = (int)(duration * ratio);
            Debug.LogWarning($"텍스처 크기가 최대 크기를 초과해서 높이 재설정 : {height} ");
        }

        int width = 2048; // 가로 해상도도 증가
        _gridTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        print($"TextureSize : {width} X {height}");
        _gridTexture.filterMode = FilterMode.Bilinear; // 선명한 텍스처를 위해 필터모드 설정
    }

    private void UpdateGrid()
    {
        if (heightScale > 0f && widthScale > 0f)
        {
            InitGrid();
        }
    }

    private void GenerateGrid()
    {
        // 배경 설정
        for (int y = 0; y < _gridTexture.height; y++)
        {
            for (int x = 0; x < _gridTexture.width; x++)
            {
                _gridTexture.SetPixel(x, y, backgroundColor);
            }
        }

        int songDuration = _audioSourceManager.AudioDuration;
        //초당 픽셀
        float pixelsPerSecond = _gridTexture.height / songDuration;
        //초당 bpm
        float secondsPerBeat = 60 / bpm;
        //bpm을 나눌 비트의 수
        int beat = (beatNum <= 1) ? BASE_BEAT : beatNum;
        //1비트 당 픽셀 -> cell의 높이
        float pixelsPerBeat = (pixelsPerSecond * secondsPerBeat);
        //cell의 넓이
        float columnWidth = _gridTexture.width / column;
        //전체 비트 수 
        _totalBeats = Mathf.CeilToInt(_gridTexture.height / pixelsPerBeat) * beat;
        _gridPoint = new Vector2[column, _totalBeats];
        print($"GridManager에 행과 열 개수 : {column} X {_totalBeats}");

        for (int c = 0; c < column; c++)
        {
            for (int b = 0; b < _totalBeats; b++)
            {
                //Cell의 중앙점 계산을 위해 0.5f 오프셋 추가
                float xPos = -5f + ((c * columnWidth) / _gridTexture.width * 10f) + (5f / column);
                //Grid 중앙에 위치하기 위해 뒤에 주석처리
                float zPos = -5f + ((b * pixelsPerBeat / beat) / _gridTexture.height * 10f)/* + (5f / _totalBeats)*/;

                _gridPoint[c, b] = new Vector2(xPos, zPos);
            }
        }

        for (int x = 0; x < column; x++)
        {
            //새로 선 그릴 포지션
            float xPos = x * columnWidth;
            DrawVerticalLine(xPos, gridColor);
        }

        for (float y = 0; y < _gridTexture.height; y += pixelsPerBeat)
        {
            DrawHorizontalLine(y, gridColor, false);

            if (beat > 1)
            {
                float subDivisionSpace = pixelsPerBeat / beat;
                for (int i = 1; i < beat; i++)
                {
                    float subY = y + (i * subDivisionSpace);
                    if (subY < _gridTexture.height)
                    {
                        DrawHorizontalLine(subY, subGridColor, true);
                    }
                }
            }
        }

        _gridTexture.Apply();
    }

    //새로선 그리는 함수
    private void DrawVerticalLine(float x, Color color)
    {
        for (int y = 0; y < _gridTexture.height; y++)
        {
            for (int t = 0; t < lineThickness; t++)
            {
                if (x + t < _gridTexture.width)
                {
                    _gridTexture.SetPixel((int)(x + t), y, color);
                }
            }
        }
    }

    //가로선 그리는 함수
    private void DrawHorizontalLine(float y, Color color, bool isSubGrid)
    {
        float line = isSubGrid? lineThickness / 2 : lineThickness;
        //lineThickness = (isSubGrid == true) ? lineThickness / 2 : lineThickness;
        for (int x = 0; x < _gridTexture.width; x++)
        {
            for (int t = 0; t < line; t++)
            {
                if (y + t < _gridTexture.height)
                {
                    _gridTexture.SetPixel(x, (int)(y + t), color);
                }
            }
        }
    }
}