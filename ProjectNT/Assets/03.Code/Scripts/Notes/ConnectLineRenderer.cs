using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectLineRenderer : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public LineRenderer lineRenderer;
    public BoxCollider boxCollider;

    public void Init()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.alignment = LineAlignment.TransformZ;

        float x = Mathf.Abs(start.localPosition.x - end.localPosition.x);

        print($"롱노트 startPos: {start.localPosition.x}, endPos: {end.localPosition.x}");
        print($"롱노트 startPos와 endPos의 차이: {x}");
        boxCollider.size = new Vector3(x, 1, 0.4f);
        boxCollider.center = new Vector3(x / 2, 0, 0);
    }

    void Update()
    {
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPosition(1, end.position);
    }
}
