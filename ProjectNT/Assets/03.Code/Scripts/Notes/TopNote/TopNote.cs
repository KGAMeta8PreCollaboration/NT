using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TopNote : Note
{
    [SerializeField] InputActionReference leftTrigger;
    [SerializeField] InputActionReference rightTrigger;
    private ScoreManager _scoreManager;
    private XRSimpleInteractable xRSimInter;
    private bool canInter = false;
    public double targetDspTime;

    private void Awake()
    {
        xRSimInter = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        leftTrigger.action.performed += Hit;
        rightTrigger.action.performed += Hit;
    }
    private void OnDisable()
    {
        leftTrigger.action.performed -= Hit;
        rightTrigger.action.performed -= Hit;
    }

    public override void Init(Transform target, NoteSpawnData noteSpawnData)
    {
        base.Init(target, noteSpawnData);

        TopNoteSpawnData topNoteSpawnData = noteSpawnData as TopNoteSpawnData;

        targetDspTime = topNoteSpawnData.canInterDspTime;

        _targetDspTime = targetDspTime;

        _scoreManager = FindObjectOfType<ScoreManager>();

        xRSimInter = GetComponent<XRSimpleInteractable>();
    }
    private void Hit(InputAction.CallbackContext ctn)
    {
        Debug.Log(xRSimInter.isHovered);
        if (!canInter || !xRSimInter.isHovered) return;
        Destroy();
        isHit = true;
        this.judgementType = JudgementType.Perfect;
        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }
        OnHit?.Invoke(this);
    }

    public override void Hit(JudgementType noteType) { }

    protected override void PostJudgement()
    {
        if (judgementType == JudgementType.Bad)
            _scoreManager.ResetCombo();
        else
            _scoreManager.IncreaseCombo();
        _scoreManager.AddScore(judgementType);
        _scoreManager.ShowJudgementType(judgementType);
        _scoreManager.AddJudgeCount(judgementType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TopNoteZone"))
        {
            canInter = true;
        }
        if (other.CompareTag("Woofer"))
        {
            Miss();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TopNoteZone"))
        {
            canInter = false;
        }
    }

    private void Miss()
    {
        Destroy();
        isHit = true;
        judgementType = JudgementType.Bad;
        OnHit?.Invoke(this);
    }

    float t;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("TopNoteZone"))
        {
            t += Time.deltaTime;
        }
    }
}
