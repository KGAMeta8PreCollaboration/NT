using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TopNote : Note
{
    [SerializeField] private InputActionReference leftTrigger;
    [SerializeField] private InputActionReference rightTrigger;
    [SerializeField] private new ParticleSystem particleSystem;
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
        particleSystem.Play(true);
    }
    private void OnDisable()
    {
        leftTrigger.action.performed -= Hit;
        rightTrigger.action.performed -= Hit;
        particleSystem.Play(false);
    }

    public override void Init(Transform target, NoteSpawnData noteSpawnData/*, Transform indicatorPos*/)
    {
        base.Init(target, noteSpawnData);

        TopNoteSpawnData topNoteSpawnData = noteSpawnData as TopNoteSpawnData;

        targetDspTime = topNoteSpawnData.canInterDspTime;

        _targetDspTime = targetDspTime;

        _scoreManager = FindObjectOfType<ScoreManager>();

        xRSimInter = GetComponent<XRSimpleInteractable>();

        TopNoteIndicater topNoteIndicater = PoolManager.Instance.topNoteIndicaterPool.Pop();
        // topNoteIndicater.transform.position = indicatorPos.position;
    }
    private void Hit(InputAction.CallbackContext ctn)
    {
        Debug.Log(xRSimInter.isHovered);
        if (!canInter || !xRSimInter.isHovered) return;
        Destroy();
        isHit = true;
        this.judgementType = JudgementType.PERFECT;
        if (judgementType != JudgementType.MISS)
            PoolManager.Instance.HitEffect(transform.position, false);

        OnHit?.Invoke(this);
        OnHit = null;
    }

    public override void Hit(JudgementType noteType) { }

    protected override void PostJudgement()
    {
        if (judgementType == JudgementType.MISS)
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
        judgementType = JudgementType.MISS;
        OnHit?.Invoke(this);
        OnHit = null;
    }
}
