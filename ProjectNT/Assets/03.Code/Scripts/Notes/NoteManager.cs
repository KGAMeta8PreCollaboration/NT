using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public List<NoteRail> noteRails = new List<NoteRail>();
    public Note shortNotePrefab;
    public Note longNotePrefab;
    public Note topNotePrefab;
    [SerializeField] private ScoreManager _scoreManager;
    public List<Note> notes { get; private set; } = new List<Note>();

    public string topNoteTag;
    private NoteGenerator _noteGenerator;


    private void Awake()
    {
        _noteGenerator = GetComponent<NoteGenerator>();
    }

    public void CreateNoteFromData(LoadedNoteData noteData)
    {
        NoteSpawnData noteSpawnData = null;
        double spawnDspTime = AudioSettings.dspTime;
        AudioClip hitSound = AudioManager.Instance.GetAudioClipAtString(noteData.noteAudioClipName);

        noteSpawnData = noteData.noteType switch
        {
            NoteType.Short => new ShortNoteSpawnData(shortNotePrefab, hitSound, spawnDspTime, noteData.time, Quaternion.identity),
            NoteType.Long => new LongNoteSpawnData(longNotePrefab, hitSound, spawnDspTime, noteData.time, noteData.endTime, Quaternion.Euler(0, 0, 0)),
            NoteType.Top => new TopNoteSpawnData(topNotePrefab, hitSound, spawnDspTime, noteData.time, Quaternion.Euler(0, 0, 0),
            GameManager.Instance.IsMulti == true ? topNoteTag : "TopNote"),
            _ => null
        };

        if (noteSpawnData != null)
            noteRails[noteData.railIndex].SpawnNote(AddNote, RemoveNote, noteSpawnData);
    }

    public void AssignNotesToSchedulers(double startDspTime)
    {
        print("AssignNotesToSchedulers LoadedNotes size : " + _noteGenerator.loadedNotes.Count);
        List<LoadedNoteData>[] railNotes = new List<LoadedNoteData>[12]
        {
            new List<LoadedNoteData>(),
            new List<LoadedNoteData>(),
            new List<LoadedNoteData>(),
            new List<LoadedNoteData>(),
            new List<LoadedNoteData>(),
            new List<LoadedNoteData>(),
            new List<LoadedNoteData>(),
            new List<LoadedNoteData>(),
            new List<LoadedNoteData>(),
            new List<LoadedNoteData>(),
            new List<LoadedNoteData>(),
            new List<LoadedNoteData>(),
        };
        foreach (LoadedNoteData noteData in _noteGenerator.loadedNotes)
        {
            LoadedNoteData noteDataCopy = new LoadedNoteData(noteData);
            noteDataCopy.time += startDspTime;
            if (noteData.noteType == NoteType.Long)
                noteDataCopy.endTime += startDspTime;
            if (noteData.noteType != NoteType.Top)
                railNotes[noteData.railIndex].Add(noteDataCopy);
            else
            {
                // print("TopNote railIndex : " + noteData.railIndex + ", size = " + railNotes.Length + ", " + noteData.noteAudioClipName);
                railNotes[noteData.railIndex].Add(noteDataCopy);
            }

        }

        for (int i = 0; i < noteRails.Count; i++)
        {
            NoteRail rail = noteRails[i];
            NoteAudioScheduler scheduler = rail.GetComponent<NoteAudioScheduler>();
            if (scheduler != null)
            {
                scheduler.Init(railNotes[i]);
            }
            else
            {
                Debug.LogError($"NoteAudioScheduler not found on rail {i}");
            }
        }
    }


    // private void OnNoteHit(Note note)
    // {
    //     if (note.judgementType == JudgementType.Bad)
    //         _scoreManager.ResetCombo();
    //     else
    //         _scoreManager.IncreaseCombo();
    //     _scoreManager.AddScore(note.judgementType);
    //     _scoreManager.ShowJudgementType(note.judgementType);
    //     _scoreManager.AddJudgeCount(note.judgementType);
    // }

    private void AddNote(Note note)
    {
        note.SetScoreManager(_scoreManager);
        if (note.gameObject.name == "TopNote(Clone)")
        {
            note.OnHit?.Invoke(note);
        }
        notes.Add(note);
    }

    private void RemoveNote(Note note)
    {
        notes.Remove(note);
    }
}
