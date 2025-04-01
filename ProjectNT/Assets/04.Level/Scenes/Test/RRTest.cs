using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RRTest : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public LineRenderer lineRenderer;
    public LineRenderer lineRenderer2;
    private void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        //lineRenderer.alignment = LineAlignment.TransformZ;
        //lineRenderer.startWidth = 0.8f;
        //lineRenderer.endWidth = 0.8f;
        lineRenderer2.positionCount = 2;
        lineRenderer2.useWorldSpace = true;
        //lineRenderer2.alignment = LineAlignment.TransformZ;

    }
    Vector3 offset = new Vector3(-0.35f, 0.05f, 0);

    void Update()
    {
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPosition(1, end.position);

        lineRenderer2.SetPosition(0, start.position + offset);
        lineRenderer2.SetPosition(1, end.position + offset);

    }
}
