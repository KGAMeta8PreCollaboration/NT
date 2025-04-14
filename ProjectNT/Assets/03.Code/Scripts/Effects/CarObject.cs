using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarObject : MonoBehaviour
{
    [SerializeField]
    private Renderer renderer;

    public Material CarMaterial
    {
        get { return renderer.sharedMaterials[0]; }
        set
        {
            Material[] materials = renderer.sharedMaterials;
            materials[0] = value;
            renderer.sharedMaterials = materials;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CarEndPos"))
        {
            PoolManager.Instance.carEffectPool.Push(this);
        }
    }
}
