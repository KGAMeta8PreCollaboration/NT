using Photon.Pun;
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
    public Action OnGoToLobby;

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
		print( "경로 : " + Application.persistentDataPath);
		// GameSceneInit();
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
	private ProjectToLoadedData _projectToLoadedData;

	public void SingleGameStart(Difficulty difficulty, BeatMapData beatMapData, string projectPath)
	{
		_projectToLoadedData = gameObject.AddComponent<ProjectToLoadedData>();
		_projectToLoadedData.GetAudioClipsToProject(projectPath, AudioManager.Instance.SetAudioClips);
		_projectToLoadedData.GetBgmAudioClip(projectPath, beatMapData.songData.songName, AudioManager.Instance.SetBackgroundMusic);
		SceneManager.LoadScene(gameSceneName);
		loadedNoteDatas = _projectToLoadedData.BeatMapDataToLoadedNoteData(beatMapData);
	}
	
	private IEnumerator StartCo()
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
