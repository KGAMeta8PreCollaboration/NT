using UnityEngine;

public class LongNote : Note
{
	public double startTargetDspTime;
	public double endTargetDspTime;
	public int divideCount = 5;
	public double[] milestones;
	public int currentMilestoneIndex = 0;
	public bool isHolding = false;
	[SerializeField] private ConnectLineRenderer _connectLineRenderer;

	private void Start()
	{
		//startTargetDspTime = AudioSettings.dspTime + 10d; //3초
		//endTargetDspTime = AudioSettings.dspTime + 20d; //7초

		//double duration = endTargetDspTime - startTargetDspTime;
		//print($"롱노트 지속시간: {duration}초, 현재 시간: {AudioSettings.dspTime.ToString("f2")}");

		//CalculateMilestones(duration);
	}

	private void CalculateMilestones(double duration)
	{
		milestones = new double[divideCount];
		double interval = duration / divideCount;
		for (int i = 0; i < divideCount; i++)
		{
			milestones[i] = startTargetDspTime + (interval * (i + 1));
			Debug.Log($"롱노트 판정 시간: {(milestones[i] - startTargetDspTime):F2}초");
		}
	}

	public override void Init(Transform target, NoteSpawnData noteSpawnData)
	{
		base.Init(target, noteSpawnData);

		LongNoteSpawnData longNoteSpawnData = noteSpawnData as LongNoteSpawnData;

		_isTargetReached = false;
		this.startTargetDspTime = longNoteSpawnData.startTargetDspTime;
		this.endTargetDspTime = longNoteSpawnData.endTargetDspTime;
		double duration = endTargetDspTime - startTargetDspTime;
		print($"롱노트 지속시간: {duration}초, 현재 시간: {AudioSettings.dspTime.ToString("f2")}");

		CalculateMilestones(duration);
		_connectLineRenderer.Init();
	}

	protected override void PostJudgement()
	{

	}

	public override void Hit(JudgementType noteType)
	{
		StartHold();
	}

	public void StartHold()
	{
		isHolding = true;
		//currentMilestoneIndex = 0;
		UpdateCurrentMilestoneIndex();
	}

	public bool Hold()
	{
		if (!isHolding || currentMilestoneIndex >= milestones.Length || AudioSettings.dspTime >= endTargetDspTime)
			return false;

		double currentTime = AudioSettings.dspTime;
		if (currentTime >= milestones[currentMilestoneIndex])
		{
			currentMilestoneIndex++;
			return true;
		}
		return false;
	}

	public void Release()
	{
		isHolding = false;
		Debug.Log("롱노트 종료!");
	}

	private void UpdateCurrentMilestoneIndex()
	{
		double currentTime = AudioSettings.dspTime;

		while (currentMilestoneIndex < milestones.Length && currentTime > milestones[currentMilestoneIndex])
		{
			currentMilestoneIndex++;
		}

		Debug.Log($"현재 milestone 인덱스 업데이트: {currentMilestoneIndex}/{milestones.Length}, 현재 시간: {currentTime:F2}");
	}


	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Woofer"))
			Miss();
	}

	private void Miss()
	{
		Destroy();
		isHit = true;
		judgementType = JudgementType.Bad;
		OnHit?.Invoke(this);
	}
}
