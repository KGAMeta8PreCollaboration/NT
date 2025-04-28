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
        //이퀄로 바꾼 후 다시 오리지널
        _displayBoardHandler.changeDisplayBoardCallback += ChangeDisplayBoard;
        //오리지널로 바꾼 후 다시 검은 화면
        //_displayBoardHandler.turnOnDisplayBoardCallback += TurnOnDisplayBoard;
        //오리지널로 켜놓음
        //_displayBoardHandler.setDisplayOriginalCallback += SetDisplayToOriginal;
        //이퀄라이저로 켜놓음
        _displayBoardHandler.setDisplayEqualizerCallback += SetDisplayBoardToEqualizer;
        _targetMaterial = GetComponent<Renderer>().material;

        SetDisplayToOriginal();
    }

    //테스트 할때 열어보시오
    //float elapsed = 0;
    //bool tmp = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetDisplayBoardToEqualizer();
            //    tmp = !tmp;
            //}
        }

        //if (tmp)
        //{
        //    elapsed += Time.deltaTime;
        //    if (elapsed >= delay)
        //    {
        //        _currentIndex = (_currentIndex + 1) % gifImage.Length;
        //        _targetMaterial.SetTexture("_MainTexture", gifImage[_currentIndex]);
        //        elapsed = 0f;
        //    }
        //}
    }

    //초기 상태는 검정색 화면이여야 함 -> 변경됨
    private void SetDisplayToBlack()
    {
        _targetMaterial.SetTexture("_BaseMap", blackTexture);
    }

    private void SetDisplayToOriginal()
    {
        _targetMaterial.SetTexture("_BaseMap", originalTexture);
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
            .AppendCallback(() => _targetMaterial.SetTexture("_BaseMap", originalTexture))
            .AppendInterval(duration)
            .AppendCallback(() => _targetMaterial.SetTexture("_BaseMap", blackTexture));
    }

    private IEnumerator TurnOnDisplayBoardCoroutine()
    {
        _targetMaterial.SetTexture("_BaseMap", originalTexture);
        yield return new WaitForSeconds(duration);
        _targetMaterial.SetTexture("_BaseMap", blackTexture);
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
            _targetMaterial.SetTexture("_BaseMap", gifImage[currentIndex]);
            currentIndex = (currentIndex + 1) % gifImage.Length;

            yield return new WaitForSeconds(delay);
            elapsed += delay;
        }

        _targetMaterial.SetTexture("_BaseMap", originalTexture);
        _changeDisplayCoroutine = null;
    }

    private void SetDisplayBoardToEqualizer()
    {
        StartCoroutine(SetDisplayBoardToEqualizerCoroutine());
    }

    private IEnumerator SetDisplayBoardToEqualizerCoroutine()
    {
        int currentIndex = 0;
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                _targetMaterial.SetTexture("_BaseMap", gifImage[currentIndex]);
                currentIndex = (currentIndex + 1) % gifImage.Length;
                timer = 0f;
            }
            yield return null;
        }
    }
}
