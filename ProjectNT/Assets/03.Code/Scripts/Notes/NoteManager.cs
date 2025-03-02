using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{ 
	public List<NoteRail> noteRails = new List<NoteRail>();
	public int maxNoteRails = 4;
	public Note notePrefab;
	
	public List<Note> notes { get; private set; } = new List<Note>();

	private void Start()
	{
		for (int i = 0; i < maxNoteRails; i++)
			noteRails[i].SpawnNote(AddNote, RemoveNote, notePrefab);
	}

	public void CreateNote(int index)
	{
		noteRails[index].SpawnNote(AddNote, RemoveNote, notePrefab);
	}
	
	private void AddNote(Note note)
	{
		notes.Add(notePrefab);
	}
	
	private void RemoveNote(Note note)
	{
		notes.Remove(note);
	}
}
