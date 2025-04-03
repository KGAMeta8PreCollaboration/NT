using UnityEngine;
using System.Collections.Generic;

public class AudioPool : MonoBehaviour
{
	public GameObject audioSourcePrefab;
	public int poolSize = 10;
	
	private Queue<AudioSource> audioSourcePool = new Queue<AudioSource>();
	
	private Transform poolParent;

	void Start()
	{
		poolParent = new GameObject("AudioSourcePool").transform;
		poolParent.SetParent(transform);
		for (int i = 0; i < poolSize; i++)
		{
			GameObject audioObj = Instantiate(audioSourcePrefab, poolParent, false); 
			audioObj.SetActive(false);
			audioSourcePool.Enqueue(audioObj.GetComponent<AudioSource>());
		}
	}

	// 오디오 소스를 풀에서 가져오는 함수
	public AudioSource GetAudioSource()
	{
		if (audioSourcePool.Count > 0)
		{
			AudioSource audioSource = audioSourcePool.Dequeue();
			audioSource.gameObject.SetActive(true);
			return audioSource;
		}
		else
		{
			GameObject audioObj = Instantiate(audioSourcePrefab);
			return audioObj.GetComponent<AudioSource>();
		}
	}

	// 사용 후 오디오 소스를 풀에 반환하는 함수
	public void ReturnAudioSource(AudioSource audioSource)
	{
		audioSource.Stop();
		audioSource.gameObject.SetActive(false);
		audioSourcePool.Enqueue(audioSource);
	}
	
	private void OnDestroy()
	{
		foreach (AudioSource source in audioSourcePool)
		{
			if (source != null)
			{
				source.clip = null;
				Destroy(source);
			}
		}
		audioSourcePool.Clear();
		Resources.UnloadUnusedAssets();
	}
}
