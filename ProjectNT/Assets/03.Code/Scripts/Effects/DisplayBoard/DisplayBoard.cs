using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBoard : MonoBehaviour
{
    [SerializeField] private Texture2D[] gifImage;
    [SerializeField] private Texture2D originalTexture;
    [SerializeField] private Texture2D blackTexture;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private float duration = 2f;

    private Material _targetMaterial;
    private int _currentIndex = 0;
    private Coroutine _changeDisplayCoroutine;
    private Coroutine _turnOnDisplayCoroutine;
    private DisplayBoardHandler _displayBoardHandler;

    private void Awake()
    {
        _displayBoardHandler = FindObjectOfType<DisplayBoardHandler>(); 
    }

    private void Start()
    {
        _displayBoardHandler.changeDisplayBoardCallback += ChangeDisplayBoard;
        _displayBoardHandler.turnOnDisplayBoardCallback += TurnOnDisplayBoard;
        _displayBoardHandler.setDisplayOriginalCallback += SetDisplayToOriginal;
        _targetMaterial = GetComponent<Renderer>().material;

        //시작할때는 검정색 화면인 상태
        SetDisplayToBlack();
    }

    //private void Update()
    //{
    //    timer += Time.deltaTime;
    //    if (timer >= delay)
    //    {
    //        _currentIndex = (_currentIndex + 1) % gifImage.Length;
    //        _targetMaterial.SetTexture("_MainTexture", gifImage[_currentIndex]);
    //        timer = 0f;
    //    }
    //}

    //초기 상태는 검정색 화면이여야 함
    private void SetDisplayToBlack()
    {
        _targetMaterial.SetTexture("_MainTexture", blackTexture);
    }

    private void SetDisplayToOriginal()
    {
        _targetMaterial.SetTexture("_MainTexture", originalTexture);
    }

    private Sequence _turnOnSequence;
    //디스플레이 켜는 함수
    private void TurnOnDisplayBoard()
    {
        if (_turnOnSequence != null && _turnOnSequence.IsActive())
        {
            _turnOnSequence.Kill();
        }

        _turnOnSequence = DOTween.Sequence()
            .AppendCallback(() => _targetMaterial.SetTexture("_MainTexture", originalTexture))
            .AppendInterval(duration)
            .AppendCallback(() => _targetMaterial.SetTexture("_MainTexture", blackTexture));
    }

    private IEnumerator TurnOnDisplayBoardCoroutine()
    {
        _targetMaterial.SetTexture("_MainTexture", originalTexture);
        yield return new WaitForSeconds(duration);
        _targetMaterial.SetTexture("_MainTexture", blackTexture);
    }


    //이퀄라이저로 바꾸는 함수
    private void ChangeDisplayBoard()
    {
        if (_changeDisplayCoroutine != null)
        {
            StopCoroutine(_changeDisplayCoroutine);
        }

        _changeDisplayCoroutine = StartCoroutine(ChangeDisplayBoardCoroutine());
    }

    private IEnumerator ChangeDisplayBoardCoroutine()
    {
        float elapsed = 0f;
        int currentIndex = 0;

        while (elapsed < duration)
        {
            _targetMaterial.SetTexture("_MainTexture", gifImage[currentIndex]);
            currentIndex = (currentIndex +1) % gifImage.Length;

            yield return new WaitForSeconds(delay);
            elapsed += delay;
        }

        _targetMaterial.SetTexture("_MainTexture", originalTexture);
        _changeDisplayCoroutine = null;
    }
}
