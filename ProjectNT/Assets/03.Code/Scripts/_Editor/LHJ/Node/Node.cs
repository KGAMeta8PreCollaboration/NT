using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Node : MonoBehaviour
{
    protected Vector2Int _index;
    protected EditorNoteType _nodeType;
    public string _keySound;

    protected TextMeshPro _keySoundText;

    public virtual void InitializeNode(Vector2Int index, string keySound = null)
    {
        _keySoundText = GetComponentInChildren<TextMeshPro>();
        _index = index;
        //키음이 없다면 배정이 안된것 이므로 현재 눌려있는 키음으로 배정
        _keySound = keySound ?? EditorDataManager.Instance.CurKeySoundName;
    }

    public abstract NodeData GetNodeData();
}
