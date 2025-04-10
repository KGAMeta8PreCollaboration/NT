using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NoteAudioScheduler : MonoBehaviour
{
	private Woofer _woofer;
	private List<(double time, AudioClip clip)> _scheduledAudioData = new List<(double, AudioClip)>();

	private void Start()
	{
		_woofer = GetComponentInChildren<Woofer>();
		Debug.Log("NoteAudioScheduler Awake");
	}

	private List<(double time, AudioClip clip)> LoadedNoteDataToTuple(List<LoadedNoteData> sortedNotes)
	{ 
		List<(double time, AudioClip clip)> ret = new List<(double, AudioClip)>();

		foreach (LoadedNoteData note in sortedNotes)
		{
			AudioClip clip = AudioManager.Instance.GetAudioClipAtString(note.noteAudioClipName);
			if (clip == null)
				continue;
			ret.Add((note.time, clip));
			if (note.noteType == NoteType.Long)
				ret.Add((note.endTime, clip));
		}
		return ret;
	}

	public void Init(List<LoadedNoteData> sortedNotes)
	{
		print("NoteAudioScheduler Init, sortedNotes.size() = " + sortedNotes.Count);
		_scheduledAudioData.Clear();
		_scheduledAudioData = LoadedNoteDataToTuple(sortedNotes);
		print("_scheduledAudioData size : " + _scheduledAudioData.Count);
		if (_scheduledAudioData.Count > 0)
			_woofer.SetAudioClip(_scheduledAudioData[0].clip);
	}
	
	private void Update()
	{
		if (_scheduledAudioData.Count == 0) 
			return;

		double currentTime = AudioSettings.dspTime;

		if (_scheduledAudioData.Count <= 1)
			return;
		var currData = _scheduledAudioData[0];
		var nextData = _scheduledAudioData[1];

		double timeDiff = nextData.time - currData.time;
		double normalizedTime = (currentTime - currData.time) / timeDiff; 
		double transitionPoint = 0.2;
			
		if (normalizedTime >= transitionPoint)
		{
			_woofer.SetAudioClip(nextData.clip);
			// AudioManager.Instance.SetBackgroundMusic(nextData.clip);
			// AudioManager.Instance.StartBGM(0.1f);
			
			// print(nextData.clip.name);
			_scheduledAudioData.RemoveAt(0);
		}
	}
}
