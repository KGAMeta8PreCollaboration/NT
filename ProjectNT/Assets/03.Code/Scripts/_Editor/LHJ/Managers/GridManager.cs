using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("그리드를 그릴 오브젝트")]
    [SerializeField] private GameObject targetObject;
    [Header("가로 길이(클수록 가로로 길어짐)")]
    [SerializeField] private float widthScale = 1f;
    [Header("새로 길이(클수록 새로로 길어짐)")]
    [SerializeField] private float heightScale = 1f;
    [Header("Texture해상도")]
    [SerializeField] private float texturePerSecond;
    [SerializeField] private int row;    //열(가로줄)
    [SerializeField] private int column = 4; //행(새로줄)
    [SerializeField] private Color gridColor = Color.black;
    [SerializeField] private Color backgroundColor = Color.white;
    [SerializeField] private float lineThickness = 2f;

    private AudioSourceManager _audioSourceManager;
    private Texture2D _gridTexture;
    private Material _targetMaterial;


    private void Awake()
    {
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
    }

    private void Start()
    {
        InitGrid();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateGrid();
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
            float hight = duration * heightScale;

            targetObject.transform.localScale = new Vector3(widthScale / 10f, 1, hight / 10f);
        }
    }

    //텍스쳐 크기 결정
    private void CreateGridTexture()
    {
        float duration = _audioSourceManager.AudioDuration;
        int height = (int)(duration * texturePerSecond);
        if (height > AudioVisualizable.MAX_TEXTUREWIDTH)
        {
            int maxSample = AudioVisualizable.MAX_TEXTUREWIDTH / (int)duration;
            height = (int)(duration * maxSample);
            print($"heightPerSecond의 최대값 : {maxSample}");
        }
        int width = 256;
        _gridTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
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
        //배경 설정
        for (int y = 0; y < _gridTexture.height; y++)
        {
            for (int x = 0; x < _gridTexture.width; x++)
            {
                _gridTexture.SetPixel(x, y, backgroundColor);
            }
        }

        int cellWidth = _gridTexture.height / row;
        int cellHeight = _gridTexture.width / column;

        for (int y = 0; y < _gridTexture.height; y++)
        {
            for (int x = 0; x < _gridTexture.width; x++)
            {
                if (x == cellWidth || y == cellHeight)
                {
                    _gridTexture.SetPixel(x, y, gridColor);
                }
                //int cellWidth = _gridTexture.height / row;
                //int cellHeight = _gridTexture.width / column;

                //bool isGridLine = (x % cellWidth < lineThickness || y % cellHeight < lineThickness);

                //if (isGridLine)
                //{
                //    _gridTexture.SetPixel(x, y, gridColor);
                //}
            }
        }

        _gridTexture.Apply();
    }
}
