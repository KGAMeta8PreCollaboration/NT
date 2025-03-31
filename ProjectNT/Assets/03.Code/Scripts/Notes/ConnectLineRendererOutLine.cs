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
        outLineRenderer.positionCount = 2;
        outLineRenderer.useWorldSpace = true;
        outLineRenderer.alignment = LineAlignment.TransformZ;

        origin = start;

        _target = FindDeepChildComponent<Transform>(target, isLeft == true ? "LeftPos" : "RightPos");
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
}

