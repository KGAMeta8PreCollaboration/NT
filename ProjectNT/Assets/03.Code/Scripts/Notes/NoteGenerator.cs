using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class LoadedNoteData
{
	public double time;
	public int railIndex;
	public string noteAudioClipName;
}

public class NoteGenerator : MonoBehaviour
{
	public List<LoadedNoteData> loadedNotes = new List<LoadedNoteData>();
	private NoteManager _noteManager;
	private double _startDspTime;
	private double _noteLeadTime = 3.0;

	private void Awake()
	{
		_noteManager = GetComponent<NoteManager>();
	}

	private void Start()
	{
		loadedNotes.Sort((lh, rh) => lh.time.CompareTo(rh.time));
	}

	// startTime : 현재시간 + 3초뒤
	public async void NoteGenerateStart(double startTime)
	{
		try
		{
			_startDspTime = AudioSettings.dspTime;
			_noteLeadTime = startTime - AudioSettings.dspTime;
			print($"_noteLeadTime : {_noteLeadTime}");
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
		while (Application.isPlaying && loadedNotes.Count > 0)
		{
			double currentTime = AudioSettings.dspTime;
			LoadedNoteData noteData = loadedNotes[0];
			if (Application.isPlaying && noteData.time <= currentTime - _startDspTime)
			{
				noteData.time += _startDspTime + _noteLeadTime;
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
