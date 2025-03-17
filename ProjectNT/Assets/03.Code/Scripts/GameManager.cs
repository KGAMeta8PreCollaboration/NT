using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private NoteManager _noteManager;

	private void Start()
	{
		_noteManager = FindObjectOfType<NoteManager>();
	}

	public void GameStart()
	{
	}
}
