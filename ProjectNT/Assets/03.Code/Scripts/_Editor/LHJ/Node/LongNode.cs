using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNode : Node
{
    private Vector2Int _endIndex;
    public Vector2Int StartIndex { get; private set; }
    public Vector2Int EndIndex { get; private set; }

    public void InitializeLongNode(Vector2Int start, Vector2Int end)
    {
        _index = start;
        _endIndex = end;
        _nodeType = EditorNoteType.LongNote;
        _keySound = EditorDataManager.Instance.CurKeySoundName;

        //프로퍼티 초기화
        StartIndex = start;
        EndIndex = end;
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
