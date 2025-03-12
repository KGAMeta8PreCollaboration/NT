using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using UnityEngine;

public class AlarmPopup : Popup
{
    [SerializeField] private TextMeshPro _contentText;

    public void SetPopup(string content, Action closeAction = null)
    {
        _contentText.text = content;
        this.closeAction = closeAction;
    }
}
