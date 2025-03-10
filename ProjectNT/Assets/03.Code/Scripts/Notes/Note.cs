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
    public float speed { get; private set; }
    public bool isHit { get; private set; }
    public NoteType noteType { get; private set; }

    public Action<Note> OnDestroyed;
    public Action<Note> OnHit;

    public AudioClip hitSound;

    public void Init(Transform target, float speed, AudioClip hitSound = null)
    {
        this.target = target;
        this.speed = speed;
        this.hitSound = hitSound;
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
        if (target)
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // print("트리거 진입");
        // 판정 구역 접근
    }

    private void OnTriggerExit(Collider other)
    {
        // print("트리거 나감");
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

    private void FixedUpdate()
    {
        Move();
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
            Destroy();
    }
}
