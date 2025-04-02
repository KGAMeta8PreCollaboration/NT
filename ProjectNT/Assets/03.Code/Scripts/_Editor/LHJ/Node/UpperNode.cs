using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UpperNode : MonoBehaviour
{
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;
    private int _index;
    private bool _isOn;

    public void SetUpperNodeIndex(int index)
    {
        _index = index;
    }

    public void InitializeUpperNode(int index)
    {
        _isOn = _index == index;
        SetColor();
    }

    private void SetColor()
    {
        if (_isOn)
        {
            gameObject.GetComponent<Image>().color = selectedColor;
        }
        else
        {
            gameObject.GetComponent<Image>().color = normalColor;
        }
    }
}
