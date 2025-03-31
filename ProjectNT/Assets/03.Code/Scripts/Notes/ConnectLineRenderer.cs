using Photon.Pun.Demo.SlotRacer.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ConnectLineRenderer : MonoBehaviour
{
    public Transform start;
    public Transform endNote;
    public Transform endPoint;
    public Transform startRenderer;
    public LineRenderer lineRenderer;
    public BoxCollider boxCollider;
    private Transform _origin;
    private Transform _target;
    public ConnectLineRendererOutLine leftLR;
    public ConnectLineRendererOutLine rightLR;

    public void Init(float distance, Transform target)
    {
        Vector3 railDirection = (start.position - target.position).normalized;

        endNote.localPosition = start.localPosition + (railDirection * distance);
        //end.localPosition = start.localPosition + new Vector3(distance, 0, 0);

        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.alignment = LineAlignment.TransformZ;

        float z = (endNote.localPosition.z - start.localPosition.z);

        // print($"롱노트 startPos: {start.localPosition.z}, endPos: {end.localPosition.z}");
        // print($"롱노트 startPos와 endPos의 차이: {z}");
        boxCollider.size = new Vector3(0.4f, 1, Mathf.Abs(z));
        boxCollider.center = new Vector3(0, 0, (z / 2));

        _origin = start;
        _target = target;

        leftLR.Init(target);
        rightLR.Init(target);
    }

    void Update()
    {
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }

    public void Hold()
    {
        start = _target;
        leftLR.Hold();
        rightLR.Hold();
    }

    public void Release()
    {
        //start = _origin;
        startRenderer.position = _target.position;
        start = _origin;

        leftLR.Release();
        rightLR.Release();
    }
}
