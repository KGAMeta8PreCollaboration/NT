using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UpperNode : MonoBehaviour
{
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private TextMeshProUGUI text;

    private int _index;
    private bool _isOn;
    private Toggle _toggle;
    private Image _image;

    public int Index {  get { return _index; } }

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.group = null;
        _toggle.isOn = false;
        _image = GetComponent<Image>();
    }

    public void SetUpperNodeIndex(int index)
    {
        text.text = (index + 1).ToString();
        _index = index;
    }

    public void UpdateState(List<int> activeIndexs)
    {
        //활성화 인덱스에 노드의 인덱스가 있다면 true
        bool isActive = activeIndexs.Contains(_index);
        _toggle.isOn = isActive;
        _image.color = isActive? selectedColor : normalColor;
    }
}
