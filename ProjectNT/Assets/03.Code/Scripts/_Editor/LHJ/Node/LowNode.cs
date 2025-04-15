using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LowNode : Node
{
    public override void InitializeNode(Vector2Int index, string keySound)
    {
        base.InitializeNode(index, keySound);
        _nodeType = EditorNoteType.ShortNote;
        print($"키음 : {_keySound}");

        UpdateKeySoundTextSize();
    }

    private void UpdateKeySoundTextSize()
    {
        Vector3 nodeScale = transform.lossyScale;

        float inverseX = 1f / nodeScale.x;
        float inverseY = 1f / nodeScale.y;
        _keySoundText.transform.localScale = Vector3.one;

        float minScale = Mathf.Min(nodeScale.x, nodeScale.y);

        _keySoundText.transform.localScale = new Vector3(inverseX / 2, inverseY / 2, 1f);
        _keySoundText.fontSize = 1;
        _keySoundText.transform.localPosition = new Vector3(0, 0, 0);
        _keySoundText.text = _keySound;
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
