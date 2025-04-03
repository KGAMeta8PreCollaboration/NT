using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNodeState : INodeState
{
    private NCT _nct;
    //false -> 임시 노드를 안만드는 상태, true -> 임시 노드를 만드는 상태
    private bool _isPlacing = false;
    private Vector2Int startPos;

    public LongNodeState(NCT nct)
    {
        _nct = nct;
    }

    //_isPlacing
    public void OnLeftClick(Vector2Int position)
    {
        if (_isPlacing == false)
        {
            startPos = position;
            _isPlacing = true;
        }
        else
        {
            _nct.CreateLongNode(startPos, position);
            _isPlacing = false;
        }
    }

    public void OnMiddleClick(Vector2Int position)
    {
        if (_isPlacing)
        {
            _nct.CreateLongNode(startPos, position);
            _isPlacing = false;
        }
        //else
        //{
        //    _isPlacing = false;
        //    _nct.HideLongNodePreview();
        //}
    }

    //롱노트는 오른쪽 클릭일때 하단노드 상태로 돌아감
    public void OnRightClick(Vector2Int position)
    {
        if (_isPlacing == false)
        {
            if (_nct._nodeGrid[position.x, position.y] is LongNode)
            {
                _nct.RemoveLongNode(position);
            }
            else
            {
                _nct.ChangeState(new LowNodeState(_nct));
                _nct.HideLongNodePreview();
            }
        }
        else
        {
            _isPlacing = false;
            _nct.HideLongNodePreview();
        }
    }

    public void UpdatePreview(Vector2Int position)
    {
        if (_isPlacing)
        {
            _nct.CreatePreviewLongNode(startPos, position);
        }
    }
    public string GetStateName() => "롱 노트";
}
