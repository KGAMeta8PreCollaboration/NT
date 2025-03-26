using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class StickTest : MonoBehaviour
{
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private GameObject hitProjPrefab;
    private XRRayInteractor rayInter;
    private ActionBasedController abc;
    public SphereCollider m_coll;

    private void Awake()
    {
        if (m_coll == null) m_coll = GetComponentInChildren<SphereCollider>();
        abc = GetComponentInParent<ActionBasedController>();
        rayInter = abc.GetComponentInChildren<XRRayInteractor>();
    }

    private void OnEnable()
    {
        abc.activateAction.action.performed += (x) => particleSystem.Play(true);
        abc.activateAction.action.performed += OnTopNoteHit;

    }
    private void OnDisable()
    {
        abc.activateAction.action.performed -= (x) => particleSystem.Play(true);
        abc.activateAction.action.performed -= OnTopNoteHit;
    }

    private void OnTopNoteHit(InputAction.CallbackContext cnt)
    {
        if (rayInter.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.collider.CompareTag("TopNote"))
            {
                TopNoteProjectile proj =
                PoolManager.Instance.topNoteProjPool.Pop();
                proj.transform.SetParent(transform, true);
                // Instantiate(hitProjPrefab, transform, true).GetComponent<TopNoteProjectile>();
                proj.gameObject.transform.position = transform.position;
                proj.Init(transform.position, hit.transform.position);
                abc.SendHapticImpulse(0.8f, 0.15f);
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Woofer")
        {
            abc.SendHapticImpulse(0.6f, 0.15f);
        }
    }
}
