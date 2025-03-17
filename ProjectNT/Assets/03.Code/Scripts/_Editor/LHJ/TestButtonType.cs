using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButtonType : MonoBehaviour
{
    public Action<string> keySound;
    private Button _button;
    private string _keySoundInfo;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _keySoundInfo = transform.name;
        _button.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        keySound?.Invoke(_keySoundInfo);
    }
}
