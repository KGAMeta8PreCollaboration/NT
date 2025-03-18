using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	public List<AudioClip> audioClips = new List<AudioClip>();
	int currentClipIndex = 0;
	public double startDspTime { get; private set; }
	public AudioSource bgmAudioSource;
	private AudioPool _audioPool;
	private List<AudioSource> _audioSources = new List<AudioSource>();
	bool isPlay = false;
	private NoteGenerator[] noteGenerators;
	
	protected override void Awake()
	{ 
		base.Awake();
		foreach (AudioClip item in audioClips)
			item.LoadAudioData();
		_audioPool = GetComponent<AudioPool>();
		StartCoroutine(CheckAudioPlayTime());
	}

	private void Start()
	{
		noteGenerators = FindObjectsOfType<NoteGenerator>(true);
	}

	public void Play(AudioClip clip)
	{
		if (_audioPool == null)
			_audioPool = GetComponent<AudioPool>();
		AudioSource audioSource = _audioPool.GetAudioSource();
		_audioSources.Add(audioSource);
		audioSource.clip = clip;
		double playTime = AudioSettings.dspTime + 0.01;
		// audioSource.Play();
		audioSource.PlayScheduled(playTime);
		// audioSource.PlayOneShot(clip);
	}
	
	private IEnumerator CheckAudioPlayTime()
	{
		while (true)
		{
			yield return new WaitForSeconds(5f);
			print($"BGMPlayTime : {bgmAudioSource.time:F3}, DSPTime : {AudioSettings.dspTime - startDspTime:F3}, dsp - bgm : {AudioSettings.dspTime - startDspTime - bgmAudioSource.time:F3}");
			// print($"BGM 레이턴시 : {(AudioSettings.dspTime - startDspTime)}, AudioSettings.dspTime : {AudioSettings.dspTime}, startDspTime : {startDspTime}");
		}
	}
	
	private void ReturnUnusedAudioSources()
	{
		_audioSources
			.FindAll(audioSource => !audioSource.isPlaying)
			.ForEach(audioSource => _audioPool.ReturnAudioSource(audioSource));
	}

	public void StartBGM(double delayTime)
	{
		startDspTime = AudioSettings.dspTime + delayTime;
		bgmAudioSource.Stop();
		// bgmAudioSource.Play((ulong)delayTime);
		bgmAudioSource.PlayScheduled(startDspTime);
		print($"BGM Sample rate : {bgmAudioSource.clip.frequency}");
		foreach (NoteGenerator generator in noteGenerators)
		{
			generator.NoteGenerateStart(startDspTime);
		}
	}
	
	public AudioClip GetAudioClipAtString(string clipName)
	{
		return audioClips.Find(clip => clip.name == clipName);
	}
	private void Update()
	{
		ReturnUnusedAudioSources();
	}
}
