using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerResultContainer
{
    public int score;
    public int currentCombo;
    public int maxCombo;
    public int[] judgeCount;

    public PlayerResultContainer(ScoreManager scoreManager)
    {
        score = scoreManager.score;
        currentCombo = scoreManager.currentCombo;
        maxCombo = scoreManager.maxCombo;
        judgeCount = scoreManager.judgeCount;
    }

    public PlayerResultContainer(int score, int currentCombo, int maxCombo, int[] judgeCounts)
    {
        this.score = score;
        this.currentCombo = currentCombo;
        this.maxCombo = maxCombo;
        this.judgeCount = judgeCounts;
    }
}

public class MultiScoreManager : MonoBehaviourPun
{
    private Dictionary<string, PlayerResultContainer> _playerResultDict = new Dictionary<string, PlayerResultContainer>();

    //나의 결과를 상대방 씬에 있는 나의 결과창에다가 전달
    public void SendMyResult()
    {
        ScoreManager myScoreManager = GameManager.Instance.MultiGameController.GetPlayerModuleByNick(PhotonNetwork.LocalPlayer.NickName).ScoreManager;
        print($"보내는 이의 모듈: {GameManager.Instance.MultiGameController.GetPlayerModuleByNick(PhotonNetwork.LocalPlayer.NickName).name}");
        PlayerResultContainer myResult = new PlayerResultContainer(myScoreManager);

        photonView.RPC(nameof(RPC_ReceivePlayerResult), RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName, myResult.score, myResult.maxCombo, myResult.currentCombo, myResult.judgeCount);
    }

    //상대방은 넘어온 결과창을 받아서 상대방 결과창에다가 적용
    [PunRPC]
    public void RPC_ReceivePlayerResult(string nickname, int score, int currentCombo, int maxCombo, int[] judgeCounts)
    {
        PlayerResultContainer container = new PlayerResultContainer(score, currentCombo, maxCombo, judgeCounts);

        _playerResultDict[nickname] = container;

        UpdateResultUI(nickname, container);
    }

    private void UpdateResultUI(string nickname, PlayerResultContainer result)
    {
        // 해당 플레이어의 ResultPanel 찾기
        ResultPanel resultPanel = GameManager.Instance.MultiGameController.GetPlayerModuleByNick(nickname).GetComponentInChildren<ResultPanel>();
        print($"받은 이의 모듈: {GameManager.Instance.MultiGameController.GetPlayerModuleByNick(nickname)}");
        resultPanel.SetResult(result);
    }

}
