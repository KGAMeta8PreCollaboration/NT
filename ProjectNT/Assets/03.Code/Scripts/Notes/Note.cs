using System;
using UnityEngine;

public enum NoteType
{
    Bad,
    Good,
    Cool,
    Perfect,
}

public abstract class Note : MonoBehaviour
{
    public Transform target { get; protected set; }
    public NoteType noteType { get; protected set; }
    public bool isHit { get; protected set; }
    public Action<Note> OnDestroyed;
    public Action<Note> OnHit;
    public AudioClip hitSound;

    [SerializeField] protected ParticleSystem hitEffect;

    protected Vector3 _initialPosition;
    protected Vector3 _direction;
    protected double _spawnDspTime;
    protected double _targetDspTime;
    protected double _startDspTime;
    protected float _speed;
    protected bool _isTargetReached;

    public void Init(Transform target, double spawnDspTime, double targetDspTime, AudioClip hitSound = null)
    {
        _isTargetReached = false;
        this.target = target;
        this.hitSound = hitSound;
        _spawnDspTime = spawnDspTime;
        _targetDspTime = targetDspTime;
        _initialPosition = transform.position;
        _startDspTime = AudioManager.Instance.startDspTime;
        _speed = CalculateSpeed();
        _direction = (target.position - _initialPosition).normalized;
    }

    protected float CalculateSpeed()
    {
        return Vector3.Distance(_initialPosition, target.position) / (float)(_targetDspTime - _spawnDspTime);
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
        print($"삭제 시간 : {AudioSettings.dspTime - _startDspTime:F3}, 생성 시간 : {_spawnDspTime - _startDspTime:F3}, 타겟 시간 : {_targetDspTime - _startDspTime:F3}, 오디오 소스 : {hitSound}");
        Destroy(gameObject);
    }

    public double GetTargetDspTime()
    {
        return _targetDspTime;
    }

    public abstract void Hit(NoteType noteType);
}
