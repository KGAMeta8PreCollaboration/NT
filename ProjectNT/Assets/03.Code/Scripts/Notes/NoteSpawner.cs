using System;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
	public Transform spawnPoint;
	public Transform target;
	
	[HideInInspector] public float noteSpeed = 5.0f;

	public void SpawnNote(Action<Note> onAddNote, Action<Note> onNoteDestroyed, Note notePrefab, Action<Note> onNoteHit, AudioClip hitSound)
	{
		Note newNote = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
		if (newNote == null)
			return;
		newNote.Init(target, noteSpeed, hitSound);
		onAddNote?.Invoke(newNote);
		newNote.OnHit += onNoteHit;
		newNote.OnDestroyed += onNoteDestroyed;
	}
}
