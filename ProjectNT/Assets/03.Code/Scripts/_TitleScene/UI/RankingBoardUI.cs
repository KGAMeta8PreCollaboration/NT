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

    [SerializeField]
    private MusicChangeAndSelect musicCange;
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
        musicCange.BackToFirstSongMusic();
        base.AddEventListeners();
        musicCange.changeLeftButton.onClick.AddListener(() => musicCange.PreviousMusic(RankingBoardUIUpdate));
        musicCange.changeRightButton.onClick.AddListener(() => musicCange.NextMusic(RankingBoardUIUpdate));
        RankingBoardUIUpdate();
    }

    public override void RemoveEventListeners()
    {
        RankingBarUIDestroy();
        base.RemoveEventListeners();
        musicCange.changeLeftButton.onClick.RemoveAllListeners();
        musicCange.changeRightButton.onClick.RemoveAllListeners();
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
        string curMusicName = musicCange.CurMusicData.musicName;
        yield return StartCoroutine(localSaveManager.LocalDataLoad(curMusicName));

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
}
