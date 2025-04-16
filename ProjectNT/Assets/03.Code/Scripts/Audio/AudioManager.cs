using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
	public List<AudioClip> audioClips = new List<AudioClip>();
	private int currentClipIndex = 0;
	public double startDspTime { get; private set; }
	[SerializeField] private AudioSource _bgmAudioSource;
	private AudioPool _audioPool;
	private List<AudioSource> _audioSources = new List<AudioSource>();
	private NoteGenerator[] noteGenerators;
	public float BgmLength
	{
		get { return _bgmAudioSource.clip.length; }
	}
	protected override void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		base.Awake();
		_audioPool = GetComponent<AudioPool>();
		StartCoroutine(CheckAudioPlayTime());
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		_bgmAudioSource?.Stop();
	}

	private void Start()
	{
		noteGenerators = FindObjectsOfType<NoteGenerator>(true);
	}

	public void Play(AudioClip clip, Transform transform)
	{
		if (_audioPool == null)
			_audioPool = GetComponent<AudioPool>();

		AudioSource audioSource = _audioPool.GetAudioSource();
		audioSource.transform.position = transform.position;
		_audioSources.Add(audioSource);
		audioSource.clip = clip;

		double playTime = AudioSettings.dspTime + 0.01;
		// audioSource.Play();
		audioSource.PlayScheduled(playTime);
		// audioSource.PlayOneShot(clip);
	}

	public void Play(string clipName, Transform transform)
	{
		Play(GetAudioClipAtString(clipName), transform);
	}

	private IEnumerator CheckAudioPlayTime()
	{
		while (true)
		{
			yield return new WaitForSeconds(5f);
			// print($"BGMPlayTime : {bgmAudioSource.time:F3}, DSPTime : {AudioSettings.dspTime - startDspTime:F3}, dsp - bgm : {AudioSettings.dspTime - startDspTime - bgmAudioSource.time:F3}");
			// print($"BGM 레이턴시 : {(AudioSettings.dspTime - startDspTime)}, AudioSettings.dspTime : {AudioSettings.dspTime}, startDspTime : {startDspTime}");
		}
	}

	private void ReturnUnusedAudioSources()
	{
		_audioSources
			.FindAll(audioSource => !audioSource.isPlaying)
			.ForEach(audioSource =>
			{
				_audioPool.ReturnAudioSource(audioSource);
				_audioSources.Remove(audioSource);
			});
	}

	public void StartBGM(double delayTime)
	{
		print("오디오 매니저 StartBGM 1");
		startDspTime = AudioSettings.dspTime + delayTime;
		print("오디오 매니저 StartBGM 2");
		_bgmAudioSource.Stop();
		print("오디오 매니저 StartBGM 3");
		// bgmAudioSource.Play((ulong)delayTime);
		_bgmAudioSource.PlayScheduled(startDspTime);
		print("오디오 매니저 StartBGM 4");
		print($"BGM Sample rate : {_bgmAudioSource.clip}");
		foreach (NoteGenerator generator in GameManager.Instance.noteGenerators)
		{
			print("오디오 매니저 StartBGM 5");
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

	public void SetAudioClips(List<AudioClip> clips)
	{
		audioClips = clips;
		foreach (AudioClip item in audioClips)
		{
			item.LoadAudioData();
		}
	}

	public void SetBackgroundMusic(AudioClip clip)
	{
		print("오디오매니저 SetBackgroundMusic");
		_bgmAudioSource.clip = clip;
	}
}
