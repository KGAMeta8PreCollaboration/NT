using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseTitleUI : MonoBehaviour
{
    public Button closeButton;

    public virtual void Awake()
    {
        closeButton.onClick.AddListener(CloseUI);
        gameObject.SetActive(false);
    }

    public void CloseUI()
    {
        TitleManager.instance.CloseUI();
    }
}
