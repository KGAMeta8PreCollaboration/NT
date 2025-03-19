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
		// AudioSettings.Reset(default);

		StartCoroutine(GameStartCou());
	}

	private IEnumerator GameStartCou()
	{
		yield return new WaitForSeconds(3f);
        GameStart();

    }

    public void GameStart()
	{
		AudioManager.Instance.StartBGM(delayTime);
	}
}
