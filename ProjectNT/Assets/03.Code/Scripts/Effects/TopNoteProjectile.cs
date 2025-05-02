using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ExitGames.Client.Photon;
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
        if (null == particleSystem)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }
        transform.DOMove(target, 0.3f).SetEase(Ease.OutQuint).onComplete += () => PoolManager.Instance.topNoteProjPool.Push(this);
        particleSystem.Play();
    }
}
