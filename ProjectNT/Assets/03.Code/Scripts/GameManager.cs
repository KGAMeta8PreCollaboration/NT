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
    public BeatMapData beatMapData;
    public bool skipLobby; //로비씬 없이 바로 게임 스타트 하는 개발용 변수.

    [Header("게임 씬 이름")]
    public string gameSceneName = "YKD_GameScene";

    private List<LoadedNoteData> _loadedNoteDatas = new List<LoadedNoteData>();
    public PhotonManager PhotonManager { get; private set; }

    private void Start()
    {
        if (skipLobby)
        {
            GameSceneInit();
            noteGenerators[0].Init();
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        PhotonManager = GetComponentInChildren<PhotonManager>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameSceneName)
        {
            GameSceneInit();
            noteGenerators[0].Init(_loadedNoteDatas);
        }
        else if (scene.name == "MultiGame")
        {
            print("멀티 게임 씬");
            OnGoToLobby += () => PhotonManager.LeaveRoom();
            GameSceneInit();
            noteGenerators[0].Init(noteGenerators[0].loadedNotes);
            noteGenerators[1].Init(noteGenerators[1].loadedNotes);
        }
    }
    private ProjectToLoadedData _projectToLoadedData;

    public void SingleGameStart(BeatMapData beatMapData, string projectPath, string musicName)
    {
        projectToLoadedData = gameObject.AddComponent<ProjectToLoadedData>();
        projectToLoadedData.GetAudioClipsToProject(projectPath, AudioManager.Instance.SetAudioClips);
        projectToLoadedData.GetBgmAudioClip(projectPath, musicName, AudioManager.Instance.SetBackgroundMusic);
        _loadedNoteDatas = projectToLoadedData.BeatMapDataToLoadedNoteData(beatMapData);
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
        PhotonNetwork.LoadLevel("MultiGame");
    }
    //멀티 임시 시작 메서드
    public void MultiGameStart()
    {
        PhotonNetwork.LoadLevel("MultiGame");
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
        //SceneManager.LoadScene("LobbyScene");
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
