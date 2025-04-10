using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour
{
    protected Vector2Int _index;
    protected EditorNoteType _nodeType;
    public string _keySound;

    public virtual void InitializeNode(Vector2Int index, string keySound = null)
    {
        _index = index;
        _keySound = keySound ?? EditorDataManager.Instance.CurKeySoundName;
    }

    public abstract NodeData GetNodeData();
}
