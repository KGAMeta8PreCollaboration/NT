using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameRankingUI : MonoBehaviour
{
    public LocalSaveManager localSaveManager;

    public TextMeshProUGUI lastUpdataTimeText;
    public GameObject rankingBarPrefab;
    public RectTransform contentArea;
    public GameObject loadingPanel;

    public float timerTime;
    public Image timer;

    public Button lobbyButton;
    public Button musicSelectButton;
    public Button replayButton;

    public Action lobbyUIActuon = null;
    public Action musicSelectUIActuon = null;
    public Action replayActuon = null;

    private List<GameObject> rankingBarUIs = new List<GameObject>();

    private Coroutine timerCorutine = null;

    public int newDataNumber;

    private void Awake()
    {
        replayButton.onClick.AddListener(ReplayButton);
    }

    private void OnEnable()
    {
        AddEventListeners();//이벤트 등록
    }

    private void OnDisable()
    {
        RemoveEventListeners();//이벤트 전부 지우기
    }

    public void AddEventListeners()//활성화시 초기화
    {
        StartTimer();//타이머 시작
        RankingBoardUIUpdate();//랭킹바에 Prefab 생성
        lobbyButton.onClick.AddListener(LobbyButton);//로비로 이동 버튼
        musicSelectButton.onClick.AddListener(MusicSelectButton);//곡 선택창으로 이동
    }

    public void RemoveEventListeners()//비활성화시 이벤트 전부 제거
    {
        StopTimer();//타이머 종료
        RankingBarUIDestroy();//랭킹바에 Prefab 전부 제거
        lobbyButton.onClick.RemoveListener(LobbyButton);
        musicSelectButton.onClick.RemoveListener(MusicSelectButton);
        lobbyUIActuon = null;//액션안에 있는거 제거(혹시 모를 중복 방지)
        musicSelectUIActuon = null;//액션안에 있는거 제거(혹시 모를 중복 방지)
        replayActuon = null;//액션안에 있는거 제거(혹시 모를 중복 방지)
    }

    public void LastUpdateTime()//마지막 업데이트 시간 표시
    {
        lastUpdataTimeText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt");
    }

    //RangkingSaveData 들이 들어있는상태로 이 함수 호출
    public void RankingBoardUIUpdate()
    {
        StartCoroutine(LoadData());
    }

    private IEnumerator LoadData()
    {
        Debug.Log("인게임 순위표 표시 시작");
        Debug.Log($"newDataNumber : {newDataNumber}");
        RankingBarUIDestroy();//이전 데이터들 지우기
        loadingPanel.SetActive(true);//데이터 넣는동안 로딩패널 활성화

        //데이터 불러오기 - 노래이름으로 그 노래의 세이브 데이터 가져오기
        yield return StartCoroutine(localSaveManager.LocalDataLoad());

        //데이터 불러온 후
        int rank = 1;
        if (localSaveManager.datas != null && localSaveManager.datas.Count > 0)
        {
            //해당 노래의 데이터 리스트를 가져오기
            List<PlayerLocalSaveData> rankingDataList = localSaveManager.datas;

            //데이터를 바탕으로 랭킹 UI 생성
            foreach (PlayerLocalSaveData data in rankingDataList)
            {
                Debug.Log("랭킹 데이터 생성");
                GameObject rankingBarUI = Instantiate(rankingBarPrefab, contentArea);
                rankingBarUIs.Add(rankingBarUI);
                rankingBarUI.GetComponent<RankingBar>().UISetting(data, rank);
                rank++;
                if (rank == newDataNumber)//새로운 데이터는 색깔 다르게
                {
                    Debug.Log("신규 데이터 색 변경");
                    rankingBarUI.GetComponent<RankingBar>().UIColorChane(Color.yellow);
                }
            }
        }
        LastUpdateTime(); //UI 업데이트 후 시간 표시
        loadingPanel.SetActive(false);
        Debug.Log("인게임 순위표 표시 종료");
    }

    //UI프리팹들 지우기
    public void RankingBarUIDestroy()
    {
        foreach (GameObject rankingBar in rankingBarUIs)
        {
            Destroy(rankingBar);
        }
        rankingBarUIs.Clear();
    }

    public void LobbyButton()//로비 화면으로 이동 버튼
    {
        lobbyUIActuon?.Invoke();
    }

    public void MusicSelectButton()//곡 선택 화면으로 이동 버튼
    {
        musicSelectUIActuon?.Invoke();
    }

    public void ReplayButton()//다시 플레이 버튼
    {
        replayActuon?.Invoke();
    }

    public void StartTimer()
    {
        if (timerCorutine == null)
        {
            timerCorutine = StartCoroutine(Timer());
        }
    }

    public void StopTimer()
    {
        StopCoroutine(timerCorutine);
        timerCorutine = null;
    }

    public IEnumerator Timer()
    {
        float elapsedTime = 0f;
        while (elapsedTime < timerTime)
        {
            elapsedTime += Time.deltaTime;
            timer.fillAmount = 1 - (elapsedTime / timerTime);
            yield return null;
        }
        timer.fillAmount = 0;
    }
}
