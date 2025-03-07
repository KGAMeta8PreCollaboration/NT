using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public enum TitleUIName
{
    SinglePlay,
    MultiPlay,
    RankingBoard,
    GameSetting
}

public class OutlineTrigger : MonoBehaviour
{
    public TitleUIName uiName;
    public GameObject text;
    public XRBaseInteractable interactable;

    private Outline outline; 
    private bool isOutlineActive = false; 

    private void Start()
    {
        outline = GetComponent<Outline>();  
        text.SetActive(false);
        if (outline != null)
        {
            outline.enabled = false;
        }
        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(OnOutLine);
            interactable.hoverExited.AddListener(OffOutLine);
            interactable.selectEntered.AddListener(OnSelect);
        }
    }

    private void OnOutLine(HoverEnterEventArgs args)
    {
        if (TitleManager.instance.isComplete && !TitleManager.instance.isUIActive)
        {
            if (outline != null)
            {
                text.SetActive(true);
                outline.enabled = true; //아웃라인 활성화
                isOutlineActive = true; //아웃라인 활성화 됨을 확인
            }
        }
    }

    private void OffOutLine(HoverExitEventArgs args)
    {
        if (isOutlineActive)
        {
            if (outline != null)
            {
                text.SetActive(false);
                outline.enabled = false; //아웃라인 비활성화
                isOutlineActive = false; //아웃라인 비활성화 됨을 확인
            }
        }
    }

    public void OnSelect(SelectEnterEventArgs args)
    {
        Debug.Log($"클릭으로 OnSelect 호출 {uiName} UI활성화");
        TitleManager.instance.OpenUI(uiName);
        if (outline != null)
        {
            text.SetActive(false);
            outline.enabled = false; 
            isOutlineActive = false;
        }
    }
}
