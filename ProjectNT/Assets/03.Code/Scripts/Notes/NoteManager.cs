using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{ 
    public List<NoteRail> noteRails = new List<NoteRail>();
    public int maxNoteRails = 4; 
    public Note notePrefab; 
    [SerializeField] private ScoreManager _scoreManager;

    public List<Note> notes { get; private set; } = new List<Note>();


    public void CreateNoteFromData(LoadedNoteData noteData)
    {
        double spawnDspTime = AudioSettings.dspTime;
        noteRails[noteData.railIndex].SpawnNote(AddNote, RemoveNote, notePrefab, 
            OnNoteHit, spawnDspTime, noteData.time, AudioManager.Instance.GetAudioClipAtString(noteData.noteAudioClipName));
    }
    
    private void OnNoteHit (Note note)
    {
        if (note.noteType == NoteType.Bad)
            _scoreManager.ResetCombo();
        else
            _scoreManager.IncreaseCombo();
        _scoreManager.AddScore(note.noteType);
        _scoreManager.ShowJudgementType(note.noteType);
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
