using System;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform target;

    public void SpawnNote(Action<Note> onAddNote, Action<Note> onNoteDestroyed, Action<Note> onNoteHit, NoteSpawnData noteSpawnData)
    {
        Note newNote = Instantiate(noteSpawnData.notePrefab, spawnPoint.position, noteSpawnData.rotation);
        if (newNote == null)
            return;
        newNote.Init(target, noteSpawnData);
        onAddNote?.Invoke(newNote);
        newNote.OnHit += onNoteHit;
        newNote.OnDestroyed += onNoteDestroyed;
    }
}
