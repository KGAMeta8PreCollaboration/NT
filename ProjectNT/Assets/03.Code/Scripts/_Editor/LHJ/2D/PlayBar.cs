using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBar : MonoBehaviour
{
    const float epsilon = 0.01f;

    private UpperNodeHandler _upperNodeTest;
    private NCT _nct;
    private float _yPos;

    private double _cellHeight = 0;

    private void Awake()
    {
        _nct = FindObjectOfType<NCT>();
        _upperNodeTest = FindObjectOfType<UpperNodeHandler>();
        _cellHeight = 0;
        _nct.callback += CellHeight;
    }

    private void Update()
    {
        if (_cellHeight <= 0) return;

        _yPos = transform.position.y;
        int currentGrid = Mathf.RoundToInt((float)(_yPos / _cellHeight));

        // 현재 그리드가 변경될 때만 SetText 호출
        if (_upperNodeTest._currentGridIndex != currentGrid)
        {
            _upperNodeTest.GetGridIndex(currentGrid);
        }
    }

    private void CellHeight(double cellHeight)
    {
        _cellHeight = cellHeight;
        print($"cellHeight : {_cellHeight}");
    }
}
