using UnityEngine;

public class LongNote : Note
{
    public double startTargetDspTime;
    public double endTargetDspTime;
    public int divideCount;
    public double[] milestones;
    public int currentMilestoneIndex = 0;
    public bool isHolding = false;
    [SerializeField] private ConnectLineRenderer _connectLineRenderer;

    private ScoreManager _scoreManager;

    //콤보 공식을 위해 사용하는 임시 변수
    public int bpm;

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

        CalculateMilestones(duration);
        _connectLineRenderer.Init(GetDistanceStartPosAndEndPos());

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

    private float GetDistanceStartPosAndEndPos()
    {
        double duration = endTargetDspTime - startTargetDspTime;
        double totalTime = _targetDspTime - _spawnDspTime;

        double delta = duration / totalTime;
        float distance = (float)delta * (Vector3.Distance(_initialPosition, target.position));
        print($"distance: {distance}");

        return distance;
    }
}
