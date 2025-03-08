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
    //[SerializeField] private int beatsPerBar = 4; // 마디당 박자의 수
    //[SerializeField] private int subdivision = 4; // 박자
    [SerializeField] private int nodesPerBeat = 1; //비트당 노드 수
    [SerializeField] private int row;    // 열(가로줄)
    [SerializeField] private int column = 4; // 행(세로줄)
    [SerializeField] private Color gridColor = Color.black;
    [SerializeField] private Color subGridColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // 서브그리드 색상
    [SerializeField] private Color backgroundColor = Color.white;
    [SerializeField] private float lineThickness = 2f;

    public float BPM => bpm;
    public int Row => row;
    public int Column => column;

    public Texture2D GridTexture => _gridTexture;
    public Action gridInfoCallback;

    private AudioSourceManager _audioSourceManager;
    private Texture2D _gridTexture;
    private Material _targetMaterial;
    private const float BASE_BPM = 120f; // 기준이 되는 BPM

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
        float duration = _audioSourceManager.AudioDuration;

        int height = Mathf.CeilToInt(duration * texturePerSecond);

        if (height > AudioVisualizable.MAX_TEXTUREWIDTH)
        {
            height = AudioVisualizable.MAX_TEXTUREWIDTH;
            Debug.LogWarning("텍스처 크기가 최대 크기를 초과");
        }

        int width = 2048; // 가로 해상도도 증가
        _gridTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
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

        float songDuration = _audioSourceManager.AudioDuration;
        //초당 픽셀
        float pixelsPerSecond = _gridTexture.height / songDuration;
        //초당 bpm
        float secondsPerBeat = 60 / bpm;
        //1비트 당 픽셀
        float pixelsPerBeat = pixelsPerSecond * secondsPerBeat;

        float columnWidth = _gridTexture.width / (float)column;
        for (int x = 0; x < column; x++)
        {
            //새로 선 그릴 포지션
            int xPos = Mathf.RoundToInt(x * columnWidth);
            DrawVerticalLine(xPos, gridColor);
        }

        for (float y = 0; y < _gridTexture.height; y += pixelsPerBeat)
        {
            DrawHorizontalLine(Mathf.FloorToInt(y), gridColor);
        }

        _gridTexture.Apply();
    }

    //새로선 그리는 함수
    private void DrawVerticalLine(int x, Color color)
    {
        for (int y = 0; y < _gridTexture.height; y++)
        {
            for (int t = 0; t < lineThickness; t++)
            {
                if (x + t < _gridTexture.width)
                {
                    _gridTexture.SetPixel(x + t, y, color);
                }
            }
        }
    }

    //가로선 그리는 함수
    private void DrawHorizontalLine(int y, Color color)
    {
        for (int x = 0; x < _gridTexture.width; x++)
        {
            for (int t = 0; t < lineThickness; t++)
            {
                if (y + t < _gridTexture.height)
                {
                    _gridTexture.SetPixel(x, y + t, color);
                }
            }
        }
    }

    //public void UpdateGridSettings(float newBpm, int newBeatsPerBar, int newSubdivision)
    //{
    //    bpm = newBpm;
    //    beatsPerBar = newBeatsPerBar;
    //    subdivision = newSubdivision;
    //    UpdateGrid();
    //}

    public void SetNodesPerBeat(int count)
    {
        nodesPerBeat = Mathf.Max(1, count);
        UpdateGrid();
    }
}