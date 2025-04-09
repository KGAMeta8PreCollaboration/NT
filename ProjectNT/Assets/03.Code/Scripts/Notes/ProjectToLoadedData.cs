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
	public AudioClip bgmAudioClip;

	public List<LoadedNoteData> BeatMapDataToLoadedNoteData(BeatMapData beatMapData)
	{
		GridSetting gridSetting = beatMapData.gridSetting;
		foreach (NodeData nodeData in beatMapData.nodes)
		{
			LoadedNoteData loadedNoteData = NodeToNoteData(nodeData, gridSetting);
			loadedNoteDatas.Add(loadedNoteData);
		}
		foreach (UpperNodeData upperNodeData in beatMapData.upperNodes)
		{
			foreach (int nodeIndex in upperNodeData.nodeIndexs)
			{
				LoadedNoteData loadedNoteData = NodeToNoteData(upperNodeData, gridSetting, nodeIndex + 4);
				loadedNoteDatas.Add(loadedNoteData);
			}
		}
		
		return loadedNoteDatas;
	}

	private static LoadedNoteData NodeToNoteData(NodeData nodeData, GridSetting gridSetting)
	{
		return new LoadedNoteData
		{
			noteType = nodeData.nodeType == EditorNoteType.LongNote ? NoteType.Long : NoteType.Short,
			time = 60 * nodeData.index.y / (gridSetting.BeatNum * gridSetting.BPM),
			endTime = nodeData.nodeType == EditorNoteType.LongNote ? 60 * nodeData.endIndex.y / (gridSetting.BeatNum * gridSetting.BPM) : 0,
			railIndex = nodeData.index.x,
			noteAudioClipName = nodeData.keySound,
		};
	}

	private static LoadedNoteData NodeToNoteData(UpperNodeData nodeData, GridSetting gridSetting, int railIndex)
	{
		return new LoadedNoteData
		{
			noteType = NoteType.Top,
			time = 60 * nodeData.gridIndex / (gridSetting.BeatNum * gridSetting.BPM),
			endTime = 0,
			railIndex = railIndex,
			noteAudioClipName = "",
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
		print("프로젝트 겟 오디오클립 1");
		projectPath = Path.Combine(projectPath, "KeySounds");
		if (!Directory.Exists(projectPath)) return;
		print("프로젝트 겟 오디오클립 2");
		string[] strings = Directory.GetFiles(projectPath);
		List<AudioClip> res = new List<AudioClip>();
		print("프로젝트 겟 오디오클립 3");
		foreach (string item in strings)
		{
			try
			{
				AudioClip audioClip = WavUtility.WavToAudioClip(File.ReadAllBytes(item), Path.GetFileName(item));
				AddAudioClip(audioClip);
			}
			catch (ArgumentException e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		print("프로젝트 겟 오디오클립 4");

		returnCallback?.Invoke(audioClips);
		print("프로젝트 겟 오디오클립 5");
	}

	private void AddAudioClip(AudioClip clip) => audioClips.Add(clip);

	public void GetBgmAudioClip(string projectPath, string bgmName, Action<AudioClip> returnCallback)
	{
		Debug.Log("프로젝트 BGM 로드 1");
		projectPath = Path.Combine(projectPath, "bgmSaveFile", "MainTheme.wav");
		Debug.Log($"bgm 경로 : {projectPath}");
		if (!File.Exists(projectPath)) return;
		AudioClip clip = WavUtility.WavToAudioClip(File.ReadAllBytes(projectPath), Path.GetFileName(projectPath));
		Debug.Log("프로젝트 BGM 로드 2");
		bgmAudioClip = clip;
		Debug.Log("프로젝트 BGM 로드 3");
		returnCallback?.Invoke(clip);
		Debug.Log("프로젝트 BGM 로드 SetBGM 실행함");
		// StartCoroutine(BGMWebRequest(projectPath, returnCallback));
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
		path = Path.Combine("file://", path);
		UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV);
		yield return request.SendWebRequest();

		if (request.result != UnityWebRequest.Result.Success)
		{
			bool isFile = File.Exists(path);
			Debug.LogError($"파일 체크 : {isFile}, SFX Error loading audio clip: {request.error}, {path}");
			yield break;
		}
		clip = DownloadHandlerAudioClip.GetContent(request);
		clip.name = Path.GetFileName(path);
		callback?.Invoke(clip);
		_currentLoadClipCount++;
	}

	private IEnumerator BGMWebRequest(string path, Action<AudioClip> callback)
	{
		AudioClip clip = null;
		UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV);
		yield return request.SendWebRequest();

		if (request.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError($"BGM Error loading audio clip: {request.error}, {path}");
			yield break;
		}
		clip = DownloadHandlerAudioClip.GetContent(request);
		clip.name = Path.GetFileName(path);
		bgmAudioClip = clip;
		callback?.Invoke(clip);
	}

}