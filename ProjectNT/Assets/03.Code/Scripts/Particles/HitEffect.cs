using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem parentParticle;
    [SerializeField] private ParticleSystem lastParticle;
    [SerializeField] private ParticleSystemRenderer craterRenderModule;
    private void OnEnable()
    {
        parentParticle.Play();
    }
    private void Update()
    {
        if (lastParticle.isStopped)
        {
            PoolManager.Instance.hitEffectPool.Push(this);
        }
    }
    public void EffectHorizontal()
    {
        craterRenderModule.renderMode = ParticleSystemRenderMode.HorizontalBillboard;
    }
    public void EffectBillboard()
    {
        craterRenderModule.renderMode = ParticleSystemRenderMode.Billboard;
    }
}
