using System;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum JudgementType
{
    MISS,
    Good,
    Cool,
    PERFECT,
}

public abstract class Note : MonoBehaviour
{
    public Transform target { get; protected set; }
    public JudgementType judgementType { get; protected set; }
    public bool isHit { get; protected set; }
    public Action<Note> OnDestroyed;
    public Action<Note> OnHit;
    public AudioClip hitSound;
    protected Vector3 _initialPosition;
    protected Vector3 _direction;
    protected double _spawnDspTime;
    protected double _targetDspTime;
    protected double _startDspTime;
    protected float _speed;
    protected bool _isTargetReached;
    protected ScoreManager _scoreManager;

    public virtual void Init(Transform target, NoteSpawnData noteSpawnData, Transform indicatorPos = null)
    {
        isHit = false;
        _isTargetReached = false;
        this.target = target;
        this.hitSound = noteSpawnData.hitSound;
        _spawnDspTime = noteSpawnData.spawnDspTime;
        _initialPosition = transform.position;
        _startDspTime = AudioManager.Instance.startDspTime;
        _speed = CalculateSpeed();
        _direction = (target.position - _initialPosition).normalized;

        OnHit += (note) => PostJudgement();
    }

    protected abstract void PostJudgement();
    protected float CalculateSpeed()
    {
        return Vector3.Distance(_initialPosition, target.position) / (float)(_targetDspTime - _spawnDspTime);
    }
    public void SetScoreManager(ScoreManager scoreManager)
    {
        _scoreManager = scoreManager;
    }

    protected virtual void Move()
    {
        double currentTime = AudioSettings.dspTime;
        double elapsedTime = currentTime - _spawnDspTime;
        double totalTime = _targetDspTime - _spawnDspTime;

        float timeProgress = Mathf.Clamp01((float)(elapsedTime / totalTime));
        if (target)
            transform.position = Vector3.Lerp(_initialPosition, target.position, timeProgress);
    }

    protected virtual void Update()
    {
        if (_isTargetReached || Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            _isTargetReached = true;
            _speed = CalculateSpeed();
            _direction = (target.position - _initialPosition).normalized;
            transform.position += _direction * _speed * Time.deltaTime;
        }
        else
        {
            Move();
        }
    }

    protected virtual void Destroy()
    {
        OnDestroyed?.Invoke(this);
        OnDestroyed = null;
        // print($"삭제 시간 : {AudioSettings.dspTime - _startDspTime:F3}, 생성 시간 : {_spawnDspTime - _startDspTime:F3}, 타겟 시간 : {_targetDspTime - _startDspTime:F3} ,오디오 소스 : {hitSound}");
        // Destroy(gameObject);
        EffectManager.Instance.EffectInvoke(this, judgementType, _scoreManager.currentCombo);
        PoolManager.Instance.PushNote(this);
    }

    public double GetTargetDspTime()
    {
        return _targetDspTime;
    }

    public abstract void Hit(JudgementType noteType);
}
