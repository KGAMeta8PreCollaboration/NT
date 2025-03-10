using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class LoadedNoteData
{
    public double time;
    public int railIndex;
    public int audioIndex;
}

public class NoteGenerator : MonoBehaviour
{
    public List<LoadedNoteData> loadedNotes = new List<LoadedNoteData>();
    private NoteManager _noteManager;
    private double _startDspTime;

    private void Awake()
    {
        _noteManager = GetComponent<NoteManager>();
    }

    private async void Start()
    {
        loadedNotes.Sort((lh, rh) => lh.time.CompareTo(rh.time));
        await CheckAndGenerateNotesAsync();
    }

    private async Task CheckAndGenerateNotesAsync()
    {
        _startDspTime = AudioSettings.dspTime;
        while (Application.isPlaying && loadedNotes.Count > 0)
        {
            double currentTime = AudioSettings.dspTime;
            var noteData = loadedNotes[0];
            
            if (Application.isPlaying && noteData.time <= currentTime - _startDspTime)
            {
                print($"시작시간 : {_startDspTime} 현재시간 : {currentTime}, 생성시간 : {noteData.time}, 차이 : {currentTime - _startDspTime}");
                _noteManager.CreateNoteFromData(noteData);
                loadedNotes.RemoveAt(0);
            }
            else
            {
                await Task.Delay(1);
            }
        }
    }
    
    

}
