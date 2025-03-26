using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
public class PopupManager : MonoBehaviour
{
    public List<Popup> popupList = new List<Popup>();

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        FindPopups();
    }

    public T OpenPopup<T>() where T : Popup
    {
        T foundPopup = popupList.Find(popup => popup is T) as T;

        if (foundPopup != null)
        {
            foundPopup.transform.SetAsLastSibling();
            foundPopup.gameObject.SetActive(true);
        }

        return foundPopup;
    }
    public T OpenPopup<T>(T popup) where T : Popup
    {
        if (popupList.Contains(popup))
        {
            popup.gameObject.SetActive(true);
            return popup;
        }
        return null;
    }


    public void ClosePopup(Popup popup)
    {
        if (popupList.Contains(popup))
        {
            popup.gameObject.SetActive(false);
        }
    }

    public void ClosePopup<T>() where T : Popup
    {
        T foundPopup = popupList.Find(popup => popup is T) as T;
        if (foundPopup != null)
        {
            foundPopup.gameObject.SetActive(false);
        }
    }

    //씬에 있는 모든 팝업 찾기
    private void FindPopups()
    {
        Popup[] popups = FindObjectsOfType<Popup>();

        foreach (Popup popup in popups)
        {
            popupList.Add(popup);
            popup.Init(this);
            popup.gameObject.SetActive(false);
            print($"찾은 팝업: {popup.name}");
        }
    }
}
