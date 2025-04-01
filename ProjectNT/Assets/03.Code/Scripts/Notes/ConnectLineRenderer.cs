using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectLineRenderer : MonoBehaviour
{
    public Transform startNote;
    public Transform endNote;
    public Transform startPoint;
    public Transform endPoint;
    public Transform startRenderer;
    public LineRenderer lineRenderer;
    public BoxCollider boxCollider;
    public Transform _origin;
    public Transform _target;
    public ConnectLineRendererOutLine leftLR;
    public ConnectLineRendererOutLine rightLR;

    private Vector3 _startRendererOriginPos;

    public void Init(float distance, Transform target)
    {
        Transform parentTransform = GetRootParent(transform);
        print($"=========={parentTransform.name}============");
        startNote = FindDeepChildComponent<Transform>(parentTransform, "StartNote");
        endNote = FindDeepChildComponent<Transform>(parentTransform, "EndNote");
        startPoint = FindDeepChildComponent<Transform>(parentTransform, "Start");
        endPoint = FindDeepChildComponent<Transform>(parentTransform, "End");
        startRenderer = FindDeepChildComponent<Transform>(parentTransform, "Disc");
        lineRenderer = GetComponent<LineRenderer>();
        leftLR = FindDeepChildComponent<ConnectLineRendererOutLine>(transform, "LeftOutLineRenderer");
        rightLR = FindDeepChildComponent<ConnectLineRendererOutLine>(transform, "RightOutLineRenderer");

        _startRendererOriginPos = new Vector3(0, 0.1f, 0);
        print($"Init 시 롱노트 렌더러 디스크 POS: {startRenderer.position}");
        Vector3 railDirection = (startNote.position - target.position).normalized;

        endNote.localPosition = startNote.localPosition + (railDirection * distance);
        //end.localPosition = start.localPosition + new Vector3(distance, 0, 0);

        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.alignment = LineAlignment.TransformZ;

        float z = (endNote.localPosition.z - startNote.localPosition.z);

        // print($"롱노트 startPos: {start.localPosition.z}, endPos: {end.localPosition.z}");
        // print($"롱노트 startPos와 endPos의 차이: {z}");
        boxCollider.size = new Vector3(0.4f, 1.5f, Mathf.Abs(z));
        boxCollider.center = new Vector3(0, 0, (z / 2));

        _origin = startPoint;
        _target = target;

        leftLR.Init(target);
        rightLR.Init(target);
    }

    void Update()
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }

    public void Hold()
    {
        startPoint = _target;
        leftLR.Hold();
        rightLR.Hold();
    }

    public void Release()
    {
        //start = _origin;
        startRenderer.position = _target.position;
        startPoint = _origin;

        leftLR.Release();
        rightLR.Release();
    }

    public void Destroy()
    {
        startPoint = _origin;
        //_origin = null;
        startRenderer.position = _startRendererOriginPos;
        print($"Destroy 시 롱노트 렌더러 디스크 POS: {startRenderer.position}");
        _target = null;

        leftLR.Destroy();
        rightLR.Destroy();
    }

    public T FindDeepChildComponent<T>(Transform parent, string name) where T : Component
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child.GetComponent<T>();
            }
            T result = FindDeepChildComponent<T>(child, name);
            if (result != null)
                return result;
        }
        return null;
    }

    public Transform GetRootParent(Transform child)
    {
        while (child.parent != null)
        {
            child = child.parent;
        }
        return child;
    }
}
