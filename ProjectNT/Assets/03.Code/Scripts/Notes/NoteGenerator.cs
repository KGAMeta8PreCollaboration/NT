using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum NoteType
{
    Short,
    Long,
    Top
}

[Serializable]
public class LoadedNoteData
{
    public NoteType noteType;
    public double time;
    public double endTime; // 롱노트에만 사용하는 변수
    public int railIndex;
    public string noteAudioClipName;
}

public class NoteGenerator : MonoBehaviour
{
    public List<LoadedNoteData> loadedNotes = new List<LoadedNoteData>();
    private List<LoadedNoteData> _loadedNotes = new List<LoadedNoteData>();
    private NoteManager _noteManager;
    private double _startDspTime;
    private double _noteLeadTime = 3.0;

    private void Awake()
    {
        _noteManager = GetComponent<NoteManager>();
    }

    public void Init(List<LoadedNoteData> loadedNotes)
    {
        this.loadedNotes = loadedNotes;
        print("NoteGenerator 시작~~~~~~~~~~");
        _loadedNotes.AddRange(loadedNotes);
        _loadedNotes.Sort((lh, rh) => lh.time.CompareTo(rh.time));
    }
    public void Init()
    {
        _loadedNotes.AddRange(loadedNotes);
        _loadedNotes.Sort((lh, rh) => lh.time.CompareTo(rh.time));
    }

    public bool IsAllGenerated()
    {
        return _loadedNotes.Count == 0;
    }

    // startTime : 현재시간 + 3초뒤
    public async void NoteGenerateStart(double startTime)
    {
        print("노트 생성기 놑트 생성 시작 1");
        try
        {
            print("노트 생성기 놑트 생성 시작 2");
            _startDspTime = AudioSettings.dspTime;
            _noteLeadTime = startTime - AudioSettings.dspTime;
            print($"NoteGenerateStart _noteLeadTime : {_noteLeadTime}");
            await CheckAndGenerateNotesAsync();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"NoteGenerator.NoteGenerateStart Error : {e.Message}");
            throw;
        }
    }

    private async Task CheckAndGenerateNotesAsync()
    {
        print("노트 생성기 비동기 생성 시작 1");
        while (Application.isPlaying && _loadedNotes.Count > 0)
        {
            double currentTime = AudioSettings.dspTime;
            LoadedNoteData noteData = _loadedNotes[0];
            if (Application.isPlaying && noteData.time <= currentTime - _startDspTime)
            {
                print("노트 생성기 비동기 생성 시작 2");
                noteData.time += _startDspTime + _noteLeadTime;
                //LoadedNoteData 구조화 전까지는 일단 사용. 롱노트에 대한 endTime부여
                if (noteData.noteType == NoteType.Long)
                    noteData.endTime += _startDspTime + _noteLeadTime;

                print("노트 생성기 비동기 생성 시작 3");
                _noteManager.CreateNoteFromData(noteData);
                _loadedNotes.RemoveAt(0);
            }
            else
            {
                await Task.Delay(1);
            }
        }
    }
}
