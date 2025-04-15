using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookMarkHandler : MonoBehaviour
{
    [SerializeField] private Button bookMarkButton;
    [SerializeField] private Slider audioSlider;
    [SerializeField] private GameObject bookMarkPrefab;
    [SerializeField] private RectTransform sliderArea;

    private AudioSourceManager _audioSourceManager;
    private List<BookMarkPrefab> bookMarks = new List<BookMarkPrefab>();

    private void Awake()
    {
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
        bookMarkButton.onClick.AddListener(OnClickBookMarkButton);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CheckBookMarkCanDelete();
        }

        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    print($"현재 북마크 개수 : {bookMarks.Count}");
        //}
    }

    private void CheckBookMarkCanDelete()
    {
        foreach (var bookmark in bookMarks.ToArray())
        {
            if (bookmark != null && IsPointerOverBookMark(bookmark))
            {
                DeleteBookMark(bookmark);
                break;
            }
        }
    }

    private void DeleteBookMark(BookMarkPrefab bookmark)
    {
        bookmark.OnBookMarkClicked -= JumpToBookMark;
        bookMarks.Remove(bookmark);
        Destroy(bookmark.gameObject);
    }

    private bool IsPointerOverBookMark(BookMarkPrefab bookmark)
    {
        RectTransform rt = bookmark.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition);
    }

    private void OnClickBookMarkButton()
    {
        float currentPosition = audioSlider.value;
        CreateBookMark(currentPosition);
        print($"현재 오디오 슬라이더의 값 : {audioSlider.value}");
    }

    private void CreateBookMark(float position)
    {
        float sliderWidth = sliderArea.rect.width;

        // 보여지는 오브젝트 생성
        GameObject bookMarkObj = Instantiate(bookMarkPrefab, sliderArea);
        RectTransform bookMarkRect = bookMarkObj.GetComponent<RectTransform>();
        float xPos = position * sliderWidth;
        //지금은 슬라이더 때문에 많이 내리는데 원래 -22정도가 적당
        bookMarkRect.anchoredPosition = new Vector2(xPos, -22f);

        // BookMarkPrefab 컴포넌트 초기화
        BookMarkPrefab bookMark = bookMarkObj.GetComponent<BookMarkPrefab>();
        bookMark.Initialize(position);
        bookMark.OnBookMarkClicked += JumpToBookMark;

        bookMarks.Add(bookMark);
    }

    private void JumpToBookMark(float position)
    {
        float targetTime = position * _audioSourceManager.AudioDuration;
        _audioSourceManager.AudioSource.time = targetTime;
        audioSlider.value = position;
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        foreach (var bookmark in bookMarks)
        {
            if (bookmark != null)
                bookmark.OnBookMarkClicked -= JumpToBookMark;
        }
    }
}
