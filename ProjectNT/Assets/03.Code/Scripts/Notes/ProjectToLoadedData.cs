using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public partial class ProjectToLoadedData : MonoBehaviour
{
	public List<LoadedNoteData> loadedNoteDatas = new List<LoadedNoteData>();
	public List<AudioClip> audioClips = new List<AudioClip>();

	public List<LoadedNoteData> BeatMapDataToLoadedNoteData(BeatMapData beatMapData)
	{
		GridSetting gridSetting = beatMapData.gridSetting;
		foreach (NodeData nodeData in beatMapData.nodes)
		{
			LoadedNoteData loadedNoteData = NodeToNoteData(nodeData, gridSetting);
			loadedNoteDatas.Add(loadedNoteData);
		}
		return loadedNoteDatas;
	}

	private static LoadedNoteData NodeToNoteData(NodeData nodeData, GridSetting gridSetting)
	{
		return new LoadedNoteData
		{
			noteType = nodeData.nodeType == EditorNoteType.LongNote ? NoteType.Long : NoteType.Short,
			time = 60 * nodeData.index.y / (gridSetting.BeatNum * gridSetting.BPM),
			endTime = 0,
			railIndex = nodeData.index.x,
			noteAudioClipName = nodeData.keySound,
		};
	}

}

// Project에서 오디오 소스 반환받기 위한 함수들
public partial class ProjectToLoadedData
{
	private int _currentLoadClipCount = 0;

	// Projects/{ProjectName}/KeySounds
	public void GetAudioClipsToProject(string projectPath, Action<List<AudioClip>> returnCallback)
	{
		projectPath = Path.Combine(projectPath, "KeySounds");
		if (!Directory.Exists(projectPath)) return;
		string[] strings = Directory.GetFiles(projectPath);
		List<AudioClip> res = new List<AudioClip>();
		foreach (string item in strings)
			StartCoroutine(AudioWebRequest(item, AddAudioClip));
		StartCoroutine(CheckAudioClipLoad(returnCallback, strings.Length));
	}

	private void AddAudioClip(AudioClip clip) => audioClips.Add(clip);

	public void GetBgmAudioClip(string projectPath, string bgmName, Action<AudioClip> returnCallback)
	{
		projectPath = Path.Combine(projectPath, "bgmSaveFile", bgmName);
		if (!File.Exists(projectPath)) return;
		StartCoroutine(AudioWebRequest(projectPath, returnCallback));
	}

	private IEnumerator CheckAudioClipLoad(Action<List<AudioClip>> callback, int cnt)
	{
		while (_currentLoadClipCount < cnt)
			yield return null;
		callback?.Invoke(audioClips);
	}

	private IEnumerator AudioWebRequest(string path, Action<AudioClip> callback)
	{
		AudioClip clip = null;
		UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV);
		yield return request.SendWebRequest();

		if (request.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError($"Error loading audio clip: {request.error}");
			yield break;
		}
		clip = DownloadHandlerAudioClip.GetContent(request);
		clip.name = Path.GetFileName(path);
		callback?.Invoke(clip);
		_currentLoadClipCount++;
	}

}