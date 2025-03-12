using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

public class GameModeCtrl : MonoBehaviour
{
    [SerializeField] private List<Toggle> diffToggles;
    [SerializeField] private List<Toggle> modeToggles;
    private Enums.ModeDiff currentGameMode;
    private int diffCount;
    private int modeCount;

    public Enums.ModeDiff CurrentGameMode => currentGameMode;

    private void Awake()
    {
        foreach (Toggle tg in diffToggles)
        {
            tg.onValueChanged.AddListener(SetDiff);
            tg.onValueChanged.AddListener((x) => tg.interactable = !x);
        }
        foreach (Toggle tg in modeToggles)
        {
            tg.onValueChanged.AddListener(SetMode);
            tg.onValueChanged.AddListener((x) => tg.interactable = !x);
        }
        diffToggles[0].isOn = true;
        modeToggles[0].isOn = true;
        currentGameMode = 0;
    }

    private void SetMode(bool isTrue)
    {
        if (isTrue)
        {
            int i = 0;
            foreach (Toggle tg in modeToggles)
            {
                if (tg.isOn)
                {
                    modeCount = i;
                    int total = modeCount + diffCount;
                    currentGameMode = (Enums.ModeDiff)total;
                    return;
                }
                i += 4;
            }
        }
    }
    private void SetDiff(bool isTrue)
    {
        if (isTrue)
        {
            int i = 0;
            foreach (Toggle tg in diffToggles)
            {
                if (tg.isOn)
                {
                    diffCount = i;
                    int total = modeCount + diffCount;
                    currentGameMode = (Enums.ModeDiff)total;
                    return;
                }
                i++;
            }
        }
    }
}
