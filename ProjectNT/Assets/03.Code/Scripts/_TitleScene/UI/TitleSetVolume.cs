using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TitleSetVolume : MonoBehaviour
{
    public AudioMixer audioMixer;

    private float defaultBGMVolume = 0.5f;
    private float defaultSFXVolume = 0.5f;

    private void Awake()
    {
        SetVolume();
    }

    public void SetVolume()
    {
        float startBGMVolume = PlayerPrefs.GetFloat("BGM", defaultBGMVolume);
        float startSFXVolume = PlayerPrefs.GetFloat("SFX", defaultSFXVolume);

        audioMixer.SetFloat("BGM", Mathf.Log10(startBGMVolume) * 20);
        audioMixer.SetFloat("SFX", Mathf.Log10(startSFXVolume) * 20);

        DebugLog(startBGMVolume, startSFXVolume);
    }

    public void DebugLog(float startBGMVolume, float startSFXVolume)
    {
        //디버그용
        float currentBGMVolume;
        float currentSFXVolume;

        audioMixer.GetFloat("BGM", out currentBGMVolume); // BGM 볼륨 가져오기
        audioMixer.GetFloat("SFX", out currentSFXVolume); // SFX 볼륨 가져오기

        // 디버그 출력
        Debug.Log($"Start BGM Volume (PlayerPrefs): {startBGMVolume}, Audio Mixer BGM Volume: {currentBGMVolume}");
        Debug.Log($"Start SFX Volume (PlayerPrefs): {startSFXVolume}, Audio Mixer SFX Volume: {currentSFXVolume}");
    }
}
