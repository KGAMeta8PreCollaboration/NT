using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    public double delayTime = 2.0;
    public Action OnGameEnd;
    public Action OnGoToLobby;
    public ProjectToLoadedData projectToLoadedData;

    public NoteManager[] noteManagers;
    public NoteGenerator[] noteGenerators;
   
    private GamePhotonManager _gamePhotonManager;

    public BeatMapData beatMapData;

    public bool skipLobby; //로비씬 없이 바로 게임 스타트 하는 개발용 변수.

	[Header("게임 씬 이름")]
	public string gameSceneName = "YKD_GameScene";
	
	
	// TODO : 프로토타입용 임시 UI, 나중에 UIManager든 뭐든 뺄것
	[SerializeField] private GameObject endGameMenuPanel;
	List<LoadedNoteData> loadedNoteDatas = new List<LoadedNoteData>();
	
	private void Start()
	{
		if (skipLobby)
		{
			GameSceneInit();
			noteGenerators[0].Init();
		}
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		print("Scene Loaded : " + scene.name);
		if (scene.name == gameSceneName)
		{
			print("프로토타입 씬");
			GameSceneInit();
			noteGenerators[0].Init(loadedNoteDatas);
		}
		else if (scene.name == "LSH_MultiGame2")
		{
			print("멀티 게임 씬");
			GameSceneInit();
			noteGenerators[0].Init();
			noteGenerators[1].Init();
		}
	}

	public void SingleGameStart(Difficulty difficulty, BeatMapData beatMapData, string projectPath)
	{
		projectToLoadedData = gameObject.AddComponent<ProjectToLoadedData>();
		projectToLoadedData.GetAudioClipsToProject(projectPath, AudioManager.Instance.SetAudioClips);
		projectToLoadedData.GetBgmAudioClip(projectPath, beatMapData.songData.songName, AudioManager.Instance.SetBackgroundMusic);
		loadedNoteDatas = projectToLoadedData.BeatMapDataToLoadedNoteData(beatMapData);
		SceneManager.LoadScene(gameSceneName);
	}
	
	private IEnumerator GameSceneInitCo()
	{
		yield return new WaitForSeconds(5f);
		
        GameStart();
        StartCoroutine(CheckGameEndCoroutine());
    }
    public void MultiGameStart(Difficulty difficulty, BeatMapData beatMapData)
    {
        PhotonNetwork.LoadLevel("LSH_MultiGame2");
        OnGoToLobby += () => _gamePhotonManager.LeaveRoom();
    }
    //멀티 임시 시작 메서드
    public void MultiGameStart()
    {
        PhotonNetwork.LoadLevel("LSH_MultiGame2");
        OnGoToLobby += () => _gamePhotonManager.LeaveRoom();
    }

    private void GameSceneInit()
    {
        noteManagers = FindObjectsOfType<NoteManager>();
        noteGenerators = FindObjectsOfType<NoteGenerator>();

        StopCoroutine(GameSceneInitCo());
        StartCoroutine(GameSceneInitCo());
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
        OnGoToLobby?.Invoke();
        OnGoToLobby = null;
        SceneManager.LoadScene("Prototype_Lobby");
    }

    public void GameEnd()
    {
        print("Game End");
        OnGameEnd?.Invoke();
        OnGameEnd = null;
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
            if (CheckGameEnd())
            {
				print("CheckGameEndCoroutine");
                GameEnd();
                yield break;
            }
            yield return null;
        }
    }

}
