using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveTrakerElem : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI modDiff_tmp;
    public bool IsOn { get { return toggle.isOn; } }
    public string TmpText
    {
        get { return modDiff_tmp.text; }
        set { modDiff_tmp.text = value; }
    }
}
