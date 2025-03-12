using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerLocalSaveData
{
    public int lavel;
    public string playerName;
    public float score;
    public string playerImageName;

    public PlayerLocalSaveData(string imageName, int lavel, string playerName, float score)
    {
        this.lavel = lavel;
        this.playerName = playerName;
        this.score = score;

        this.playerImageName = imageName;
    }

    public Texture2D GetPlayerUmage()
    {
        string path = "Images/" + playerImageName;
        Texture2D texture = Resources.Load<Texture2D>(path);
        return texture;
    }
}

public class RankingBoardUI : BaseTitleUI
{
    public LocalSaveManager localSaveManager;

    public TextMeshProUGUI lastUpdataTimeText;
    public GameObject rankingBarPrefab;
    public RectTransform contentArea;
    public GameObject loadingPanel;

    private List<GameObject> rankingBarUIs = new List<GameObject>();

    public override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        RankingBoardUIUpdate();
    }

    private void OnDisable()
    {
        RankingBaardUIDestroy();
    }

    public void LastUpdateTime()
    {
        //updataTimeText 에 업데이트 시간 넣기
    }

    //RangkingSaveData 들이 들어있는상태로 이 함수 호출
    public void RankingBoardUIUpdate()
    {
        StartCoroutine(LoadData());
    }

    private IEnumerator LoadData()
    {
        loadingPanel.SetActive(true);

        //데이터 불러오기
        yield return StartCoroutine(localSaveManager.LocalDataLoad());

        //데이터 불러온 후
        int rank = 1;
        foreach (PlayerLocalSaveData data in localSaveManager.datas)
        {
            GameObject rankingBarUI = Instantiate(rankingBarPrefab, contentArea);
            rankingBarUIs.Add(rankingBarUI);
            rankingBarUI.GetComponent<RankingBarUI>().UISetting(data, rank);
            rank++;
        }
        LastUpdateTime();//UI업데이트하고 시간표시
        loadingPanel.SetActive(false);
    }

    //UI프리팹들 지우기
    public void RankingBaardUIDestroy()
    {
        foreach (GameObject rankingBar in rankingBarUIs)
        {
            Destroy(rankingBar);
        }
        rankingBarUIs.Clear();
    }
}
