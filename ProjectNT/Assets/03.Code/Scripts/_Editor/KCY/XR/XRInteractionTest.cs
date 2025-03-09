using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractionTest : MonoBehaviour
{
    public InputActionReference refer;
    private XRSimpleInteractable xRSimpleInteractable;
    private bool focusing;

    private void Awake()
    {
        xRSimpleInteractable = GetComponent<XRSimpleInteractable>();
        xRSimpleInteractable.hoverEntered.AddListener(HoverEnterd);
        xRSimpleInteractable.hoverExited.AddListener(HoverExited);
    }
    private void OnEnable()
    {
        refer.action.performed += Test;

    }
    private void OnDisable()
    {
        refer.action.performed -= Test;
    }
    public void HoverEnterd(HoverEnterEventArgs args)
    {
        focusing = true;
    }
    public void HoverExited(HoverExitEventArgs args)
    {
        focusing = false;
    }
    public void Test(InputAction.CallbackContext context)
    {
        if (focusing)
        {
            Debug.Log("작동");
        }
    }
}
