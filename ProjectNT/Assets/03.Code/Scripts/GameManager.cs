using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	public double delayTime = 2.0;
	public Action OnGameEnd; 
	
	public NoteManager[] noteManagers;
	public NoteGenerator[] noteGenerators;
	private ResultPanel _resultPanel;
	
	// TODO : 프로토타입용 임시 UI, 나중에 UIManager든 뭐든 뺄것
	[SerializeField] private GameObject endGameMenuPanel;
	
	public BeatMapData beatMapData;
	public AudioManager audioManager;
	
	List<LoadedNoteData> loadedNoteDatas = new List<LoadedNoteData>();
	
	private void Start()
	{
		print( "경로 : " + Application.persistentDataPath);
		// GameSceneInit();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		print("Scene Loaded : " + scene.name);
		if (scene.name == "GameScene")
		{
			print("프로토타입 씬");
			GameSceneInit();
			noteGenerators[0].Init(loadedNoteDatas);
		}
	}
	private ProjectToLoadedData _projectToLoadedData;

	public void SingleGameStart(Difficulty difficulty, BeatMapData beatMapData, string projectPath)
	{
		_projectToLoadedData = gameObject.AddComponent<ProjectToLoadedData>();
		_projectToLoadedData.GetAudioClipsToProject(projectPath, AudioManager.Instance.SetAudioClips);
		_projectToLoadedData.GetBgmAudioClip(projectPath, beatMapData.songData.songName, AudioManager.Instance.SetBackgroundMusic);
		// returnCallback 으로 AudioManager.audioClips에 넣어주면 될듯
		SceneManager.LoadScene("GameScene");
		loadedNoteDatas = _projectToLoadedData.BeatMapDataToLoadedNoteData(beatMapData);
	}
	
	private IEnumerator StartCo()
	{
		yield return new WaitForSeconds(5f);

		GameStart();
		StartCoroutine(CheckGameEndCoroutine());
	}

	private void GameSceneInit()
	{
		noteManagers = FindObjectsOfType<NoteManager>();
		noteGenerators = FindObjectsOfType<NoteGenerator>();
		_resultPanel = FindObjectOfType<ResultPanel>(true);
		_resultPanel?.gameObject.SetActive(false);
		endGameMenuPanel = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "EndGameMenuPanel");
		if (endGameMenuPanel == null)
		{
			Debug.LogError("EndGameMenuPanel not found in the scene.");
		}
		StopCoroutine(StartCo());
		StartCoroutine(StartCo());
	}

	// TODO: 프로토타입 임시
    public void GameStart()
	{
		print("게임매니저 게임스타트");
		AudioManager.Instance.StartBGM(delayTime);
		FindObjectOfType<GameSceneMove>()?.GameSceneMoveAndLightStart();
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
		return !noteManagers.Any(item => item.notes.Count > 0)
		       && noteGenerators.All(item => item.IsAllGenerated());
	}
	
	private IEnumerator CheckGameEndCoroutine()
	{
		while (true)
		{
			print("CheckGameEndCoroutine");
			if (CheckGameEnd())
			{
				GameEnd();
				yield break;
			}
			yield return null;
		}
	}

}
