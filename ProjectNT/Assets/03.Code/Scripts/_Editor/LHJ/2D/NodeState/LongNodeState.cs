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
        if (EditorDataManager.Instance.CurKeySoundName == "")
        {
            // Debug.LogWarning("키음 없음");
            return;
        }

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
        //배치 상태가 아닐때
        if (position == new Vector2Int(-1, -1))
        {
            _nct.ChangeState(new LowNodeState(_nct));
            _nct.HideLongNodePreview();
            return;
        }

        if (_isPlacing == false)
        {
            _nct.HideLongNodePreview();
            //그리드에 노드가 있으면
            if (_nct.NodeGrid[position.x, position.y] != null)
            {
                //노드 제거
                _nct.RemoveNode(position);
                _nct.HideLongNodePreview();
            }
            //그리드에 노드가 없으면
            else
            {
                //상태 변경
                _nct.ChangeState(new LowNodeState(_nct));
                _nct.HideLongNodePreview();
            }
        }
        //배치 상태일때
        else
        {
            _isPlacing = false;
            _nct.HideLongNodePreview();
            return;
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
