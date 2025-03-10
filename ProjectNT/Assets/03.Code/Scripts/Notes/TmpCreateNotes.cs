using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TmpCreateNotes : MonoBehaviour
{
	private NoteManager noteManager;
	[SerializeField] private Woofer[] _woofers;

	private void Start()
	{
		noteManager = FindObjectOfType<NoteManager>();
		StartCoroutine(CreateCoroutine());
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
			int index = context.control.name == "a" ? 0 : 
				context.control.name == "s" ? 1 : 
				context.control.name == "d" ? 2 : 
				context.control.name == "f" ? 3 : 0;
			_woofers[index].Hit();
		}
	}
	
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
		noteManager.CreateNote(Random.Range(0, noteManager.maxNoteRails));
	}
}
