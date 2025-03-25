using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingBar : MonoBehaviour//프리팹용
{
    public TextMeshProUGUI rankingUI;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI playerNameUI;
    public TextMeshProUGUI comboUI;
    public TextMeshProUGUI gameMusicNameUI;
    public TextMeshProUGUI DifficultyUI;

    private PlayerLocalSaveData data;

    public void UISetting(PlayerLocalSaveData data, int ranking)
    {
        this.data = data;
        scoreUI.text = int.Parse(this.data.score.ToString()).ToString("N0");
        playerNameUI.text = this.data.playerName.ToString();
        comboUI.text = int.Parse(this.data.combo.ToString()).ToString("N0");
        gameMusicNameUI.text = this.data.gameMusicName.ToString();
        DifficultyUI.text = this.data.difficulty.ToString();
        rankingUI.text = ranking.ToString();
    }

    public void UIColorChane(Color color)
    {
        scoreUI.color = color;
        playerNameUI.color = color;
        comboUI.color = color;
        gameMusicNameUI.color = color;
        DifficultyUI.color = color;
        rankingUI.color = color;
    }
}
