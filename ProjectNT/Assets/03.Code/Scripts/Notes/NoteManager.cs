using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{ 
	public List<NoteRail> noteRails = new List<NoteRail>();
	public int maxNoteRails = 4;
	public Note notePrefab;

	// 게임이 끝날때 macCombo * 100을 score에 더해주는 로직이 필요함
	// ScoreManager로 빼도 될듯

	[SerializeField] private ScoreManager _scoreManager;
	
	public List<Note> notes { get; private set; } = new List<Note>();

	public void CreateNote(int index)
	{
		noteRails[index].SpawnNote(AddNote, RemoveNote, notePrefab,
			(note) =>
		{
			if (note.noteType == NoteType.Bad)
				_scoreManager.ResetCombo();
			else
				_scoreManager.IncreaseCombo();
			_scoreManager.AddScore(note.noteType);
		});
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
