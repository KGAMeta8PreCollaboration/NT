using UnityEngine;

public class ShortNote : Note
{
    public double targetDspTime;

    // TODO: GameManager.Scoremanager로 변경할듯?
    private ScoreManager _scoreManager;

    public override void Init(Transform target, NoteSpawnData noteSpawnData)
    {
        base.Init(target, noteSpawnData);

        ShortNoteSpawnData shortNoteSpawnData = noteSpawnData as ShortNoteSpawnData;

        targetDspTime = shortNoteSpawnData.targetDspTime;

        _targetDspTime = targetDspTime;

        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    public override void Hit(JudgementType noteType)
    {
        Destroy();
        PoolManager.Instance.shortNotePool.Push(this);
        isHit = true;
        this.judgementType = noteType;
        if (judgementType != JudgementType.Bad)
            HitEffect();
        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }
        OnHit?.Invoke(this);
        OnHit = null;
    }

    protected override void PostJudgement()
    {
        if (judgementType == JudgementType.Bad)
            _scoreManager.ResetCombo();
        else
            _scoreManager.IncreaseCombo();
        _scoreManager.AddScore(judgementType);
        _scoreManager.ShowJudgementType(judgementType);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Woofer"))
            Miss();
    }

    private void Miss()
    {
        Destroy();
        PoolManager.Instance.shortNotePool.Push(this);
        isHit = true;
        judgementType = JudgementType.Bad;
        OnHit?.Invoke(this);
        OnHit = null;
    }
}
