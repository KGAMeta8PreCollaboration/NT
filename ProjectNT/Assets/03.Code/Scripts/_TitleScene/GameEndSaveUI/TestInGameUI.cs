using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInGameUI : MonoBehaviour
{
    public Button testButton;

    public GameEndPanel gameEndPanel;

    private void Awake()
    {
        testButton.onClick.AddListener(TestStart);
    }

    public void TestStart()
    {
        int randomScroe = Random.Range(900000000, 999999999);
        int randomCombo = Random.Range(50, 150);
        gameEndPanel.SetGameEndData(randomScroe, randomCombo, "aa", "Easy");
        StartCoroutine(gameEndPanel.NewHighScoreCheck());
    }
}
