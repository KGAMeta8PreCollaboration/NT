using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Vector2Int _index;
    //노드 타입은 닷, 롱, 상단 노트 이렇게 3가지
    private NodeType _nodeType;
    //keySound에 악기 정보 및 키음이 들어있음
    private string _keySound;
    private TestNodeInfo _testNodeInfo;

    private void Awake()
    {
        _testNodeInfo = FindObjectOfType<TestNodeInfo>();
    }

    public void InitializeNode(Vector2Int index)
    {
        _index = index;
        //현재 키음 정보를 담아줌
        _keySound = EditorDataManager.Instance.CurKeySoundName;
    }

    private void ChangeNodeType(string keySound)
    {
        _keySound = keySound;
    }

    //저장할때 사용
    public NodeData GetNodeData()
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

    public void InitializeFromData(NodeData data)
    {
        _index = data.index;
        _nodeType = data.nodeType;
        _keySound = data.keySound;
    }

    public void TestPrint()
    {
        _keySound = _testNodeInfo.CurrentNodeInfo;
        print($"현재 노드의 정보 : {_keySound}");
    }
}
