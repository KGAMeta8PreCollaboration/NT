using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperNodeState : INodeState
{
    private NCT _nct;
    
    public UpperNodeState(NCT nct)
    {
        _nct = nct;
    }

    public string GetStateName()
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftClick(Vector2Int position)
    {
        throw new System.NotImplementedException();
    }

    public void OnMiddleClick(Vector2Int position)
    {
        throw new System.NotImplementedException();
    }

    public void OnRightClick(Vector2Int position)
    {
        throw new System.NotImplementedException();
    }

    public void UpdatePreview(Vector2Int position)
    {
        throw new System.NotImplementedException();
    }
}
