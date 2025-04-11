using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TopNote : Note
{
    [SerializeField] private new ParticleSystem particleSystem;
    private XRSimpleInteractable xRSimInter;
    public bool canInter = false;
    private bool isIndicatorOn;
    private void Awake()
    {
        xRSimInter = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        canInter = false;
        isIndicatorOn = false;
        gameObject.tag = "Untagged";
    }
    private void OnDisable()
    {

        particleSystem.Stop();
    }
    protected override void Update()
    {
        base.Update();
        if (true == isIndicatorOn)
        {
            return;
        }

        if (_targetDspTime - AudioSettings.dspTime <= 1)
        {
            particleSystem.Play();
            isIndicatorOn = true;
        }

    }
    public override void Init(Transform target, NoteSpawnData noteSpawnData, Transform indicatorPos)
    {
        print($"상단 노트 : {noteSpawnData.hitSound}");
        base.Init(target, noteSpawnData);

        TopNoteSpawnData topNoteSpawnData = noteSpawnData as TopNoteSpawnData;

        _targetDspTime = topNoteSpawnData.targetDspTime;

        _scoreManager = FindObjectOfType<ScoreManager>();

        xRSimInter = GetComponent<XRSimpleInteractable>();

    }
    public void Hit()
    {

        if (false == canInter || false == xRSimInter.isHovered)
        {
            return;
        }
        isHit = true;
        this.judgementType = JudgementType.PERFECT;
        OnHit?.Invoke(this);
        OnHit = null;
        EffectManager.Instance.OnMapEffect?.Invoke(this, _scoreManager.currentCombo);
        AudioManager.Instance.Play(hitSound.name, transform);
        PoolManager.Instance.HitEffect(transform.position, false);

        if (judgementType == JudgementType.MISS)
        {
            _scoreManager.ResetCombo();
        }
        else
        {
            _scoreManager.IncreaseCombo();
        }

        _scoreManager.AddScore(judgementType);
        _scoreManager.ShowJudgementType(judgementType);
        _scoreManager.AddJudgeCount(judgementType);
        Destroy();
    }
    public void AutoHit(InputAction.CallbackContext ctn)
    {
        isHit = true;
        this.judgementType = JudgementType.PERFECT;
        OnHit?.Invoke(this);
        OnHit = null;

        AudioManager.Instance.Play(hitSound.name, transform);
        PoolManager.Instance.HitEffect(transform.position, false);

        if (judgementType == JudgementType.MISS)
        {
            _scoreManager.ResetCombo();
        }
        else
        {
            _scoreManager.IncreaseCombo();
        }

        _scoreManager.AddScore(judgementType);
        _scoreManager.ShowJudgementType(judgementType);
        _scoreManager.AddJudgeCount(judgementType);
        Destroy();
    }

    public override void Hit(JudgementType noteType) { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TopNoteZone"))
        {
            canInter = true;
            gameObject.tag = "TopNote";
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
            gameObject.tag = "Untagged";
        }
    }

    private void Miss()
    {
        isHit = true;
        judgementType = JudgementType.MISS;
        OnHit?.Invoke(this);
        OnHit = null;
        Destroy();
    }

    protected override void PostJudgement() { }
}
