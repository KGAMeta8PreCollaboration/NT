using System;
using UnityEngine;

public enum NoteType
{
    Bad,
    Good,
    Cool,
    Perfect,
}

public class Note : MonoBehaviour
{
    public Transform target { get; private set; }
    public bool isHit { get; private set; }
    public NoteType noteType { get; private set; }
    public Action<Note> OnDestroyed;
    public Action<Note> OnHit;
    public AudioClip hitSound;

    [SerializeField] private ParticleSystem hitEffect;

    private float _speed;
    private Vector3 _initialPosition;
    private double _spawnDspTime;
    private double _targetDspTime;
    private double _startDspTime;
    private Vector3 _direction;
    private bool _isTargetReached;

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

    public void Hit(NoteType noteType)
    {
        Destroy();
        isHit = true;
        this.noteType = noteType;
        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }
        OnHit?.Invoke(this);
    }

    private void Destroy()
    {
        OnDestroyed?.Invoke(this);
        print($"삭제 시간 : {AudioSettings.dspTime - _startDspTime:F3}, 노트 생성 시간 : {_spawnDspTime - _startDspTime:F3}, 노트 타겟 시간 : {_targetDspTime - _startDspTime:F3}, 오디오 소스 : {hitSound}");
        Destroy(gameObject);
    }

    private float CalculateSpeed()
    {
        return Vector3.Distance(_initialPosition, target.position) / (float)(_targetDspTime - _spawnDspTime);
    }

    private void Move()
    {
        double currentTime = AudioSettings.dspTime;
        double elapsedTime = currentTime - _spawnDspTime;
        double totalTime = _targetDspTime - _spawnDspTime;

        float timeProgress = Mathf.Clamp01((float)(elapsedTime / totalTime));
        if (target)
            transform.position = Vector3.Lerp(_initialPosition, target.position, timeProgress);
    }

    private void ContinueMoving()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    private void OnTriggerExit(Collider other)
    {
        // 판정 구역 벗어남 == 노트 미스
        if (other.CompareTag("Woofer"))
            Miss();
    }

    private void Miss()
    {
        Destroy();
        isHit = true;
        noteType = NoteType.Bad;
        OnHit?.Invoke(this);
    }

    private void Update()
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
}