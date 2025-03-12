using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpJudgementTime : MonoBehaviour
{
	private double _startDspTime;
	private void Awake()
	{
		_startDspTime = AudioSettings.dspTime;
	}
	private void OnCollisionEnter(Collision other)
	{
		if (!other.transform.CompareTag("Note"))
			return;
		print($"닿음 : {(AudioSettings.dspTime - _startDspTime):F3}");
	}
}
