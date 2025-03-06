using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineTrigger : MonoBehaviour
{
    public GameObject text;
    public GameObject uiObject;

    private Outline outline;  // Outline 컴포넌트
    private Renderer objectRenderer;
    private bool isOutlineActive = false; // 아웃라인 활성화 여부

    private void Start()
    {
        outline = GetComponent<Outline>();  // Outline 컴포넌트 가져오기
        objectRenderer = GetComponent<Renderer>();  // Renderer 컴포넌트 가져오기
        text.SetActive(false);
        uiObject.SetActive(false);
        // 처음에는 아웃라인 비활성화
        if (outline != null)
        {
            outline.enabled = false; // 아웃라인 비활성화
        }
    }

    private void OnMouseOver()
    {
        // TitleManager의 isComplete가 true일 때만 아웃라인 활성화
        if (TitleManager.instance.isComplete && !isOutlineActive)
        {
            if (outline != null)
            {
                outline.enabled = true; // 마우스가 오브젝트에 올라가면 아웃라인 활성화
                isOutlineActive = true; // 아웃라인이 활성화되었음을 기록
                text.SetActive(true);
            }
        }
    }

    private void OnMouseExit()
    {
        if (isOutlineActive)
        {
            if (outline != null)
            {
                outline.enabled = false; // 마우스가 오브젝트 밖으로 나가면 아웃라인 비활성화
                isOutlineActive = false; // 아웃라인이 비활성화되었음을 기록
                text.SetActive(false);
            }
        }
    }

    private void OnMouseDown()
    {
        // 오브젝트를 클릭했을 때 활성화할 오브젝트가 설정되어 있다면
        if (uiObject != null)
        {
            TitleManager.instance.isUIActive
            uiObject.SetActive(true); // 해당 오브젝트를 활성화
        }
    }
}
