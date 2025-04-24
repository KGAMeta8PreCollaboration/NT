using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBoard : MonoBehaviour
{
    [SerializeField] private Texture2D[] gifImage;
    [SerializeField] private Texture2D originalTexture;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private float duration = 2f;

    private Material _targetMaterial;
    private int _currentIndex = 0;
    private Coroutine _displayCoroutine;
    private DisplayBoardHandler _displayBoardHandler;

    private void Awake()
    {
        _displayBoardHandler = FindObjectOfType<DisplayBoardHandler>(); 
    }

    private void Start()
    {
        _displayBoardHandler.displayBoardCallback += ChangeDisplayBoard;
        _targetMaterial = GetComponent<Renderer>().material;
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

    private void ChangeDisplayBoard()
    {
        if (_displayCoroutine != null)
        {
            StopCoroutine(_displayCoroutine);
        }

        _displayCoroutine = StartCoroutine(HandleDisplayBoard());
    }

    private IEnumerator HandleDisplayBoard()
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
        _displayCoroutine = null;
    }
}
