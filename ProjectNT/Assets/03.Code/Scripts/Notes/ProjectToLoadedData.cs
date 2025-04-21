using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public partial class ProjectToLoadedData : MonoBehaviour
{
    private List<AudioClip> _audioClips = new List<AudioClip>();
    public AudioClip bgmAudioClip;

    public List<LoadedNoteData> BeatMapDataToLoadedNoteData(BeatMapData beatMapData)
    {
        List<LoadedNoteData> loadedNoteDatas = new List<LoadedNoteData>();

        GridSetting gridSetting = beatMapData.gridSetting;
        foreach (NodeData nodeData in beatMapData.nodes)
        {
            LoadedNoteData loadedNoteData = NodeToNoteData(nodeData, gridSetting);
            loadedNoteDatas.Add(loadedNoteData);
        }
        foreach (UpperNodeData upperNodeData in beatMapData.upperNodes)
        {
            for (int i = 0; i < upperNodeData.nodeIndexs.Count; i++)
            {
                LoadedNoteData loadedNoteData = NodeToNoteData(upperNodeData, gridSetting, upperNodeData.nodeIndexs[i] + 4, i);
                loadedNoteDatas.Add(loadedNoteData);
            }
        }

        // TODO : 임시로 여기서 phase설정해주는겁니다.
        GameManager.Instance.phase2ChangeTime = beatMapData.songData.phase2;
        GameManager.Instance.phase3ChangeTime = beatMapData.songData.phase3;
        GameManager.Instance.bpm = beatMapData.gridSetting.BPM;
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

    private static LoadedNoteData NodeToNoteData(UpperNodeData nodeData, GridSetting gridSetting, int railIndex, int nodeIndex)
    {
        LoadedNoteData loadedNoteData = new LoadedNoteData();
        loadedNoteData.noteType = NoteType.Top;
        loadedNoteData.time = 60 * nodeData.gridIndex / (gridSetting.BeatNum * gridSetting.BPM);
        loadedNoteData.endTime = 0;
        loadedNoteData.railIndex = railIndex;
        loadedNoteData.noteAudioClipName = nodeData.keySounds[nodeIndex];
        return loadedNoteData;
    }

}

// Project에서 오디오 소스 반환받기 위한 함수들
public partial class ProjectToLoadedData
{
    private int _currentLoadClipCount = 0;

    public List<AudioClip> GetAudioClipsToProject(string projectPath)
    {
        _audioClips.Clear();
        projectPath = Path.Combine(projectPath, "KeySounds");
        if (!Directory.Exists(projectPath))
            return null;
        string[] strings = Directory.GetFiles(projectPath, "*", SearchOption.AllDirectories);
        foreach (string item in strings)
        {
            try
            {
                AudioClip audioClip = WavUtility.WavToAudioClip(File.ReadAllBytes(item), Path.GetFileName(item));
                _audioClips.Add(audioClip);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        return _audioClips;
    }
    
    public AudioClip GetBgmAudioClip(string projectPath, string musicName = "MainTheme.wav")
    {
        projectPath = Path.Combine(projectPath, "bgmSaveFile", musicName);
        Debug.Log("ProjectTOLoadedData GetBgmAudioClip : " + projectPath);
        if (!File.Exists(projectPath)) 
            return null;
        return WavUtility.WavToAudioClip(File.ReadAllBytes(projectPath), Path.GetFileName(projectPath));
    }

    private IEnumerator CheckAudioClipLoad(Action<List<AudioClip>> callback, int cnt)
    {
        while (_currentLoadClipCount < cnt)
            yield return null;
        callback?.Invoke(_audioClips);
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