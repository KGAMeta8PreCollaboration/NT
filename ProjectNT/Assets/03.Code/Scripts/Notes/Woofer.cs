using System.Collections.Generic;
using UnityEngine;

public class Woofer : MonoBehaviour
{
    public List<Note> notes = new List<Note>();
    private AudioSource _audioSource;
    private JudgementSystem _judgementSystem;
    private NoteScanner _noteScanner;
    public AudioClip hitSound { get; private set; }
    
    private AudioPlayer _audioPlayer;
    

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _judgementSystem = GetComponent<JudgementSystem>();
        _audioPlayer = FindObjectOfType<AudioPlayer>();
        _judgementSystem = FindObjectOfType<JudgementSystem>();
        _noteScanner = FindObjectOfType<NoteScanner>();

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
        _audioPlayer.Play(hitSound);
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
