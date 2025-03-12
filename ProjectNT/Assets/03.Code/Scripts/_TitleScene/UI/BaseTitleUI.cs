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
        closeButton.onClick.AddListener(CloseUIButtonClick);
        gameObject.SetActive(false);
    }

    public virtual void CloseUIButtonClick()
    {
        TitleManager.instance.CloseUI();
    }
}
