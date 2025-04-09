using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteRail : MonoBehaviour
{
    [SerializeField] protected Woofer woofer;

    [HideInInspector] public NoteSpawner noteSpawner;
    protected LinkedList<Note> noteList = new LinkedList<Note>();

    protected virtual void Awake()
    {
        woofer = GetComponentInChildren<Woofer>();
    }

    protected virtual void Start()
    {
        noteSpawner = GetComponentInChildren<NoteSpawner>();
    }

    public virtual void SpawnNote(Action<Note> onAddNote, Action<Note> onNoteDestroyed, NoteSpawnData noteSpawnData)
    {
        onAddNote += note => AddNote(note);
        onNoteDestroyed += note => RemoveNote(note);
        noteSpawner.SpawnNote(onAddNote, onNoteDestroyed, (note) => { }, noteSpawnData);

    }

    public virtual void AddNote(Note note)
    {
        noteList.AddLast(note);
        // note.OnHit = null;
        note.OnHit += OnNoteHit;
        if (noteList.Count == 1)
        {
            // woofer.SetAudioClip(note.hitSound);
        }
    }

    public virtual void RemoveNote(Note note)
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
                // woofer.SetAudioClip(firstNote.hitSound);
            }
        }
    }

    public LinkedList<Note> GetNoteList()
    {
        return noteList;
    }
}
