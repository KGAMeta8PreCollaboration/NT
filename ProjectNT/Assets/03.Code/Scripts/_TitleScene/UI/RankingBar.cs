using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingBar : MonoBehaviour//프리팹용
{
    public Image playerImage;
    public TextMeshProUGUI lavelUI;
    public TextMeshProUGUI playerNameUI;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI rankingUI;

    public void UISetting(PlayerLocalSaveData data, int ranking)
    {
        //playerImage.sprite = data.playerImage.sprite;
        lavelUI.text = data.lavel.ToString();
        playerNameUI.text = data.playerName.ToString();
        scoreUI.text = data.score.ToString();
        rankingUI.text = ranking.ToString();
    }
}
