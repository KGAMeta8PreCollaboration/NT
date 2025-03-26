using System;
using System.Collections.Generic;
using UnityEngine;

public class Woofer : MonoBehaviour
{
    public List<Note> notes = new List<Note>();
    private AudioSource _audioSource;
    [SerializeField] private JudgementSystem _judgementSystem;
    [SerializeField] private NoteScanner _noteScanner;
    public AudioClip hitSound { get; private set; }

    public bool isHoldingLongNote = false;

    public PlayerController controller;

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
        AudioManager.Instance.Play(hitSound);

        if (notes.Count == 0)
            return;
        Note note = notes[0];
        //note.Hit(_judgementSystem.CheckTiming());
        note.Hit(_judgementSystem.JudgeNote());
        if (note is LongNote)
        {
            isHoldingLongNote = true;
        }

        controller?.InvokeHaptic(0.5f, 0.3f);
    }

    public void Hold()
    {
        if (isHoldingLongNote && notes.Count > 0)
        {
            LongNote longNote = notes[0] as LongNote;
            longNote.Hold();

            if (AudioSettings.dspTime >= longNote.endTargetDspTime)
            {
                ReleaseLongNote();
            }

            controller?.InvokeHaptic(0.3f, 0.2f);
        }
    }

    public void ReleaseLongNote()
    {
        if (isHoldingLongNote && notes.Count > 0)
        {
            LongNote longNote = notes[0] as LongNote;
            longNote.Release();
        }
        isHoldingLongNote = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            controller = playerController;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            controller = playerController;
        }
    }
}
