using System.Collections.Generic;
using UnityEngine;

public class Woofer : MonoBehaviour
{
	private List<Note> notes = new List<Note>();
	public void Hit()
	{
		Debug.Log($"{gameObject.name} Hit");
		if (notes.Count == 0)
			return;	
		notes[0].Hit();
		notes.RemoveAt(0);
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
