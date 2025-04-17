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
    public ProjectToLoadedData projectToLoadedData;

    public NoteManager[] noteManagers;
    public NoteGenerator[] noteGenerators;
    public BeatMapData beatMapData;
    public bool skipLobby; //로비씬 없이 바로 게임 스타트 하는 개발용 변수.

    [Header("게임 씬 이름")]
    public string gameSceneName = "YKD_GameScene";

    private List<LoadedNoteData> _loadedNoteDatas = new List<LoadedNoteData>();

    //멀티 플레이를 위한 변수들
    public PhotonManager PhotonManager { get; private set; }
    public MultiGameController MultiGameController { get; private set; }
    public bool IsMulti { get; private set; }
    private List<LoadedNoteData> _player1LoadedNoteDatas = new List<LoadedNoteData>();
    private List<LoadedNoteData> _player2LoadedNoteDatas = new List<LoadedNoteData>();

    public float bpm;
    public float phase2;
    public float phase3;

    private Enums.PlayMode playMode;
    public Enums.PlayMode PlayMode
    {
        get { return playMode; }
        set
        {
            playMode = value;
        }
    }
    private Enums.Phase phase;
    public Enums.Phase Phase
    {
        get { return phase; }
        private set
        {
            phase = value;
            EffectManager.Instance.SetPhaseEffect(phase);
        }
    }
    private IEnumerator phaseEnumerator;
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

    private GameSceneMove gameSceneMove;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
            return;
        projectToLoadedData = gameObject.AddComponent<ProjectToLoadedData>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameSceneName)
        {
            OnGoToLobby += () => SceneManager.LoadScene("LobbyScene");
            GameSceneInit();
            noteGenerators[0].Init(_loadedNoteDatas);
            phaseEnumerator = PhaseTracker();

        }
        else if (scene.name == "MultiGame")
        {
            print("멀티 게임 씬");
            IsMulti = true;
            OnGoToLobby += () =>
            {
                PhotonManager.LeaveRoom();
                IsMulti = false;
            };
            MultiGameController = FindObjectOfType<MultiGameController>();
            MultiGameController.SetupAndReady(_player1LoadedNoteDatas, _player2LoadedNoteDatas);
            phaseEnumerator = PhaseTracker();
            //MultiGameSceneInit();
            //noteGenerators[0].Init(noteGenerators[0].loadedNotes);
            //noteGenerators[1].Init(noteGenerators[1].loadedNotes);
        }
    }

    private IEnumerator PhaseTracker()
    {
        // double phase1Time = AudioSettings.dspTime + phase2;
        double phase2Time = AudioSettings.dspTime + phase3;
        double phase3Time = AudioSettings.dspTime + AudioManager.Instance.BgmLength;


        gameSceneMove.mapmovePosTimes[0].travelTime = phase2;
        gameSceneMove.mapmovePosTimes[1].travelTime = phase3 - phase2;
        gameSceneMove.mapmovePosTimes[2].travelTime = AudioManager.Instance.BgmLength - phase3;
        double curr = AudioSettings.dspTime + phase2;
        List<(double, Enums.Phase)> tuple = new List<(double, Enums.Phase)>
        {
            (phase2Time, Enums.Phase.Phase2),
            (phase3Time, Enums.Phase.Phase3)
        };

        Phase = Enums.Phase.Phase1;
        gameSceneMove.MapMoveByPhase(Phase);

        while (tuple.Count != 0)
        {
            if (AudioSettings.dspTime > curr)
            {
                curr = tuple[0].Item1;
                Phase = tuple[0].Item2;
                tuple.RemoveAt(0);
                gameSceneMove.MapMoveByPhase(Phase);
            }
            yield return null;
        }
    }

    public void SingleGameStart(BeatMapData beatMapData, string projectPath, string musicName)
    {
        projectToLoadedData.GetBgmAudioClip(projectPath, musicName, AudioManager.Instance.SetBackgroundMusic);
        projectToLoadedData.GetAudioClipsToProject(projectPath, AudioManager.Instance.SetAudioClips);
        _loadedNoteDatas = projectToLoadedData.BeatMapDataToLoadedNoteData(beatMapData);
        PlayMode = Enums.PlayMode.Single;
        SceneManager.LoadScene(gameSceneName);
    }

    public IEnumerator GameSceneInitCo()
    {
        yield return new WaitForSeconds(5f);

        GameStart();
        StartCoroutine(CheckGameEndCoroutine());
    }
    public void MultiGameStart(Difficulty difficulty, BeatMapData beatMapData)
    {

    }

    //멀티 임시 시작 메서드
    public void MultiGameStart()
    {
        // 데이터

        PhotonNetwork.LoadLevel("MultiGame");
    }

    // TODO : 멀티 데이터 여기서 넘겨줍니다.
    public void SetDataForMultiGameStart(BeatMapData loMapData1, BeatMapData loMapData2, string projectPath, string musicName)
    {
        projectToLoadedData.GetBgmAudioClip(projectPath, musicName, AudioManager.Instance.SetBackgroundMusic);
        projectToLoadedData.GetAudioClipsToProject(projectPath, AudioManager.Instance.SetAudioClips);
        _player1LoadedNoteDatas = projectToLoadedData.BeatMapDataToLoadedNoteData(loMapData1);
        _player2LoadedNoteDatas = projectToLoadedData.BeatMapDataToLoadedNoteData(loMapData2);
    }

    private void GameSceneInit()
    {
        noteManagers = FindObjectsOfType<NoteManager>();
        noteGenerators = FindObjectsOfType<NoteGenerator>();

        StopCoroutine(GameSceneInitCo());
        StartCoroutine(GameSceneInitCo());
    }

    private void MultiGameSceneInit()
    {
        noteManagers = FindObjectsOfType<NoteManager>();
        noteGenerators = FindObjectsOfType<NoteGenerator>();
    }

    // TODO: 프로토타입 임시
    public void GameStart()
    {
        // print("게임매니저 게임스타트");
        AudioManager.Instance.StartBGM(delayTime);
        gameSceneMove = FindObjectOfType<GameSceneMove>();
        StartCoroutine(phaseEnumerator);
    }

    public void GoToLobby()
    {
        OnGoToLobby?.Invoke();
        OnGoToLobby = null;
        //SceneManager.LoadScene("LobbyScene");
    }

    public void GameEnd()
    {
        // print("Game End");
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
                // print("CheckGameEndCoroutine");
                GameEnd();
                yield break;
            }
            yield return null;
        }
    }

}