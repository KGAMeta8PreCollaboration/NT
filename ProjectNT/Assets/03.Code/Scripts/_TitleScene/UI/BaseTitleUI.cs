using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseTitleUI : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;

    public virtual void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual void AddEventListeners()
    {
        closeButton.onClick.AddListener(CloseUIButtonClick);
    }

    public virtual void RemoveEventListeners()
    {
        closeButton.onClick.RemoveListener(CloseUIButtonClick);
    }

    public virtual void CloseUIButtonClick()
    {
        TitleManager.instance.CloseUI();
    }
}
