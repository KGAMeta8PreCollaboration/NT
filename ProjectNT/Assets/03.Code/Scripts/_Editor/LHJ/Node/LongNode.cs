using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LongNode : Node
{
    private Vector2Int _endIndex;
    public Vector2Int StartIndex { get; private set; }
    public Vector2Int EndIndex { get; private set; }

    public void InitializeLongNode(Vector2Int start, Vector2Int end, string keySound)
    {
        _keySoundText = GetComponentInChildren<TextMeshPro>();

        _index = start;
        _endIndex = end;
        _nodeType = EditorNoteType.LongNote;
        _keySound = keySound ?? EditorDataManager.Instance.CurKeySoundName;

        //프로퍼티 초기화
        StartIndex = start;
        EndIndex = end;
        print($"롱노드 키 사운드 : {_keySound}");
        UpdateKeySoundText();
    }

    private void UpdateKeySoundText()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        Vector3 startPos = lineRenderer.GetPosition(0);
        Vector3 endPos = lineRenderer.GetPosition(1);
        Vector3 middlePos = (startPos + endPos) / 2;

        Vector3 nodeScale = transform.lossyScale;

        float inverseX = 1f / nodeScale.x;
        float inverseY = 1f / nodeScale.y;
        _keySoundText.transform.localScale = Vector3.one;

        float minScale = Mathf.Min(nodeScale.x, nodeScale.y);
        _keySoundText.fontSize = 1;

        _keySoundText.transform.localScale = new Vector3(inverseX / 2, inverseY / 2, 1f);
        _keySoundText.transform.position = middlePos;
        _keySoundText.text = _keySound;
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
