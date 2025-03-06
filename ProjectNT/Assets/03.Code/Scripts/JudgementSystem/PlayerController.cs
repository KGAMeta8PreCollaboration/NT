using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private JudgementSystem _judgementSystem;

	private NoteManager noteManager;
	[SerializeField] private Woofer[] _woofers;

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
		if (context.performed && context.control.name.Equals("space"))
		{
			_judgementSystem.CheckTiming();
		}
	}
}
