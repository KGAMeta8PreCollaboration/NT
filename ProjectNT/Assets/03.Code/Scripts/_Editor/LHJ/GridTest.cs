using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int column;
    [SerializeField] private int textureSize = 512;
    [SerializeField] private Color gridColor = Color.black;
    [SerializeField] private Color backgroundColor = Color.white;
    [SerializeField] private float lineThickness = 2f;

    private Action<int, int> _gridChange;

    private void Start()
    {
        UpdateGrid();
    }

    //값이 변할 때 마다 실행
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateGrid();
        }
    }

    private Texture2D InitGrid()
    {
        Texture2D texture = new Texture2D(textureSize, textureSize);
        
        //배경 설정 -> 이건 없어도 될 듯
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                texture.SetPixel(x, y, backgroundColor);
            }
        }

        for (int y = 0; y < textureSize; y++)
        {
            for (int x= 0; x < textureSize; x++)
            {
                int cellWidth = textureSize / column;
                int cellHeight = textureSize / row;

                bool isGridLine = (x % cellWidth < lineThickness || y % cellHeight < lineThickness);

                if (isGridLine)
                {
                    texture.SetPixel(x, y, gridColor);
                }
            }
        }

        texture.Apply();
        return texture;
    }

    private void UpdateGrid()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.mainTexture = InitGrid();
        }
    }

    //public void AddListener(Action<int, int> callback)
    //{
    //    _gridChange += callback;
    //}

    //public void RemoveListener(Action<int, int> callback)
    //{
    //    _gridChange -= callback;
    //}
}
