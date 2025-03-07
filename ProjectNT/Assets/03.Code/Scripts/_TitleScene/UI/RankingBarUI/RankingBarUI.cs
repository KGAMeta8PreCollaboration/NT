using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RangkingSaveData
{
    public Image playerImage;
    public int lavel;
    public string playerName;
    public float score;
    public int ranking;
}

public class RankingBarUI : MonoBehaviour
{
    public Image playerImage;
    public TextMeshProUGUI lavelUI;
    public TextMeshProUGUI playerNameUI;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI rankingUI;

    private RangkingSaveData data;

    // Start is called before the first frame update
    void Start()
    {
        if (data != null)
        {
            UISetting();
        }
    }
    
    public void UISetting()
    {
        playerImage.sprite = data.playerImage.sprite;
        lavelUI.text = data.lavel.ToString();
        playerNameUI.text = data.playerName.ToString();
        scoreUI.text = data.score.ToString();
        rankingUI.text = data.ranking.ToString();
    }
}
