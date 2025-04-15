using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpperNodeDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalPosition = _rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //    (RectTransform)_parentCanvas.transform,
        //    eventData.position,
        //    _parentCanvas.worldCamera,
        //    out Vector2 localPoint)
        //   )
        //{
        //    _rectTransform.position = localPoint;
        //}
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
