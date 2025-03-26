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
        //Color를 Hex값으로 변환 예) #FF0000 이런식의 코드로 변환하지만 string에는 FF0000같은 #을 뺀 숫자만들어감
        string colorHex = ColorUtility.ToHtmlStringRGB(color);
        scoreUI.text = $"<color=#{colorHex}>{scoreUI.text}</color>";
        playerNameUI.text = $"<color=#{colorHex}>{playerNameUI.text}</color>";
        comboUI.text = $"<color=#{colorHex}>{comboUI.text}</color>";
        gameMusicNameUI.text = $"<color=#{colorHex}>{gameMusicNameUI.text}</color>";
        DifficultyUI.text = $"<color=#{colorHex}>{DifficultyUI.text}</color>";
        rankingUI.text = $"<color=#{colorHex}>{rankingUI.text}</color>";
    }
}
