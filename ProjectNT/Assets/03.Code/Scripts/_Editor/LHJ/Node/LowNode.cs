using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowNode : Node
{
    public override void InitializeNode(Vector2Int index)
    {
        base.InitializeNode(index);
        _nodeType = EditorNoteType.ShortNote;
    }

    //저장할때 사용
    public override NodeData GetNodeData()
    {
        return new NodeData
        {
            index = _index,
            nodeType = _nodeType,
            keySound = _keySound
        };
    }

    //로드할때 사용
    public void SetNodeData(NodeData nodeData)
    {
        _index = nodeData.index;
        _nodeType = nodeData.nodeType;
        _keySound = nodeData.keySound;
    }
}
