using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{
	public double delayTime = 2.0;
	public Action OnGameEnd; 
	
	private NoteManager[] _noteManager;
	private NoteGenerator[] _noteGenerator;
	private ResultPanel _resultPanel;
	

	private void Start()
	{
		_noteManager = FindObjectsOfType<NoteManager>();
		_noteGenerator = FindObjectsOfType<NoteGenerator>();
		_resultPanel = FindObjectOfType<ResultPanel>(true);
		_resultPanel?.gameObject.SetActive(false);
		StartCoroutine(StartCoroutine());
	}
	
	private IEnumerator StartCoroutine()
	{
		yield return new WaitForSeconds(5f);
		GameStart();
	}

	public void GameStart()
	{
		AudioManager.Instance.StartBGM(delayTime);
	}
	
	public void GameEnd()
	{
		print("Game End");
		_resultPanel?.gameObject.SetActive(true);
		OnGameEnd?.Invoke();
	}
	
	public bool CheckGameEnd()
	{
		return !_noteManager.Any(item => item.notes.Count > 0)
		       && !_noteGenerator.Any(item => item.loadedNotes.Count > 0);
	}
	
	private void Update()
	{
		if (CheckGameEnd())
		{
			GameEnd();
		}
	}
}
