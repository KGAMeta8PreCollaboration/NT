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
        gameEndPanel.SetGameEndData(999999999, 999, "aa", "Easy");
        gameEndPanel.NewHighScoreCheck();
    }
}
