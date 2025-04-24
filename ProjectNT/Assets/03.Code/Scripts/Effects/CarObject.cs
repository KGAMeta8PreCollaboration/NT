using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarObject : MonoBehaviour
{
    public MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

}
