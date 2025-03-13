using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Vector2Int _index;
    private NodeType _nodeType;
    private NodeInstrument _nodeInstrument;
    private string _keySound;
    private Vector3 _nodeColor;
    private TestNodeInfo _testNodeInfo;

    private void Awake()
    {
        _testNodeInfo = FindObjectOfType<TestNodeInfo>();
    }

    private void ChangeNodeType(string keySound)
    {
        _keySound = keySound;
    }

    public void InitializeFromData(NodeData data)
    {

    }
    
    public void TestPrint()
    {
        _keySound = _testNodeInfo.CurrentNodeInfo;
        print($"현재 노드의 정보 : {_keySound}");
    }
}
