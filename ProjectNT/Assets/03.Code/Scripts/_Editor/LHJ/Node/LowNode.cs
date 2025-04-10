using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowNode : Node
{
    public override void InitializeNode(Vector2Int index, string keySound)
    {
        base.InitializeNode(index, keySound);
        _nodeType = EditorNoteType.ShortNote;
        print($"키음 : {_keySound}");
    }


    //이 아래 두개는 안씀
    //============================================================
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
