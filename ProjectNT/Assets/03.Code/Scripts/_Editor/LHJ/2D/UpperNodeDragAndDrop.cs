using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpperNodeDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private string nodeId;

    private const string NODE_POS_KEY = "NodePosition";

    private RectTransform _rectTransform;
    private Vector2 _originalPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        LoadPosition();
        //DisableChildDragging();
    }

    private void DisableChildDragging()
    {
        // Get all UpperNodeDragAndDrop components from children
        var childDrags = GetComponentsInChildren<UpperNodeDragAndDrop>();
        foreach (var drag in childDrags)
        {
            // Skip if it's this component
            if (drag == this) continue;

            // Remove the drag component from children
            Destroy(drag);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != gameObject) return;
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

        if (eventData.pointerCurrentRaycast.gameObject != gameObject) return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    private void SavePosition()
    {
        PlayerPrefs.SetFloat($"{NODE_POS_KEY}{nodeId}_X", _rectTransform.position.x);
        PlayerPrefs.SetFloat($"{NODE_POS_KEY}{nodeId}_Y", _rectTransform.position.y);
        PlayerPrefs.Save();
    }

    private void LoadPosition()
    {
        string xKey = $"{NODE_POS_KEY}{nodeId}_X";
        string yKey = $"{NODE_POS_KEY}{nodeId}_Y";

        if (PlayerPrefs.HasKey(xKey) && PlayerPrefs.HasKey(yKey))
        {
            float x = PlayerPrefs.GetFloat(xKey);
            float y = PlayerPrefs.GetFloat(yKey);
            _rectTransform.position = new Vector2(x, y);
        }
    }

    private void OnDestroy()
    {
        SavePosition();
    }
}
