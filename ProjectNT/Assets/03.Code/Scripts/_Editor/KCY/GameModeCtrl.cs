using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeCtrl : MonoBehaviour
{
    [SerializeField] private List<Toggle> diffToggles;
    [SerializeField] private List<Toggle> modeToggles;
    private int diffCount;
    private int modeCount;
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
    }
    private void Start()
    {
        EditorDataManager.Instance.beatMapLoadAction?.Invoke(EditorDataManager.Instance.CurBeatMap);
        EditorDataManager.Instance.CurModeDiff = 0;
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
                    EditorDataManager.Instance.CurModeDiff = (Enums.ModeDiff)total;
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
                    EditorDataManager.Instance.CurModeDiff = (Enums.ModeDiff)total;
                    return;
                }
                i++;
            }
        }
    }
}
