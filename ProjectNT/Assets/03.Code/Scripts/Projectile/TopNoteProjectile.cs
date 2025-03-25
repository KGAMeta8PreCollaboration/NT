using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TopNoteProjectile : MonoBehaviour
{

    private Vector3 startPos;
    private Vector3 target;
    private new ParticleSystem particleSystem;
    public void Init(Vector3 startPos, Vector3 target)
    {
        this.startPos = startPos;
        this.target = target;
        transform.LookAt(target);
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Move()
    {
        transform.position = Vector3.Lerp(startPos, target, 1f);
    }

    private void Update()
    {
        Move();

        if (particleSystem.time > 0.3)
        {
            PoolManager.Instance.topNoteProjPool.Push(this);
        }
    }

}
