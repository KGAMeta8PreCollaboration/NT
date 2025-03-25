using Photon.Pun;
using System;
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

public class InteractableObject : MonoBehaviour
{
    public TitleManager titleManager;
    public TitleUIName uiType;
    public GameObject ui;
    public GameObject uiNameText;
    [SerializeField]
    private InputActionReference left;
    [SerializeField]
    private InputActionReference right;

    private Outline outline;

    private bool isEventRegistered = false;
    private XRSimpleInteractable simpleInteractable;
    private Action onCkilcAction;

    // TODO: 프로토타입 임시
    [SerializeField] private GameObject popupPanel;


    private void Awake()
    {
        outline = GetComponent<Outline>();
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        uiNameText.SetActive(false);
        if (outline != null)//아웃라인 참조 후 비활성화
        {
            outline.enabled = false;
        }
        if (simpleInteractable != null)
        {
            simpleInteractable.hoverEntered.AddListener(OnOutLine);
            simpleInteractable.hoverExited.AddListener(OffOutLine);
        }
        //멀티플레이 빼고는 ui 오픈으로
        if (uiType == TitleUIName.MultiPlay)
        {
            onCkilcAction += MultiPlayOpen;
        }
        else
        {
            onCkilcAction += UIOpen;
        }
    }

    private void OnOutLine(HoverEnterEventArgs args)
    {
        Debug.Log("OnOutLine");
        //페이드아웃이 끝나고 ui가 활성화중이 아닐때
        if (titleManager.IsComplete && !titleManager.IsUIActive)
        {
            if (outline != null)
            {
                outline.enabled = true; //아웃라인 활성화
                uiNameText.SetActive(true);
                if (!isEventRegistered)
                {
                    isEventRegistered = true;
                    left.action.started += OnSelectObject;
                    right.action.started += OnSelectObject;
                }
            }
        }
    }

    private void OffOutLine(HoverExitEventArgs args)
    {
        Debug.Log("OffOutLine");
        if (outline.enabled == true)
        {
            if (outline != null)
            {
                outline.enabled = false; //아웃라인 비활성화
                uiNameText.SetActive(false);
                if (isEventRegistered)
                {
                    isEventRegistered = false;
                    left.action.started -= OnSelectObject;
                    right.action.started -= OnSelectObject;
                }
            }
        }
    }

    private void OnSelectObject(InputAction.CallbackContext context)
    {
        onCkilcAction?.Invoke();
    }

    public void UIOpen()
    {
        titleManager.SetUIActive(true);//현재 ui가 켜져있음
        ui.SetActive(true);
        if (outline.enabled == true)
        {
            uiNameText.SetActive(false);
            outline.enabled = false;
            if (isEventRegistered)
            {
                isEventRegistered = false;
                left.action.started -= OnSelectObject;
                right.action.started -= OnSelectObject;
            }
        }
    }

    public void MultiPlayOpen()
    {
        titleManager.SetUIActive(true);//현재 ui가 켜져있음
        ui.SetActive(true);
        //이 아래에 멀티로 이동하는거
        PhotonNetwork.ConnectUsingSettings(); // Photon 서버 연결
    }
}
