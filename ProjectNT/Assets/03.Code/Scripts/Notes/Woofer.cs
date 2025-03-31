using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Woofer : MonoBehaviour
{
    public List<Note> notes = new List<Note>();
    private AudioSource _audioSource;
    [SerializeField] private JudgementSystem _judgementSystem;
    [SerializeField] private NoteScanner _noteScanner;
    public AudioClip hitSound { get; private set; }

    public bool isHoldingLongNote = false;

    //Test
    public TextMeshProUGUI logText2;
    public TmpCreateNotes tmp;
    private void Awake()
    {
        //Test
        logText2 = GameObject.Find("LogText2")?.GetComponent<TextMeshProUGUI>();
        tmp = FindObjectOfType<TmpCreateNotes>();

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
    }

    public void Hold()
    {
        if (isHoldingLongNote && notes.Count > 0)
        {
            if (notes[0] is LongNote)
            {
                LongNote longNote = notes[0] as LongNote;
                if (!longNote.isEnd)
                    longNote.Hold(transform);

                if (AudioSettings.dspTime >= longNote.endTargetDspTime)
                {
                    ReleaseLongNote();
                    tmp.count++;
                    logText2.text = $"우퍼의 Release호출({tmp.count})";
                }
            }
        }
    }

    public void ReleaseLongNote()
    {
        if (isHoldingLongNote && notes.Count > 0)
        {
            if (notes[0] is LongNote)
            {
                LongNote longNote = notes[0] as LongNote;
                longNote.Release();
            }
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
}
