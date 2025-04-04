using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRControllerPosition : MonoBehaviour
{
    private ActionBasedController _xrController;
    public float minY = 1.5f;  // 제한할 Y값
    public float maxY = 2.5f;

    void Start()
    {
        _xrController = GetComponent<ActionBasedController>();
    }

    void Update()
    {
        if (_xrController)
        {
            Vector3 pos = transform.position;

            // Y값 제한 적용
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            transform.position = pos;
        }
    }
}
