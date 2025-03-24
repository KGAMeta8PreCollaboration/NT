using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TopNote : Note
{
    [SerializeField] InputActionReference leftTrigger;
    private ScoreManager _scoreManager;
    private XRSimpleInteractable xRSimInter;
    private bool canInter = false;
    public double targetDspTime;

    private void Awake()
    {
        leftTrigger.action.performed += Hit;
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
        if (!canInter) return;
        Destroy();
        Debug.LogError("1");
        isHit = true;
        this.judgementType = JudgementType.Perfect;
        if (hitEffect != null)
        {
            Debug.LogError("1-1");
            ParticleSystem effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }
        Debug.LogError("2");
        OnHit?.Invoke(this);
        Debug.LogError("3");
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
