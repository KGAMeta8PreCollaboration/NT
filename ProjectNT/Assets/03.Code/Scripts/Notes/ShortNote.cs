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
		isHit = true;
		this.judgementType = noteType;
		OnHit?.Invoke(this);
		OnHit = null;
		if (judgementType != JudgementType.MISS)
		{
			PoolManager.Instance.HitEffect(transform.position, true);
		}
		EffectManager.Instance.playerMapEffect?.Invoke(this, _scoreManager.currentCombo, playMode);
		// EffectManager.Instance.player2MapEffect?.Invoke(this, _scoreManager.currentCombo);
		Destroy();
	}

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

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("NoteScanner"))
			Miss();
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
