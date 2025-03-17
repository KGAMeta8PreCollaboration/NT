using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioPlayer : Singleton<AudioPlayer>
{
	[SerializeField] private AudioSource _bgmAudioSource;
	
	private AudioPool _audioPool;
	private List<AudioSource> _audioSources = new List<AudioSource>();
	
	private void Awake()
	{
		_audioPool = GetComponent<AudioPool>();
		
		bgmStartTime = AudioSettings.dspTime;
		_bgmAudioSource.PlayScheduled(AudioSettings.dspTime);
		StartCoroutine(CheckAudioPlayTime());
	}
	
	private IEnumerator CheckAudioPlayTime()
	{
		while (true)
		{
			yield return new WaitForSeconds(5f);
			print($"BGMPlayTime : {_bgmAudioSource.time:F3}, DSPTime : {AudioSettings.dspTime - bgmStartTime:F3}, dsp - bgm : {AudioSettings.dspTime - bgmStartTime - _bgmAudioSource.time:F3}");
			// print($"BGM 레이턴시 : {(AudioSettings.dspTime - bgmStartTime)}, AudioSettings.dspTime : {AudioSettings.dspTime}, bgmStartTime : {bgmStartTime}");
		}
	}
	
	public void Play(AudioClip clip)
	{
		AudioSource audioSource = _audioPool.GetAudioSource();
		_audioSources.Add(audioSource);
		audioSource.clip = clip;
		double playTime = AudioSettings.dspTime + 0.01; // 현재 시간보다 약간 뒤에 실행
		audioSource.PlayScheduled(playTime);
		// _audioSource.PlayOneShot(clip);
	}
	
	private void ReturnUnusedAudioSources()
	{
		_audioSources
			.FindAll(audioSource => !audioSource.isPlaying)
			.ForEach(audioSource => _audioPool.ReturnAudioSource(audioSource));
	}
	private bool IsPlaying(AudioSource audioSource)
	{
		return audioSource.isPlaying;
	}
	
	bool isPlay = false;
	private double bgmStartTime;

	private void SearchBGMPlayTime()
	{	
		if (!isPlay && IsPlaying(_bgmAudioSource))
		{
			isPlay = true;
			print($"BGM 레이턴시 : {(AudioSettings.dspTime - bgmStartTime)}, AudioSettings.dspTime : {AudioSettings.dspTime}, bgmStartTime : {bgmStartTime}");
		}
	}

	private void Update()
	{
		ReturnUnusedAudioSources();
		SearchBGMPlayTime();
	}
}
