using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Vector2Int _index;
    private NodeType _nodeType;
    private NodeInstrument _nodeInstrument;
    private string _keySound;
    private TestNodeInfo _testNodeInfo;

    private void Awake()
    {
        _testNodeInfo = FindObjectOfType<TestNodeInfo>();
    }

    public void Initialize(Vector2Int index)
    {

    }

    private void ChangeNodeType(string keySound)
    {
        _keySound = keySound;
    }

    public void InitializeFromData(NodeData data)
    {
        _index = data.index;
        _nodeType = data.nodeType;
        _nodeInstrument = data.nodeInstrument;
        _keySound = data.keySound;
    }
    
    public void TestPrint()
    {
        _keySound = _testNodeInfo.CurrentNodeInfo;
        print($"현재 노드의 정보 : {_keySound}");
    }
}
