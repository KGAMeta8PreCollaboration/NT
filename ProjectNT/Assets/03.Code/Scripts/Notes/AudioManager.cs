using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	public List<AudioClip> audioClips = new List<AudioClip>();
	int currentClipIndex = 0;
	
	public AudioClip GetRandomAudioClip()
	{
		return audioClips[Random.Range(0, audioClips.Count)];
	}
	
	public AudioClip GetNextAudioClip()
	{
		currentClipIndex %= audioClips.Count;
		// print($"AudioManager currentClipIndex : {currentClipIndex}");
		return audioClips[currentClipIndex++];
	}
}
