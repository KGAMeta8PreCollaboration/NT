using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorPauseButton : MonoBehaviour
{
    [SerializeField] private Image pauseImage;
    [SerializeField] private Image resumeImage;

    private AudioSliderHandler _audioSliderHandler;
    private Button _pauseButton;
    private bool _paused;

    private void Awake()
    {
        _pauseButton = GetComponent<Button>();
        _audioSliderHandler = FindObjectOfType<AudioSliderHandler>();
        _pauseButton.onClick.AddListener(OnClickPauseButton);
        _paused = true;
        UpdateImage();
    }

    private void OnClickPauseButton()
    {
        _paused = !_paused;
        _audioSliderHandler.OnClickPauseButton(_paused);
        UpdateImage();
    }

    private void UpdateImage()
    {
        //멈춰있으면
        if (_paused)
        {
            pauseImage.gameObject.SetActive(true);
            resumeImage.gameObject.SetActive(false);
        }
        else
        {
            pauseImage.gameObject.SetActive(false);
            resumeImage.gameObject.SetActive(true);
        }
    }
}
