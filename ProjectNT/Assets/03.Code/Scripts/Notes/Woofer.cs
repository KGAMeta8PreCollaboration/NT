using System.Collections.Generic;
using UnityEngine;

public class Woofer : MonoBehaviour
{
    public List<Note> notes = new List<Note>();
    private AudioSource _audioSource;
    [SerializeField] private JudgementSystem _judgementSystem;
    [SerializeField] private NoteScanner _noteScanner;
    public AudioClip hitSound { get; private set; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _noteScanner.OnNoteEnter += AddNote;
        _noteScanner.OnNoteExit += RemoveNote;

        if (hitSound)
            _audioSource.clip = hitSound;
    }

    public void SetAudioClip(AudioClip clip)
    {
        hitSound = clip;
    }

    public void Hit()
    {
        // if (_audioSource.isPlaying)
        // {
        // 	// _audioSource.Stop();
        //       }
        // if (_audioSource.clip != hitSound)
        // 	_audioSource.clip = hitSound;

        // _audioSource.PlayOneShot(hitSound);

        if (notes.Count == 0)
            return;
        AudioManager.Instance.Play(hitSound);
        Note note = notes[0];
        note.Hit(_judgementSystem.CheckTiming());
    }

    public void AddNote(Note note)
    {
        note.OnDestroyed += note => notes.Remove(note);
        notes.Add(note);
    }

    public void RemoveNote(Note note)
    {
        notes?.Remove(note);
    }
}
