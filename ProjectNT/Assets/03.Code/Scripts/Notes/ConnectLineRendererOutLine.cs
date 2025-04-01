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
        Transform parentTransform = GetRootParent(transform);
        if (isLeft)
        {
            start = FindDeepChildComponent<Transform>(parentTransform, "LeftStart");
            end = FindDeepChildComponent<Transform>(parentTransform, "LeftEnd");
            _target = FindDeepChildComponent<Transform>(target, "LeftPos");
        }
        else
        {
            start = FindDeepChildComponent<Transform>(parentTransform, "RightStart");
            end = FindDeepChildComponent<Transform>(parentTransform, "RightEnd");
            _target = FindDeepChildComponent<Transform>(target, "RightPos");
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

    public void Destroy()
    {
        start = origin;
        origin = null;
    }
}

