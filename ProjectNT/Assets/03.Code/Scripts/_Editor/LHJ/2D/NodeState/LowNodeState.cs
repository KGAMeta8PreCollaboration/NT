using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowNodeState : INodeState
{
    private NCT _nct;

    public LowNodeState(NCT nct)
    {
        _nct=nct;
    }

    public void OnLeftClick(Vector2Int position)
    {
        _nct.CreateLowNode(position);
    }

    public void OnMiddleClick(Vector2Int position)
    {
        _nct.ChangeState(new LongNodeState(_nct));
        _nct.HideLowNodePreview();
    }

    public void OnRightClick(Vector2Int position)
    {
        _nct.RemoveLowNode(position);
    }

    public void UpdatePreview(Vector2Int position)
    {
        _nct.CreatePreviewLowNode(position);
    }

    public string GetStateName() => "하단 노트";
}
