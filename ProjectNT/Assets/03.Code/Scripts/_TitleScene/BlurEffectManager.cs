using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurEffectManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public GameObject uiCamere;
    public float fadeOutTime = 3f;
    public Volume volume;

    private VolumeProfile profile;
    private DepthOfField depth;

    void Start()
    {
        if (volume != null)
        {
            volume.enabled = true;
        }
        profile = volume.profile;
    }

    void Update()
    {
        // 카메라의 정면 (화면 정면)을 기준으로 위치를 고정시키기 위해
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, 2f);  // 화면의 중앙에 고정
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(screenCenter);  // 화면 좌표를 월드 좌표로 변환

        // 타이틀 텍스트의 위치를 화면 정면으로 고정
        titleText.transform.position = worldPosition;
        titleText.transform.rotation = Camera.main.transform.rotation;  // 카메라의 회전을 따라가도록 설정
    }

    public void ResetTitle()
    {
        ResetTitleText();
        titleText.gameObject.SetActive(true);
    }

    public void FadeOutStart(Action onComplete)
    {
        StartCoroutine(FadeOutTitleText(onComplete));
    }

    private IEnumerator FadeOutTitleText(Action onComplete)
    {
        float elapsedTime = 0f;
        Color startColor = titleText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        // 타이틀 텍스트를 서서히 투명하게 만듬
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            titleText.color = Color.Lerp(startColor, endColor, elapsedTime / fadeOutTime);
            yield return null;  // 매 프레임마다 업데이트
        }

        titleText.color = endColor; // 최종 색상 설정
        titleText.gameObject.SetActive(false); // 타이틀 텍스트를 비활성화하여 화면에서 제거

        if (depth != null)
        {
            volume.weight = 0f;
            volume.enabled = false;
        }
        uiCamere.SetActive(false);

        onComplete?.Invoke();
    }

    private void ResetTitleText()
    {
        if (volume != null)
        {
            volume.enabled = true;
        }
        profile = volume.profile;

        titleText.gameObject.SetActive(true);
        uiCamere.SetActive(true);
        Color color = titleText.color;
        color.a = 1f;  // 알파값을 1로 설정
        titleText.color = color;

        if (profile.TryGet(out depth))
        {
            //여기서 시작할때 전체화면 블러처리
            depth.mode.value = DepthOfFieldMode.Gaussian;

            depth.gaussianStart.value = 0.1f;
            depth.gaussianEnd.value = 1000f;
            depth.gaussianMaxRadius.value = 10f;
            depth.highQualitySampling.value = true;
            Debug.Log("블러 효과 성공");
        }
        else
        {
            Debug.LogWarning("volume.profile 가져오기 실패");
        }
        volume.weight = 1f;
        volume.isGlobal = true;
    }
}
