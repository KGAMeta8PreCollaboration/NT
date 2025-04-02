using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNode : Node
{
    private Vector2Int _endIndex;

    public void InitializeLongNode(Vector2Int start, Vector2Int end)
    {
        _index = start;
        _endIndex = end;
        _nodeType = NodeType.LongNode;
        _keySound = EditorDataManager.Instance.CurKeySoundName;
    }

    public override NodeData GetNodeData()
    {
        return new NodeData
        {
            index = _index,
            endIndex = _endIndex,
            nodeType = _nodeType,
            keySound = _keySound
        };
    }
}
