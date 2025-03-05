using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	public List<AudioClip> audioClips = new List<AudioClip>();
	
	public AudioClip GetRandomAudioClip()
	{
		return audioClips[Random.Range(0, audioClips.Count)];
	}
}
