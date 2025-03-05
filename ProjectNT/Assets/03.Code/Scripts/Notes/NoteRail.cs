using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteRail : MonoBehaviour
{
	[SerializeField] private Woofer woofer; 
	
	[HideInInspector] public NoteSpawner noteSpawner;
	LinkedList<Note> noteList = new LinkedList<Note>();
	
	private void Start()
	{
		noteSpawner = GetComponentInChildren<NoteSpawner>();
	}
	
	public void SpawnNote(Action<Note> onAddNote, Action<Note> onNoteDestroyed, Note notePrefab, Action<Note> onNoteHit)
	{
		onAddNote += note => AddNote(note);
		onNoteDestroyed += note => RemoveNote(note);
		AudioClip audioClip = AudioManager.Instance.GetRandomAudioClip();
		noteSpawner.SpawnNote(onAddNote, onNoteDestroyed, notePrefab, onNoteHit, audioClip);
	}

	public void AddNote(Note note)
	{
		noteList.AddLast(note);
		note.OnHit += OnNoteHit;
		if (noteList.Count == 1)
		{
			woofer.SetAudioClip(note.hitSound);
		}
	}

	public void RemoveNote(Note note)
	{
		noteList.Remove(note);
	}
	
	private void OnNoteHit(Note note)
	{
		if (noteList.Count > 0)
		{
			var firstNote = noteList.First?.Value;
			if (firstNote != null)
			{
				woofer.SetAudioClip(firstNote.hitSound);
			}
		}
	}
}
