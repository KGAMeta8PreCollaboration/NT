using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	public double delayTime = 2.0;
	public Action OnGameEnd; 
	
	private NoteManager[] _noteManager;
	private NoteGenerator[] _noteGenerator;
	private ResultPanel _resultPanel;
	
	// TODO : 프로토타입용 임시 UI, 나중에 UIManager든 뭐든 뺄것
	[SerializeField] private GameObject endGameMenuPanel;
	

	private void Start()
	{
		GameSceneInit();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Prototype_Game")
		{
			GameSceneInit();
			// 특정 씬이 로드될 때 수행할 행동들
		}
	}

	private void GameSceneInit()
	{
		_noteManager = FindObjectsOfType<NoteManager>();
		_noteGenerator = FindObjectsOfType<NoteGenerator>();
		_resultPanel = FindObjectOfType<ResultPanel>(true);
		_resultPanel?.gameObject.SetActive(false);
		endGameMenuPanel = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "EndGameMenuPanel");
		StopCoroutine(StartCoroutine());
		StartCoroutine(StartCoroutine());
	}
	
	private IEnumerator StartCoroutine()
	{
		yield return new WaitForSeconds(5f);
		GameStart();
	}

	// TODO: 프로토타입 임시
    public void GameStart()
	{
		AudioManager.Instance.StartBGM(delayTime);
		FindObjectOfType<GameSceneMove>().GameSceneMoveAndLightStart();
	}
	
	public void GoToLobby()
	{
		SceneManager.LoadScene("Prototype_Lobby");
	}
	
	public void GameEnd()
	{
		print("Game End");
		_resultPanel?.gameObject.SetActive(true);
		endGameMenuPanel?.SetActive(true);
		OnGameEnd?.Invoke();
	}
	
	public bool CheckGameEnd()
	{
		return !_noteManager.Any(item => item.notes.Count > 0)
		       && _noteGenerator.All(item => item.IsAllGenerated());
	}
	
	private void Update()
	{
		if (CheckGameEnd())
		{
			GameEnd();
		}
	}
}
