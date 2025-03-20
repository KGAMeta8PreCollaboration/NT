using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorSaveTracker : MonoBehaviour
{
    [SerializeField] private List<SaveTrakerElem> saveTrakerElems;

    public void SaveTracking(BeatMapData curBeatMap, BeatMapData cacheBeatMap)
    {
        if (curBeatMap == cacheBeatMap) return;
        foreach (SaveTrakerElem elem in saveTrakerElems)
        {
            if (elem.TmpText[elem.TmpText.Length - 1] != '*')
                if (elem.IsOn)
                {
                    elem.TmpText = elem.TmpText + "*";
                }
        }
    }
}
