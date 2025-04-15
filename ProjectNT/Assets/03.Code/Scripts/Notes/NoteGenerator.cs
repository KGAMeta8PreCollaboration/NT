using Photon.Pun;
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

    public LoadedNoteData()
    {
        noteType = NoteType.Short;
        time = 0;
        endTime = 0;
        railIndex = 0;
        noteAudioClipName = "";
    }

    public LoadedNoteData(LoadedNoteData noteData)
    {
        noteType = noteData.noteType;
        time = noteData.time;
        endTime = noteData.endTime;
        railIndex = noteData.railIndex;
        noteAudioClipName = noteData.noteAudioClipName;
    }
}

public class NoteGenerator : MonoBehaviour
{
    public List<LoadedNoteData> loadedNotes = new List<LoadedNoteData>();
    private List<LoadedNoteData> _loadedNotes = new List<LoadedNoteData>();
    private NoteManager _noteManager;
    private double _startDspTime;
    private double _noteLeadTime = 3.0;

    public double startDspTime { get => _startDspTime + _noteLeadTime; }

    public int tempCount = 0;

    private void Awake()
    {
        _noteManager = GetComponent<NoteManager>();
    }

    public void Init(List<LoadedNoteData> loadedNotes)
    {
        print("NoteGenerator 시작~~~~~~~~~~");
        this.loadedNotes = loadedNotes;
        // for (int i = 0; i < loadedNotes.Count; i++)
        // {
        //     if (loadedNotes[i].noteType == NoteType.Top)
        //     {
        //         print($"NOTEINIT Top : {loadedNotes[i].noteAudioClipName}");
        //     }
        //     else if (loadedNotes[i].noteType == NoteType.Long)
        //     {
        //         print($"NOTEINIT Long : {loadedNotes[i].noteAudioClipName}");
        //     }
        //     else
        //     {
        //         print($"NOTEINIT Short : {loadedNotes[i].noteAudioClipName}");
        //     }
        // }

        Init();
    }

    public void Init()
    {
        loadedNotes.Sort((lh, rh) => lh.time.CompareTo(rh.time));
        _loadedNotes.AddRange(loadedNotes);
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
            _noteManager.AssignNotesToSchedulers(_startDspTime + _noteLeadTime);

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
        print($"노트 갯수 : {_loadedNotes.Count}");
        while (Application.isPlaying && _loadedNotes.Count > 0)
        {
            double currentTime = AudioSettings.dspTime;
            LoadedNoteData noteData = _loadedNotes[0];
            //DebuggingTestHost("노트제너레이터 밖");
            if (Application.isPlaying && noteData.time <= currentTime - _startDspTime)
            {
                // print("노트 생성기 비동기 생성 시작 2");
                noteData.time += _startDspTime + _noteLeadTime;
                //LoadedNoteData 구조화 전까지는 일단 사용. 롱노트에 대한 endTime부여
                if (noteData.noteType == NoteType.Long)
                    noteData.endTime += _startDspTime + _noteLeadTime;

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
