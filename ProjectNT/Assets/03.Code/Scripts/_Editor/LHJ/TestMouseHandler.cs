using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestMouseHandler : MonoBehaviour
{
    private Camera _editorCamera;

    private void Awake()
    {
        _editorCamera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _editorCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Node clickedNode = hit.collider.GetComponent<Node>();
                if (clickedNode != null)
                {
                    NodeData nodeData = clickedNode.GetNodeData();
                    Debug.Log($"위치: ({nodeData.index.x}, {nodeData.index.y})\n" +
                             $"키음: {nodeData.keySound}\n");
                }
            }
        }
    }
}
