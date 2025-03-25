using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingBoardUI : BaseTitleUI
{
    public LocalSaveManager localSaveManager;

    public TextMeshProUGUI lastUpdataTimeText;
    public GameObject rankingBarPrefab;
    public RectTransform contentArea;
    public GameObject loadingPanel;

    public Button lobbyButton;
    public Button musicSelectButton;
    public GameObject musicSelectUI;

    private List<GameObject> rankingBarUIs = new List<GameObject>();

    public override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        AddEventListeners();
    }

    private void OnDisable()
    {
        RemoveEventListeners();
    }

    public override void AddEventListeners()
    {
        base.AddEventListeners();
        RankingBoardUIUpdate();
        lobbyButton.onClick.AddListener(LobbyButton);
        musicSelectButton.onClick.AddListener(MusicSelectButton);
    }

    public override void RemoveEventListeners()
    {
        RankingBarUIDestroy();
        base.RemoveEventListeners();
        lobbyButton.onClick.RemoveListener(LobbyButton);
        musicSelectButton.onClick.RemoveListener(MusicSelectButton);
    }

    public void LastUpdateTime()
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
                GameObject rankingBarUI = Instantiate(rankingBarPrefab, contentArea);
                rankingBarUIs.Add(rankingBarUI);
                rankingBarUI.GetComponent<RankingBar>().UISetting(data, rank);
                rank++;
            }
        }
        LastUpdateTime(); //UI 업데이트 후 시간 표시
        loadingPanel.SetActive(false);
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
        CloseUIButtonClick();
    }

    public void MusicSelectButton()//곡 선택 화면으로 이동 버튼
    {
        musicSelectUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
