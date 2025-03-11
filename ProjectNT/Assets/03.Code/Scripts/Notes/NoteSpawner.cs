using System;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
	public Transform spawnPoint;
	public Transform target;

	public void SpawnNote(Action<Note> onAddNote, Action<Note> onNoteDestroyed, Note notePrefab, Action<Note> onNoteHit, AudioClip hitSound, double spawnDspTime, double targetDspTime)
	{
		Note newNote = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
		if (newNote == null)
			return;
		newNote.Init(target, spawnDspTime, targetDspTime, hitSound);
		onAddNote?.Invoke(newNote);
		newNote.OnHit += onNoteHit;
		newNote.OnDestroyed += onNoteDestroyed;
	}
}
