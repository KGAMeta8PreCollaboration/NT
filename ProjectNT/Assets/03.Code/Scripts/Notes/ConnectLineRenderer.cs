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

    private void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.alignment = LineAlignment.TransformZ;

        float x = Mathf.Abs(start.position.x - end.position.x);
        boxCollider.size = new Vector3(x, 1, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPosition(1, end.position);
    }
}
