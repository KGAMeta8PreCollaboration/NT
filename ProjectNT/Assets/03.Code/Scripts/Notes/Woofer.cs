using System.Collections.Generic;
using UnityEngine;

public class Woofer : MonoBehaviour
{
    public List<Note> notes = new List<Note>();
    private AudioSource _audioSource;
    private JudgementSystem _judgementSystem;
    public AudioClip hitSound { get; private set; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _judgementSystem = GetComponent<JudgementSystem>();
        if (hitSound)
            _audioSource.clip = hitSound;
    }

    public void SetAudioClip(AudioClip clip)
    {
        hitSound = clip;
    }


    public void Hit()
    {
        // 타격음은 무조건 재생해야되니까
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        if (_audioSource.clip != hitSound)
            _audioSource.clip = hitSound;

        _audioSource.Play();

        if (notes.Count == 0)
            return;

        notes[0].Hit(_judgementSystem.CheckTiming());
        notes.RemoveAt(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Note note))
        {
            notes.Add(note);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Note note))
        {
            notes.Remove(note);
        }
    }
}
