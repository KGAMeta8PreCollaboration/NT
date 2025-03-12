using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using UnityEngine;
using Game;

public class AlarmPopup : Popup
{
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private TextMeshProUGUI _closeText;

    public void SetPopup(string content ,string closeText,Action closeAction = null)
    {
        _contentText.text = content;
        _closeText.text = closeText;
        this.closeAction = closeAction;
    }
}
