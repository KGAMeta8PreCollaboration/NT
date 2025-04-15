using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookMarkPrefab : MonoBehaviour
{
    private float _position;
    public float Position => _position;

    public Action<float> OnBookMarkClicked;

    public void Initialize(float position)
    {
        _position = position;
        print($"북마크 생성 위치 : {_position}");
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        OnBookMarkClicked?.Invoke(_position);
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(HandleClick);
    }
}
