using System;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform target;

    public void SpawnNote(Action<Note> onAddNote, Action<Note> onNoteDestroyed, Action<Note> onNoteHit, NoteSpawnData noteSpawnData)
    {
        // Note newNote = Instantiate(noteSpawnData.notePrefab, spawnPoint.position, noteSpawnData.rotation);
        Note newNote = PoolManager.Instance.FindNote(noteSpawnData.notePrefab);
        NoteInit(newNote, noteSpawnData.rotation);
        if (newNote == null)
            return;
        newNote.Init(target, noteSpawnData);
        onAddNote?.Invoke(newNote);
        newNote.OnHit += onNoteHit;
        newNote.OnDestroyed += onNoteDestroyed;
    }

    private void NoteInit(Note newNote, Quaternion rotation)
    {
        newNote.transform.position = spawnPoint.position;
        newNote.transform.rotation = rotation;
    }
}
