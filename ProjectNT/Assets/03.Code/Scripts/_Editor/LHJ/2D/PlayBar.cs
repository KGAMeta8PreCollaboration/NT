using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBar : MonoBehaviour
{
    const float epsilon = 0.01f;

    private UpperNodeTest _upperNodeTest;
    private NCT _nct;
    private float _yPos;

    private double _cellHeight = 0;

    private void Awake()
    {
        _nct = FindObjectOfType<NCT>();
        _upperNodeTest = FindObjectOfType<UpperNodeTest>();
        _cellHeight = 0;
        _nct.callback += CellHeight;
    }

    private void Update()
    {
        if (_cellHeight == 0)
        {
            return;
        }

        _yPos = transform.position.y;

        // 현재 위치를 cell 높이로 나눈 값
        double gridPosition = _yPos / _cellHeight;
        int currentGrid = Mathf.RoundToInt((float)gridPosition);

        // 가장 가까운 정수값과의 차이 계산
        double distanceToNearestGrid = Math.Abs(gridPosition - Math.Round(gridPosition));

        // 현재 그리드가 변경될 때만 SetText 호출
        if (distanceToNearestGrid < epsilon)
        {
            if (_upperNodeTest._currentGridIndex != currentGrid)
            {
                _upperNodeTest.SetText(currentGrid);
            }
        }
    }

    private void CellHeight(double cellHeight)
    {
        _cellHeight = cellHeight;
        print($"cellHeight : {_cellHeight}");
    }
}
