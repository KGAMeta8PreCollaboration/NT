using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDeviceBase : MonoBehaviour
{
    [SerializeField] private InputActionReference menu_BTN;
    //스틱은 컨트롤러의 자식 오브젝트
    private StickTest leftStick;
    private StickTest rightStick;

    public XRBaseController leftHand;
    public XRBaseController rightHand;

    private void OnEnable()
    {
        menu_BTN.action.performed += Menu;
    }
    private void OnDisable()
    {
        menu_BTN.action.performed -= Menu;
    }
    private IEnumerator Start()
    {
        //스틱이 생성되는 걸 기다리기 위한 한프레임 휴식
        yield return null;
        leftStick = leftHand.model.GetComponent<StickTest>();
        rightStick = rightHand.model.GetComponent<StickTest>();
    }

    private void Menu(InputAction.CallbackContext context)
    {

        Debug.Log(context);
        Debug.Log("감지");

    }
}
