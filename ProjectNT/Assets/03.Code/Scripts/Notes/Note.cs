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

    private Vector3 _initialPosition;
    private double _spawnDspTime;
    private double _targetDspTime;

    public void Init(Transform target, double spawnDspTime, double targetDspTime, AudioClip hitSound = null)
    {
        this.target = target;
        this.hitSound = hitSound;
        _spawnDspTime = spawnDspTime;
        _targetDspTime = targetDspTime;
        _initialPosition = transform.position;
    }

    public void Hit(NoteType noteType)
    {
        Destroy();
        isHit = true;
        this.noteType = noteType;
        OnHit?.Invoke(this);
    }

    private void Destroy()
    {
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }

    private void Move()
    {
        double currentTime = AudioSettings.dspTime;
        double elapsedTime = currentTime - _spawnDspTime;
        double totalTime = _targetDspTime - _spawnDspTime;
    
        //  Mathf.Clamp01 : 0 ~ 1 로 정규화
        float timeProgress = Mathf.Clamp01((float)(elapsedTime / totalTime));
    
        if (target)
            transform.position = Vector3.Lerp(_initialPosition, target.position, timeProgress);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 판정 구역 접근
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
        Move();
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
            Destroy();
    }
}