using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Woofer[] _woofers;

    private NoteManager _noteManager;
    private ActionBasedController _controller;

    private void Start()
    {
        _noteManager = FindObjectOfType<NoteManager>();
        _controller = GetComponentInParent<ActionBasedController>();

        _controller.activateAction.action.performed += TriggerButtonAction;

        StartCoroutine(CreateCoroutine());
    }

    private void TriggerButtonAction(InputAction.CallbackContext context)
    {
        //상단 노트 상호작용, 일시정지, 확인 버튼 등등
    }

    public void Create(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _noteManager.CreateNote(int.Parse(context.control.name) - 1);
        }
    }

    public void Hit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InputControl control = context.control;
            int index = control.name.Equals("a") ? 0 :
                control.name.Equals("s") ? 1 :
                control.name.Equals("d") ? 2 :
                control.name.Equals("f") ? 3 : -1;

            if (index == -1) return;

            _woofers[index].Hit();
        }
    }

    //======================Test======================
    private IEnumerator CreateCoroutine()
    {
        while (true)
        {
            RandomCreate();
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void RandomCreate()
    {
        _noteManager.CreateNote(Random.Range(0, _noteManager.maxNoteRails));
    }
    //======================Test======================

    private void OnDestroy()
    {
        _controller.activateAction.action.performed -= TriggerButtonAction;
    }
}
