using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteRail : MonoBehaviour
{
	[HideInInspector] public NoteSpawner noteSpawner;
	public List<Note> notes { get; private set; } = new List<Note>();

	private void Start()
	{
		noteSpawner = GetComponentInChildren<NoteSpawner>();
	}

	public void SpawnNote(Action<Note> onAddNote, Action<Note> onNoteDestroyed, Note notePrefab, Action<Note> onNoteHit)
	{
		onAddNote += note => AddNote(note);
		onNoteDestroyed += note => RemoveNote(note);
		noteSpawner.StartSpawning(onAddNote, onNoteDestroyed, notePrefab, onNoteHit);
	}

	public void AddNote(Note note)
	{
		notes.Add(note);
	}

	public void RemoveNote(Note note)
	{
		notes.Remove(note);
	}
}
