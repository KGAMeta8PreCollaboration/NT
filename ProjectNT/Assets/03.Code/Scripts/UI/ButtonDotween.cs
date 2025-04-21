using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDotween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOScale(1.1f, 0.2f).SetEase(Ease.Linear);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOScale(1f, 0.2f).SetEase(Ease.Linear);
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
}
