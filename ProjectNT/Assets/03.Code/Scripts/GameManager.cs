using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public double delayTime = 2.0;
	
	private NoteManager _noteManager;
	
	private double _startDspTime;

	private void Start()
	{
		_noteManager = FindObjectOfType<NoteManager>();
		StartCoroutine(StartCoroutine());
	}
	
	private IEnumerator StartCoroutine()
	{
		yield return new WaitForSeconds(5f);
		GameStart();
	}

    public void GameStart()
	{
		AudioManager.Instance.StartBGM(delayTime);
	}
}
