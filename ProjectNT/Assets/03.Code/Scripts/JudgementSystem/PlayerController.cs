using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private JudgementSystem[] _judgementSystems;

    private NoteManager noteManager;

    private void Start()
    {
        noteManager = FindObjectOfType<NoteManager>();
    }

    public void Create(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            noteManager.CreateNote(int.Parse(context.control.name) - 1);
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

            _judgementSystems[index].CheckTiming();
        }
    }
}
