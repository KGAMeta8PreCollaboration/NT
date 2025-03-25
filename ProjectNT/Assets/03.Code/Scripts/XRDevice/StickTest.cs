using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class StickTest : MonoBehaviour
{
    [SerializeField] private new ParticleSystem particleSystem;

    private ActionBasedController abc;

    public SphereCollider m_coll;

    private void Awake()
    {
        if (m_coll == null) m_coll = GetComponentInChildren<SphereCollider>();
        abc = GetComponentInParent<ActionBasedController>();
    }

    private void OnEnable()
    {
        abc.activateAction.action.performed += (x) => particleSystem.Play(true);
    }
    private void OnDisable()
    {
        abc.activateAction.action.performed += (x) => particleSystem.Play(true);
    }
}
