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
    public GameObject uiNameText;
    [SerializeField]
    private InputActionReference left;
    [SerializeField]
    private InputActionReference right;

    private Outline outline; 
    private bool isOutlineActive = false;
    private XRSimpleInteractable simpleInteractable;

    private void Start()
    {
        outline = GetComponent<Outline>();
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        uiNameText.SetActive(false);
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
        left.action.started += OnSelect;
        right.action.started += OnSelect;
    }

    private void OnDisable()
    {
        left.action.started -= OnSelect;
        right.action.started -= OnSelect;
    }

    private void OnOutLine(HoverEnterEventArgs args)
    {
        Debug.Log("OnOutLine");
        if (TitleManager.instance.IsComplete && !TitleManager.instance.IsUIActive)
        {
            if (outline != null)
            {
                uiNameText.SetActive(true);
                outline.enabled = true; //아웃라인 활성화
                isOutlineActive = true; //아웃라인 활성화 됨을 확인
            }
        }
    }

    private void OffOutLine(HoverExitEventArgs args)
    {
        Debug.Log("OffOutLine");
        if (isOutlineActive)
        {
            if (outline != null)
            {
                uiNameText.SetActive(false);
                outline.enabled = false; //아웃라인 비활성화
                isOutlineActive = false; //아웃라인 비활성화 됨을 확인
            }
        }
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        Debug.Log($"클릭으로 OnSelect 호출 {uiName} UI활성화 시도");
        if (isOutlineActive)
        {
            TitleManager.instance.OpenUI(uiName);
            if (outline != null)
            {
                Debug.Log($"{uiName} UI활성화 성공");
                uiNameText.SetActive(false);
                outline.enabled = false;
                isOutlineActive = false;
            }
        }
        else
        {
            Debug.Log($"isOutlineActive 이므로 UI실행 실패");
        }
    }
}
