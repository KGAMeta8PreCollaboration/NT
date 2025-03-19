using UnityEngine;
using UnityEngine.InputSystem;

public class TmpInput : MonoBehaviour
{
    public TmpWoofer woofer;
    private bool isHolding = false;

    public void Hit(InputAction.CallbackContext context)
    {
        if (context.performed && context.control.name == "space")
        {
            if (woofer.IsCurrentNoteLong()) // 현재 노트가 롱노트인지 확인
            {
                woofer.StartLongNote();
                isHolding = true;
            }
            else
            {
                woofer.Hit(); // 일반 노트 판정
            }
        }

        if (context.canceled && context.control.name == "space") // 키를 뗐을 때
        {
            if (isHolding)
            {
                woofer.ReleaseLongNote(); // 롱노트 종료
                isHolding = false;
            }
        }
    }

    private void Update()
    {
        if (isHolding)
        {
            woofer.Hold(); // 롱노트 지속 판정
        }
    }
}
