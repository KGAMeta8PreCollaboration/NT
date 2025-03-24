using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public List<NoteRail> noteRails = new List<NoteRail>();
    public List<TopNoteRail> topNoteRails = new List<TopNoteRail>();
    public Note shortNotePrefab;
    public Note longNotePrefab;
    public Note topNotePrefab;
    [SerializeField] private ScoreManager _scoreManager;

    public List<Note> notes { get; private set; } = new List<Note>();

    public void CreateNoteFromData(LoadedNoteData noteData)
    {
        NoteSpawnData noteSpawnData = null;
        double spawnDspTime = AudioSettings.dspTime;
        AudioClip hitSound = AudioManager.Instance.GetAudioClipAtString(noteData.noteAudioClipName);

        switch (noteData.noteType)
        {
            case NoteType.Short:
                noteSpawnData = new ShortNoteSpawnData(shortNotePrefab, hitSound, spawnDspTime, noteData.time, Quaternion.identity);
                break;
            case NoteType.Long:
                noteSpawnData = new LongNoteSpawnData(longNotePrefab, hitSound, spawnDspTime, noteData.time, noteData.endTime, Quaternion.Euler(0, 90, 0));
                break;
            case NoteType.Top:
                noteSpawnData = new TopNoteSpawnData(topNotePrefab, hitSound, spawnDspTime, noteData.time, noteData.endTime, Quaternion.Euler(90, 0, 0));
                break;
        }

        if (noteSpawnData != null)
            noteRails[noteData.railIndex].SpawnNote(AddNote, RemoveNote, noteSpawnData);
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
