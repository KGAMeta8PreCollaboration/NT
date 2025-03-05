using System.Collections.Generic;
using UnityEngine;

public class Woofer : MonoBehaviour
{
	private List<Note> notes = new List<Note>();
	private AudioSource _audioSource;
	private AudioClip hitSound;
	private bool isClipChanged = false;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		if (hitSound)
			_audioSource.clip = hitSound;
	}

	public void SetAudioClip(AudioClip clip)
	{
		hitSound = clip;
		isClipChanged = true;
	}

	public void Hit()
	{
		if (!_audioSource.isPlaying)
		{
			_audioSource.Play();
		}

		if (notes.Count == 0)
			return;

		notes[0].Hit();
		notes.RemoveAt(0);
	}

	private void Update()
	{
		if (!_audioSource.isPlaying && isClipChanged)
		{
			_audioSource.clip = hitSound;
			isClipChanged = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out Note note))
		{
			notes.Add(note);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent(out Note note))
		{
			notes.Remove(note);
		}
	}
}
