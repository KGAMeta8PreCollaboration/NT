using UnityEngine;
using UnityEngine.InputSystem;

public class TmpCreateNotes : MonoBehaviour
{
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
}
