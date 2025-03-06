using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BlurEffectManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public PostProcessVolume postProcessVolume;
    public GameObject uiCamere;
    public float fadeOutTime = 3f;

    private DepthOfField depth;

    void Start()
    {
        postProcessVolume.profile.TryGetSettings(out depth);
        depth.focalLength.value = Mathf.Max(depth.focalLength.value, 200f);
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

        postProcessVolume.weight = 0f;
        uiCamere.SetActive(false);

        onComplete?.Invoke();
    }

    private void ResetTitleText()
    {
        titleText.gameObject.SetActive(true);
        uiCamere.SetActive(true);
        Color color = titleText.color;
        color.a = 1f;  // 알파값을 1로 설정
        titleText.color = color;
        postProcessVolume.weight = 1f;
        postProcessVolume.isGlobal = true;
    }
}
