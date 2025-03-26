using Photon.Pun.Demo.SlotRacer.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ConnectLineRenderer : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public Transform startRenderer;
    public LineRenderer lineRenderer;
    public BoxCollider boxCollider;
    private Transform _origin;
    private Transform _target;

    public void Init(float distance, Transform target)
    {
        Vector3 railDirection = (start.position - target.position).normalized;

        end.localPosition = start.localPosition + (railDirection * distance);
        //end.localPosition = start.localPosition + new Vector3(distance, 0, 0);

        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.alignment = LineAlignment.TransformZ;

        float z = (end.localPosition.z - start.localPosition.z);

        print($"롱노트 startPos: {start.localPosition.z}, endPos: {end.localPosition.z}");
        print($"롱노트 startPos와 endPos의 차이: {z}");
        boxCollider.size = new Vector3(0.4f, 1, Mathf.Abs(z));
        boxCollider.center = new Vector3(0, 0, (z / 2));

        _origin = start;
        _target = target;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPosition(1, end.position);
    }

    public void Hold()
    {
        start = _target;
    }

    public void Release()
    {
        //start = _origin;
        startRenderer.position = _target.position;
        start = startRenderer;
    }
}
