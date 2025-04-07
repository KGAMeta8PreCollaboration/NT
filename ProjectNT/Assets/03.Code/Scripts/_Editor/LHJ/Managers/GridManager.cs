using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("그리드를 그릴 오브젝트")]
    [SerializeField] private GameObject targetObject;
    [Header("가로 길이(클수록 가로로 길어짐)")]
    [SerializeField] private int widthScale = 1024;
    [Header("세로 길이(클수록 세로로 길어짐)")]
    [SerializeField] private int heightScale = 64;
    [Header("Texture해상도")]
    [SerializeField] private float texturePerSecond = 2048f; // 텍스처 해상도 증가
    //[Header("GridText를 넣어주세요")]
    //[SerializeField] private TextMeshProUGUI gridText;
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
    private BeatMapManager _beatMapManager;
    private Texture2D _gridTexture;
    private Material _targetMaterial;
    private Vector2 _cellSize;
    private Vector2[,] _gridPoint;
    private int _totalBeats;
    private const float BASE_BPM = 120f; //기준이 되는 BPM
    private const int BASE_BEAT = 1; //기준이 되는 박자 수
    private AudioSource _audioSource;

    private void Awake()
    {
        _beatMapManager = FindObjectOfType<BeatMapManager>();
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
    }

    //private IEnumerator Start()
    //{
    //    yield return new WaitUntil(() => _beatMapManager.isLoaded == true && _audioSourceManager.AudioSource.clip != null);
    //    //InitGrid();
    //}

    // private void OnValidate()
    // {
    //     if (Application.isPlaying)
    //     {
    //         UpdateGrid();
    //         gridInfoCallback?.Invoke();
    //     }
    // }

    //불러오기 기능일때만 시작
    public void InitializeFromBeatMapManager(GridSetting gridSetting)
    {
        // bpm = gridSetting.BPM;
        bpm = EditorDataManager.Instance.ProjectData.bpm; //나중에 이걸로 꼭 바꿔야함
        column = 4;
        beatNum = EditorDataManager.Instance.ProjectData.beatNum;
        print($"bpm : {bpm}, beatNum : {beatNum}");
        //CreateGrid();
        //gridText.text = $"BPM : ({bpm})";
        CreateNodeContainer(_audioSourceManager.AudioSource);
    }

    public Action<float, int, int> InitBeatMap;
    public void CreateNodeContainer(AudioSource audioSource)
    {
        InitBeatMap?.Invoke(bpm, column, beatNum);
        print(4);
        //if (targetObject == null)
        //{
        //    Debug.LogWarning("그리드를 그릴 오브젝트가 없습니다.");
        //    return;
        //}
        _audioSource = audioSource;

        //int width = 64;
        //int height = Mathf.CeilToInt(_audioSource.clip.length) * 100;
        //Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        ////Texture2D texture = GetTexture();

        //Rect rect = new Rect(Vector2.zero, new Vector2(width, height));
        //SpriteRenderer spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = Sprite.Create(texture, rect, Vector2.zero);
        //Renderer renderer = targetObject.GetComponent<Renderer>();
        //_targetMaterial = new Material(renderer.material);
        //CreateGridTexture();
        //GenerateGrid();

        //_targetMaterial.mainTexture = _gridTexture;
        //renderer.material = _targetMaterial;

        //float duration = _audioSourceManager.AudioDuration;
        //float height = duration * heightScale;
        //targetObject.transform.localScale = new Vector3(widthScale / 10f, 1, height / 10f);

    }

    private void CreateGridTexture()
    {
        float duration = _audioSourceManager.AudioDuration;

        //높이는 올림으로 관리
        int height = Mathf.CeilToInt(duration * texturePerSecond);

        if (height > AudioVisualizable.MAX_TEXTUREWIDTH)
        {
            float ratio = AudioVisualizable.MAX_TEXTUREWIDTH / duration;
            height = Mathf.CeilToInt(duration * ratio);
            Debug.LogWarning($"텍스처 크기가 최대 크기를 초과해서 높이 재설정 : {height} ");
        }

        int width = 2048; // 가로 해상도도 증가
        _gridTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        print($"TextureSize : {width} X {height}");
        _gridTexture.filterMode = FilterMode.Bilinear; // 선명한 텍스처를 위해 필터모드 설정
    }

    //private void UpdateGrid()
    //{
    //    if (heightScale > 0f && widthScale > 0f)
    //    {
    //        CreateGrid();
    //    }
    //}

    private Texture2D GetTexture()
    {
        widthScale = Mathf.CeilToInt(_audioSource.clip.length) * 100;
        Texture2D texture = new Texture2D(widthScale, heightScale, TextureFormat.RGBA32, false);
        texture.Apply();
        return texture;
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
        print($"초당 픽셀 : {pixelsPerSecond}");
        //비트 당 초
        float secondsPerBeat = 60 / bpm;
        print($"초당bpm : {secondsPerBeat}");
        //bpm을 나눌 비트의 수
        int beat = (beatNum <= 1) ? BASE_BEAT : beatNum;
        //1비트 당 픽셀 -> cell의 높이
        float pixelsPerBeat = (pixelsPerSecond * secondsPerBeat);
        print($"float일때 pixelsPerBeat : {pixelsPerBeat}");
        //cell의 넓이
        float columnWidth = _gridTexture.width / column;
        print($"셀 하나의 사이즈 : {columnWidth} X {pixelsPerBeat}");

        //전체 비트 수 
        _totalBeats = Mathf.CeilToInt(_gridTexture.height / (float)pixelsPerBeat) * beat;

        _gridPoint = new Vector2[column, _totalBeats];

        for (int c = 0; c < column; c++)
        {
            for (int b = 0; b < _totalBeats; b++)
            {
                //Cell의 중앙점 계산을 위해 0.5f 오프셋 추가
                float xPos = -5f + ((c * columnWidth) / _gridTexture.width * 10f) + (5f / column);
                //Grid 중앙에 위치하기 위해 뒤에 주석처리
                float zPos = -5f + ((b * pixelsPerBeat / beat) / _gridTexture.height * 10f)/*+ (5f / _totalBeats)*/;

                _gridPoint[c, b] = new Vector2(xPos, zPos);
            }
        }

        for (int x = 0; x < column; x++)
        {
            //새로 선 그릴 포지션
            float xPos = x * columnWidth;
            DrawVerticalLine(xPos, gridColor);
        }

        for (float y = 0; y <= _gridTexture.height; y += pixelsPerBeat)
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
        float line = isSubGrid ? lineThickness / 2 : lineThickness;
        float halfThickness = line / 2;
        float startY = y - halfThickness;
        float endY = y + halfThickness;

        for (float i = startY; i <= endY; i += 0.5f)
        {
            int pixelY = Mathf.RoundToInt(i);
            if (pixelY >= 0 && pixelY < _gridTexture.height)
            {
                for (int x = 0; x < _gridTexture.width; x++)
                {
                    _gridTexture.SetPixel(x, pixelY, color);
                }
            }
        }
    }
}