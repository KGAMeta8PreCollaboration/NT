using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
	public List<AudioClip> audioClips = new List<AudioClip>();
	int currentClipIndex = 0;

	private void Start()
	{
		// AudioSetting();
		print(AudioSettings.GetConfiguration().speakerMode);
		print(AudioSettings.GetConfiguration().sampleRate);
		print(AudioSettings.GetConfiguration().numRealVoices);
		print(AudioSettings.GetConfiguration().numVirtualVoices);
		print(AudioSettings.GetConfiguration().dspBufferSize);

		// printBuffer();
	}
	private static void AudioSetting()
	{
		print(AudioSettings.GetConfiguration());
		var config = AudioSettings.GetConfiguration();
		// config.speakerMode = AudioSpeakerMode.Stereo;
		// config.sampleRate = 25600;
		// config.numRealVoices = 64;
		// config.numVirtualVoices = 64;
		config.dspBufferSize = 128;
		AudioSettings.Reset(config);
	}
	
	private static void printBuffer()
	{
		AudioSettings.GetDSPBufferSize(out int bufferLength, out int numBuffers);
		print($"Buffer Length : {bufferLength}, Num Buffers : {numBuffers}");
	}

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
