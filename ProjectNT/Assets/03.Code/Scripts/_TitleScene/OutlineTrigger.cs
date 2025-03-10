using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public InputActionReference left;
    public InputActionReference right;

    private Outline outline; 
    private bool isOutlineActive = false;
    private XRSimpleInteractable simpleInteractable;

    private void Start()
    {
        outline = GetComponent<Outline>();
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        text.SetActive(false);
        if (outline != null)
        {
            outline.enabled = false;
        }
        if (simpleInteractable != null)
        {
            simpleInteractable.hoverEntered.AddListener(OnOutLine);
            simpleInteractable.hoverExited.AddListener(OffOutLine);
        }
    }
    private void OnEnable()
    {
        left.action.started += Test;
        right.action.started += Test;
    }
    private void OnDisable()
    {
        left.action.started -= Test;
        right.action.started -= Test;
    }
    public void OnOutLine(HoverEnterEventArgs args)
    {
        Debug.Log("OnOutLine");
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

    public void OffOutLine(HoverExitEventArgs args)
    {
        Debug.Log("OffOutLine");
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

    public void OnSelect()
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

    public void Test(InputAction.CallbackContext context)
    {
        if (isOutlineActive)
        {
            OnSelect();
        }
    }
}
