using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class RankingBoardUI : BaseTitleUI
{
    public LocalSaveManager localSaveManager;

    public TextMeshProUGUI lastUpdataTimeText;
    public GameObject rankingBarPrefab;
    public RectTransform contentArea;
    public GameObject loadingPanel;

    private List<GameObject> rankingBarUIs = new List<GameObject>();
    private bool _isDataLoadComplete;

    public void Init()
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
    }

    public override void RemoveEventListeners()
    {
        RankingBarUIDestroy();
        base.RemoveEventListeners();
    }

    public void LastUpdateTime()
    {
        lastUpdataTimeText.text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt");
    }
    
    public void RankingBoardUIUpdate()
    {
        _isDataLoadComplete = false;
        StartCoroutine(LoadData());
    }

    public IEnumerator SetHighlightIndex(int index)
    {
        yield return new WaitUntil(() => _isDataLoadComplete);
        if (index - 1 >= 0 && index - 1 < rankingBarUIs.Count)
            rankingBarUIs[index - 1].GetComponent<RankingBar>().UIColorChane(Color.yellow);
    }


    public IEnumerator LoadData()
    {
        RankingBarUIDestroy();
        loadingPanel.SetActive(true);
        _isDataLoadComplete = false;
    
        yield return StartCoroutine(localSaveManager.LocalDataLoad());
    
        // 기존 코드 유지
        int rank = 1;
        if (localSaveManager.datas != null && localSaveManager.datas.Count > 0)
        {
            List<PlayerLocalSaveData> rankingDataList = localSaveManager.datas;
        
            foreach (PlayerLocalSaveData data in rankingDataList)
            {
                GameObject rankingBarUI = Instantiate(rankingBarPrefab, contentArea);
                rankingBarUIs.Add(rankingBarUI);
                rankingBarUI.GetComponent<RankingBar>().UISetting(data, rank);
                rank++;
            }
        }
    
        LastUpdateTime();
        loadingPanel.SetActive(false);
        _isDataLoadComplete = true;
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
