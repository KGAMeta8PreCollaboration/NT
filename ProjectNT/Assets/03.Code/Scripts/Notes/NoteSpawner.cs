using System;
using System.Collections;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
	public Transform spawnPoint;
	public Transform target;
	public float noteSpeed = 5.0f;

	public void StartSpawning(Action<Note> onAddNote, Action<Note> onNoteDestroyed, Note notePrefab)
	{
		SpawnNote(onAddNote, onNoteDestroyed, notePrefab);
	}

	private void SpawnNote(Action<Note> onAddNote, Action<Note> onNoteDestroyed, Note notePrefab)
	{
		Note newNote = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
		if (newNote == null)
			return;
		newNote.Init(target, noteSpeed);
		onAddNote?.Invoke(newNote);
		newNote.OnDestroyed += onNoteDestroyed;
	}
}
