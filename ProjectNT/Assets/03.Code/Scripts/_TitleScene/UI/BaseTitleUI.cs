using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseTitleUI : MonoBehaviour
{
    public TitleManager titleManager;
    [SerializeField]
    private Button closeButton;

    public virtual void Awake()
    {
        gameObject.SetActive(false);
    }

    protected virtual void Start()
    {
        titleManager = FindObjectOfType<TitleManager>();
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
        titleManager.SetUIActive(false);
        gameObject.SetActive(false);
    }
}
