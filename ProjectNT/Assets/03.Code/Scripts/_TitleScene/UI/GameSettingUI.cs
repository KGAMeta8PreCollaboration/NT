using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSettingUI : BaseTitleUI
{
    public Button gameExitButton;
    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private float defaultBGMVolume = 0.5f;
    private float defaultSFXVolume = 0.5f;

    public override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        RemoveEventListeners();
        AddEventListeners();
    }

    private void OnDisable()
    {
        RemoveEventListeners();
    }

    public override void AddEventListeners()
    {
        base.AddEventListeners();
        gameExitButton.onClick.AddListener(GameExit);

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        StartSetVlume();
    }

    public override void RemoveEventListeners()
    {
        base.RemoveEventListeners();
        gameExitButton.onClick.RemoveListener(GameExit);

        bgmSlider.onValueChanged.RemoveListener(SetBGMVolume);
        sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }

    public override void CloseUIButtonClick()
    {
        base.CloseUIButtonClick();
    }

    public void GameExit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void StartSetVlume()
    {
        float staredBGMVolume = PlayerPrefs.GetFloat("BGM", defaultBGMVolume);
        float staredSFXVolume = PlayerPrefs.GetFloat("SFX", defaultSFXVolume);

        bgmSlider.value = staredBGMVolume;
        sfxSlider.value = staredSFXVolume;
    }

    public void SetBGMVolume(float volume)
    {
        if (volume == 0f)
        {
            audioMixer.SetFloat("BGM", -80f);//-80dB로 음소거
        }
        else
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);//dB로 볼륨 조정
        }
        PlayerPrefs.SetFloat("BGM", volume);
        
        //디버그용
        float currentBGMVolume;
        audioMixer.GetFloat("BGM", out currentBGMVolume); // 오디오 믹서에서 BGM 볼륨 값 가져오기
        Debug.Log($"BGM Slider Value: {volume}, Audio Mixer BGM Volume: {currentBGMVolume}");
    }

    public void SetSFXVolume(float volume)
    {
        if (volume == 0f)
        {
            audioMixer.SetFloat("SFX", -80f);//-80dB로 음소거
        }
        else
        {
            audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);//dB로 볼륨 조정
        }
        PlayerPrefs.SetFloat("SFX", volume);
        
        //디버그용
        float currentSFXVolume;
        audioMixer.GetFloat("SFX", out currentSFXVolume); // 오디오 믹서에서 SFX 볼륨 값 가져오기
        Debug.Log($"SFX Slider Value: {volume}, Audio Mixer SFX Volume: {currentSFXVolume}");
    }
}
