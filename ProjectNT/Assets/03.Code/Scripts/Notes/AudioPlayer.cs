using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : Singleton<AudioPlayer>
{
	[SerializeField] private AudioSource _audioSource;

	public void Play(AudioClip clip)
	{
		_audioSource.PlayOneShot(clip);
	}
}
