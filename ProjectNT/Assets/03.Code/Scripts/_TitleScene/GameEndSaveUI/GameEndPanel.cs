using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndPanel : MonoBehaviour
{
    public LocalSaveManager localSaveManager;
    public NewHighScroeUI newHighScroeUI;
    public RankRegistrationUI rankRegistrationUI;
    public InGameRankingUI inGameRankingUI;

    public PlayerLocalSaveData newData;
    public RankingBoardUI rankingBoardUI;
    
    private GameObject curUI = null;
    private int newDataNumber = 0;

    private void Awake()
    {
        newHighScroeUI.gameObject.SetActive(false);
        rankRegistrationUI.gameObject.SetActive(false);
        inGameRankingUI.gameObject.SetActive(false);
        rankingBoardUI.gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    //게임 끝나고 데이터 넣기
    public void SetGameEndData(int score, int combo, string gameMusicName, string difficulty)
    {
        newData = new PlayerLocalSaveData(score, null, combo, gameMusicName, difficulty);
    }

    public IEnumerator NewHighScoreCheck()
    {
        print("NewHighScoreCheck 1");
        List<PlayerLocalSaveData> newDataList = new List<PlayerLocalSaveData>();

        print("NewHighScoreCheck 2");
        //데이터전부 넣고
        yield return localSaveManager.LocalDataLoad();
        newDataList.AddRange(localSaveManager.datas);
        newDataList.Add(newData);
        print("NewHighScoreCheck 3");

        //비교
        newDataList.Sort((player1, player2) =>
        {
            int scoreComparison = player2.score.CompareTo(player1.score); //점수 비교
            if (scoreComparison == 0) //점수가 같으면 콤보로 비교
            {
                return player2.combo.CompareTo(player1.combo);
            }
            return scoreComparison;
        });

        print("NewHighScoreCheck 4");
        if (newDataList.Count > 50) //50개만 남기기
        {
            newDataList.RemoveRange(50, newDataList.Count - 50);
        }

        print("NewHighScoreCheck 5");
        if (newDataList.Contains(newData))
        {
            //새로운 데이터가 50위안에 듬
            print("NewHighScoreCheck 5 - 1");
            int rank = newDataList.IndexOf(newData) + 1;//새로운 데이터가 몇등인지
            newDataNumber = rank;
            print("NewHighScoreCheck 5 - 2");
            Debug.Log($"newDataNumber : {newDataNumber}, rank : {rank}");
            OpenNewHighScroeUI(rank);//새로운 데이터가 몇등인지 UI에 표시
            print("NewHighScoreCheck 5 - 3");
            Debug.Log("새로운 데이터가 50위안에 들어감");
        }
        else
        {
            //새로운 데이터가 50위안에 못 듬
            Debug.Log("새로운 데이터가 50위에 안들어감");
        }
        print("NewHighScoreCheck 6");
    }

    public void OpenNewHighScroeUI(int rank)//최고 점수 갱신 UI 오픈
    {
        CloseUI();//켜져 있던 창 닫기(여긴 사실 맨 처음 키는거라 필요없을수도 있음)
        newHighScroeUI.gameObject.SetActive(true);//UI키고
        newHighScroeUI.SetNewHighScroeUI(rank, newData);//최고 점수 UI에 표시
        newHighScroeUI.yesButtonAction += OpenRankRegistrationUI;//등록UI 오픈 등록
        newHighScroeUI.noButtonAction += CloseUI;//팝업 닫기
        curUI = newHighScroeUI.gameObject;
        Debug.Log("새로운 데이터 등록 창");
    }

    public void OpenRankRegistrationUI()//등록 UI 오픈
    {
        CloseUI();//켜져 있던 창 닫기
        rankRegistrationUI.gameObject.SetActive(true);
        rankRegistrationUI.registrationActuon += RegistrationSaveData;//저장 후 순위표 오픈
        curUI = rankRegistrationUI.gameObject;
        Debug.Log("등록할 이름 설정 창");
    }

    public void RegistrationSaveData()//이름 저장 후 로컬 데이터 폴더에 저장
    {
        newData.playerName = rankRegistrationUI.SetPlayerName();
        StartCoroutine(DataSave(OpenInGameRankingUI));
    }

    public IEnumerator DataSave(Action action)
    {
        Debug.Log("이름 결정");
        yield return StartCoroutine(localSaveManager.LocalDataSave(newData));
        Debug.Log("OpenInGameRankingUI로 이동");
        action?.Invoke();
    }

    public void OpenInGameRankingUI()//순위표 UI 오픈
    {
        CloseUI();
        // inGameRankingUI.gameObject.SetActive(true);//순위표 UI 오픈
        rankingBoardUI.gameObject.SetActive(true);
        // curUI = inGameRankingUI.gameObject;
        curUI = rankingBoardUI.gameObject;
        inGameRankingUI.timeOverAction += OpenMusicSelectUI;//로비로 이동 등록
        inGameRankingUI.newDataNumber = newDataNumber;
        Debug.Log($"newDataNumber : {newDataNumber}");
        Debug.Log("인 게임 순위표 표시");
    }

    public void OpenMusicSelectUI()//곡 선택창으로 이동
    {
        CloseUI();
        //곡 선택창으로 이동
        Debug.Log("곡 선택창 이동");
    }

    public void CloseUI()//현재 UI 끄기
    {
        curUI?.gameObject.SetActive(false);//켜져 있던 창 있으면 끄기
        curUI = null;
        Debug.Log("현재 창 끄기");
    }
}
