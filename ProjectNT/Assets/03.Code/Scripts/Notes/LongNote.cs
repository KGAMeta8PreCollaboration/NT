using UnityEngine;

public class LongNote : Note
{
    public double startTargetDspTime;
    public double endTargetDspTime;
    public int divideCount;
    public double[] milestones;
    public int currentMilestoneIndex = 0;
    public bool isHolding = false;
    public bool isFirstHolding = false; // 첫 판정에 홀드했는지
    public bool isDisconnected = false; // 중간에 끊긴적 있는지
    public bool isEnd;
    [SerializeField] private ConnectLineRenderer _connectLineRenderer;

    //콤보 공식을 위해 사용하는 임시 변수
    public int bpm;

    protected override void Update()
    {
        base.Update();

        double currentTime = AudioSettings.dspTime;

        // milestone 도달했는데 홀드하지 않았다면 BAD 처리
        if (currentMilestoneIndex < milestones.Length && currentTime >= milestones[currentMilestoneIndex])
        {
            if (!isHolding)
            {
                HandleMissedMilestone();
            }
        }
    }

    private void HandleMissedMilestone()
    {
        Debug.Log($"Milestone {currentMilestoneIndex}에서 노트가 홀드되지 않음! Bad 판정");

        // BAD 판정 적용
        judgementType = JudgementType.MISS;
        if (judgementType == JudgementType.MISS)
            _scoreManager.ResetCombo();
        else
            _scoreManager.IncreaseCombo();
        _scoreManager.AddScore(judgementType);
        _scoreManager.ShowJudgementType(judgementType);
        _scoreManager.AddJudgeCount(judgementType);

        currentMilestoneIndex++;
    }


    private void CalculateMilestones(double duration)
    {
        float beatInterval = (float)60 / (bpm * 4);
        print($"롱노트 지속시간: {duration}, 16비트 간격: {beatInterval}");
        int combo = Mathf.FloorToInt((float)duration / beatInterval);
        print($"롱노트의 총 콤보 수: {combo}");

        divideCount = combo;

        milestones = new double[divideCount];
        double interval = duration / divideCount;
        for (int i = 0; i < divideCount; i++)
        {
            //if (i == 0)
            //{
            //    milestones[i] = startTargetDspTime;
            //}
            //else if (i == divideCount - 1)
            //{
            //    milestones[i] = endTargetDspTime;
            //}
            //else
            //{
            milestones[i] = startTargetDspTime + (interval * (i + 1));
            //}
        }
    }

    public override void Init(Transform target, NoteSpawnData noteSpawnData, Transform indicaterPos = null)
    {
        base.Init(target, noteSpawnData);

        isHolding = false;
        isFirstHolding = false; // 첫 판정에 홀드했는지
        isDisconnected = false; // 중간에 끊긴적 있는지
        isEnd = false;
        currentMilestoneIndex = 0;

        LongNoteSpawnData longNoteSpawnData = noteSpawnData as LongNoteSpawnData;

        _isTargetReached = false;
        this.startTargetDspTime = longNoteSpawnData.startTargetDspTime;
        this.endTargetDspTime = longNoteSpawnData.endTargetDspTime;
        double duration = endTargetDspTime - startTargetDspTime;
        print($"롱노트 지속시간: {duration}초, 현재 시간: {AudioSettings.dspTime.ToString("f2")}");

        _targetDspTime = startTargetDspTime;

        CalculateMilestones(duration);
        _connectLineRenderer.Init(GetDistanceStartPosAndEndPos(), target);
    }

    public override void Hit(JudgementType noteType)
    {
        StartHold();
        if (currentMilestoneIndex == 0) isFirstHolding = true;
        if (isFirstHolding)
        {
            isHit = true;
            this.judgementType = noteType;
            OnHit?.Invoke(this);
            OnHit = null;
        }
    }

    public void StartHold()
    {
        isHolding = true;
        UpdateCurrentMilestoneIndex();
    }

    public void Hold(Transform wofferTransform)
    {
        if (currentMilestoneIndex >= milestones.Length || AudioSettings.dspTime >= endTargetDspTime)
        {
            Destroy();
        }

        double currentTime = AudioSettings.dspTime;
        if (currentTime >= milestones[currentMilestoneIndex])
        {
            _connectLineRenderer.Hold();

            Debug.Log($"Hold에 들어온 판단 타입: {judgementType.ToString()}");
            if (isDisconnected) judgementType = JudgementType.Good;

            if (judgementType == JudgementType.MISS)
                _scoreManager.ResetCombo();
            else
                _scoreManager.IncreaseCombo();
            _scoreManager.AddScore(judgementType);
            _scoreManager.ShowJudgementType(judgementType);
            _scoreManager.AddJudgeCount(judgementType);
            PoolManager.Instance.HitEffect(wofferTransform.position, true);
            currentMilestoneIndex++;
        }
    }

    public void Release()
    {
        isHolding = false;
        isDisconnected = true;

        _connectLineRenderer.Release();
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

    private void UpdateCurrentMilestoneIndex()
    {
        double currentTime = AudioSettings.dspTime;

        while (currentMilestoneIndex < milestones.Length && currentTime > milestones[currentMilestoneIndex])
        {
            currentMilestoneIndex++;
        }
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
        print($"삭제 시간 : {AudioSettings.dspTime - _startDspTime:F3}, 생성 시간 : {_spawnDspTime - _startDspTime:F3}, 타겟 시간 : {_targetDspTime - _startDspTime:F3}, 오디오 소스 : {hitSound}");
        print($"롱노트 Miss 호출. 삭제 시간: {AudioSettings.dspTime - _startDspTime}");
        OnHit?.Invoke(this);
        OnHit = null;
    }

    private float GetDistanceStartPosAndEndPos()
    {
        double duration = endTargetDspTime - startTargetDspTime;
        double totalTime = _targetDspTime - _spawnDspTime;

        double delta = duration / totalTime;
        float distance = (float)delta * (Vector3.Distance(_initialPosition, target.position));
        print($"distance: {distance}");

        return distance;
    }

    protected override void Destroy()
    {
        isEnd = true;
        //_connectLineRenderer.Destroy();
        base.Destroy();

    }
}
