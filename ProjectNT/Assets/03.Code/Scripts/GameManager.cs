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
    public Action OnGoToLobby;

    private NoteManager[] _noteManager;
    private NoteGenerator[] _noteGenerator;
    private GamePhotonManager _gamePhotonManager;
    //private ResultPanel _resultPanel;

    //// TODO : 프로토타입용 임시 UI, 나중에 UIManager든 뭐든 뺄것
    //[SerializeField] private GameObject endGameMenuPanel;


    public BeatMapData beatMapData;

    [Header("로비없이 게임 시작하려면 체크")]
    public bool skipLobby; //로비씬 없이 바로 게임 스타트 하는 개발용 변수.

    private void Start()
    {
        print("경로 : " + Application.persistentDataPath);
        if (skipLobby) GameSceneInit();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("Scene Loaded : " + scene.name);
        if (scene.name == "GameScene" || scene.name == "LSH_MultiGame")
        {
            //print("프로토타입 씬");
            GameSceneInit();

            // 특정 씬이 로드될 때 수행할 행동들
        }
    }

    public void SingleGameStart(Difficulty difficulty, BeatMapData beatMapData)
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MultiGameStart(Difficulty difficulty, BeatMapData beatMapData)
    {
        SceneManager.LoadScene("LSH_MultiGame");
        OnGoToLobby += () => _gamePhotonManager.LeaveRoom();
    }

    private void GameSceneInit()
    {
        _noteManager = FindObjectsOfType<NoteManager>();
        _noteGenerator = FindObjectsOfType<NoteGenerator>();
        _gamePhotonManager = FindObjectOfType<GamePhotonManager>();
        //_resultPanel = FindObjectOfType<ResultPanel>(true);
        //_resultPanel?.gameObject.SetActive(false);
        //endGameMenuPanel = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "EndGameMenuPanel");
        StopCoroutine(StartCoroutine());
        StartCoroutine(StartCoroutine());
    }

    private IEnumerator StartCoroutine()
    {
        FindObjectOfType<GameSceneMove>()?.GameSceneMoveAndLightStart();
        yield return new WaitForSeconds(5f);
        GameStart();
        StartCoroutine(CheckGameEndCoroutine());
    }

    // TODO: 프로토타입 임시
    public void GameStart()
    {
        AudioManager.Instance.StartBGM(delayTime);
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
        //_resultPanel?.gameObject.SetActive(true);
        //endGameMenuPanel?.SetActive(true);
        OnGameEnd?.Invoke();
        OnGameEnd = null;
    }

    public bool CheckGameEnd()
    {
        return !_noteManager.Any(item => item.notes.Count > 0)
               && _noteGenerator.All(item => item.IsAllGenerated());
    }

    private IEnumerator CheckGameEndCoroutine()
    {
        while (true)
        {
            //print("CheckGameEndCoroutine");
            if (CheckGameEnd())
            {
                GameEnd();
                yield break;
            }
            yield return null;
        }
    }

}
