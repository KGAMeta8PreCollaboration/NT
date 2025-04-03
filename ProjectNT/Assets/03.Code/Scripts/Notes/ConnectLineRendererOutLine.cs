using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectLineRendererOutLine : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public LineRenderer outLineRenderer;
    public Transform _target;
    public bool isLeft;
    public Transform origin;

    public void Init(Transform target)
    {
        Transform parentTransform = TransformUtil.GetRootParent(transform);
        if (isLeft)
        {
            start = TransformUtil.FindDeepChildComponent<Transform>(parentTransform, "LeftStart");
            end = TransformUtil.FindDeepChildComponent<Transform>(parentTransform, "LeftEnd");
            _target = TransformUtil.FindDeepChildComponent<Transform>(target, "LeftPos");
        }
        else
        {
            start = TransformUtil.FindDeepChildComponent<Transform>(parentTransform, "RightStart");
            end = TransformUtil.FindDeepChildComponent<Transform>(parentTransform, "RightEnd");
            _target = TransformUtil.FindDeepChildComponent<Transform>(target, "RightPos");
        }
        outLineRenderer = GetComponent<LineRenderer>();
        origin = start;

        outLineRenderer.positionCount = 2;
        outLineRenderer.useWorldSpace = true;
        outLineRenderer.alignment = LineAlignment.TransformZ;
    }

    private void Update()
    {
        outLineRenderer.SetPosition(0, start.position);
        outLineRenderer.SetPosition(1, end.position);
    }

    public void Hold()
    {
        start = _target;
    }

    public void Release()
    {
        start = origin;
    }

    public void Destroy()
    {
        start = origin;
        origin = null;
    }
}

