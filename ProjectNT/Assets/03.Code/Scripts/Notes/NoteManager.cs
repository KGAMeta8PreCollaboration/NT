using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public List<NoteRail> noteRails = new List<NoteRail>();
    public Note shortNotePrefab;
    public Note longNotePrefab;

    [SerializeField] private ScoreManager _scoreManager;

    public List<Note> notes { get; private set; } = new List<Note>();


    public void CreateNoteFromData(LoadedNoteData noteData)
    {
        NoteSpawnData noteSpawnData;
        switch (noteData.noteType)
        {
            case NoteType.Short:
                noteSpawnData = new ShortNoteSpawnData(noteData.time);
                break;
            case NoteType.Long:
                noteSpawnData = new LongNoteSpawnData(noteData.time, noteData.endTime);
                break;
        }
        double spawnDspTime = AudioSettings.dspTime;
        noteRails[noteData.railIndex].SpawnNote(AddNote, RemoveNote, shortNotePrefab,
            spawnDspTime, noteData.time, AudioManager.Instance.GetAudioClipAtString(noteData.noteAudioClipName));
    }

    private void OnNoteHit(Note note)
    {
        if (note.judgementType == JudgementType.Bad)
            _scoreManager.ResetCombo();
        else
            _scoreManager.IncreaseCombo();
        _scoreManager.AddScore(note.judgementType);
        _scoreManager.ShowJudgementType(note.judgementType);
    }

    private void AddNote(Note note)
    {
        notes.Add(note);
    }

    private void RemoveNote(Note note)
    {
        notes.Remove(note);
    }
}
