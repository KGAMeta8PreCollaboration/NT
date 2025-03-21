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
    [SerializeField] private Transform _startTrans;
    [SerializeField] private Transform _endTrans;

    private ScoreManager _scoreManager;

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
            if (i == 0)
            {
                milestones[i] = startTargetDspTime;
            }
            else if (i == divideCount - 1)
            {
                milestones[i] = endTargetDspTime;
            }
            else
            {
                milestones[i] = startTargetDspTime + (interval * (i));
            }
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

        _targetDspTime = startTargetDspTime;

        double totalTime = _targetDspTime - _spawnDspTime;

        double delta = duration / totalTime;
        float distance = (float)delta * (Vector3.Distance(_initialPosition, target.position));
        print($"distance: {distance}");
        _endTrans.localPosition = _startTrans.localPosition + new Vector3(distance, 0, 0);

        CalculateMilestones(duration);
        _connectLineRenderer.Init();

        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    public override void Hit(JudgementType noteType)
    {
        StartHold();
        isHit = true;
        this.judgementType = noteType;

        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }

        OnHit?.Invoke(this);
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

    private void UpdateCurrentMilestoneIndex()
    {
        double currentTime = AudioSettings.dspTime;

        while (currentMilestoneIndex < milestones.Length && currentTime > milestones[currentMilestoneIndex])
        {
            currentMilestoneIndex++;
        }

        //Debug.Log($"현재 milestone 인덱스 업데이트: {currentMilestoneIndex}/{milestones.Length}, 현재 시간: {currentTime:F2}");
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
        print($"삭제 시간 : {AudioSettings.dspTime - _startDspTime:F3}, 생성 시간 : {_spawnDspTime - _startDspTime:F3}, 타겟 시간 : {_targetDspTime - _startDspTime:F3}, 오디오 소스 : {hitSound}");
        OnHit?.Invoke(this);
    }
}
