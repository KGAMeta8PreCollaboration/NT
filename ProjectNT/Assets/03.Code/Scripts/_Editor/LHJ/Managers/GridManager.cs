using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("그리드를 그릴 오브젝트")]
    [SerializeField] private GameObject targetObject;
    [Header("AudioSourceManager를 참조해주세요")]
    [SerializeField] private AudioSourceManager audioSourceManager;
    [Header("가로 길이(클수록 가로로 길어짐)")]
    [SerializeField] private float heightScale = 1f;
    [Header("새로 길이(클수록 새로로 길어짐)")]
    [SerializeField] private float widthScale = 1f;
    [SerializeField] private int row;    //열(가로줄)
    [SerializeField] private int column = 4; //행(새로줄)
    [SerializeField] private Color gridColor = Color.black;
    [SerializeField] private Color backgroundColor = Color.white;
    [SerializeField] private float lineThickness = 2f;

    private Texture2D _gridTexture;
    private Material _targetMaterial;
    private int _textureWidth;
    private int _textureHeight;

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

            _targetMaterial.mainTexture = _gridTexture;
            renderer.material = _targetMaterial;

            float duration = audioSourceManager.AudioDuration;
            float width = duration * widthScale;

            targetObject.transform.localScale = new Vector3(width / 10f, 1, heightScale / 10f);
        }
    }

    //텍스쳐 크기 결정
    private void CreateGridTexture()
    {
        float duration = audioSourceManager.AudioDuration;
        _textureWidth = (int)duration;
        _textureHeight = 500; //임시 숫자
        _gridTexture = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false);
    }

    private void UpdateGrid()
    {
        if (heightScale > 0f && widthScale > 0f)
        {
            InitGrid();
        }
    }

    //private Texture2D InitGrid()
    //{
    //    textureWidth = (int)audioSourceManager.AudioDuration; //-> 이게 오차가 조금 있을 수도 있음
    //    Texture2D texture = new Texture2D(textureWidth, textureHeight);

    //    //배경 설정
    //    for (int y = 0; y < textureHeight; y++)
    //    {
    //        for (int x = 0; x < textureWidth; x++)
    //        {
    //            texture.SetPixel(x, y, backgroundColor);
    //        }
    //    }

    //    for (int y = 0; y < textureHeight; y++)
    //    {
    //        for (int x = 0; x < textureWidth; x++)
    //        {
    //            int cellWidth = textureHeight / column;
    //            int cellHeight = textureWidth / row;

    //            bool isGridLine = (x % cellWidth < lineThickness || y % cellHeight < lineThickness);

    //            if (isGridLine)
    //            {
    //                texture.SetPixel(x, y, gridColor);
    //            }
    //        }
    //    }

    //    texture.Apply();
    //    return texture;
    //}
}
