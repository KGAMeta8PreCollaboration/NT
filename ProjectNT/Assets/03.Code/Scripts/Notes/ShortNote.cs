using UnityEngine;

public class ShortNote : Note
{
    public double targetDspTime;

    public override void Init(Transform target, NoteSpawnData noteSpawnData, Transform indicatorPos = null)
    {
        base.Init(target, noteSpawnData);

        ShortNoteSpawnData shortNoteSpawnData = noteSpawnData as ShortNoteSpawnData;

        targetDspTime = shortNoteSpawnData.targetDspTime;

        _targetDspTime = targetDspTime;
    }

    public override void Hit(JudgementType noteType)
    {
        Destroy();
        isHit = true;
        this.judgementType = noteType;
        if (judgementType != JudgementType.Bad)
            //HitEffect();
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
        _scoreManager.AddJudgeCount(judgementType);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NoteScanner"))
            Miss();
    }

    private void Miss()
    {
        Destroy();
        isHit = true;
        judgementType = JudgementType.Bad;
        OnHit?.Invoke(this);
        OnHit = null;
    }
}
