using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class JudgementSystem : MonoBehaviour
{
	[SerializeField] private Transform[] _timingTrans; //Perfact, Great, Good의 Transform

	private Vector2[] _timingBoxs;

	[SerializeField] private Woofer _woofer;

	//test
	public TextMeshProUGUI logText2;

	private void Awake()
	{
		//Init();
		//logText2 = GameObject.Find("LogText2").GetComponent<TextMeshProUGUI>();
	}

	private void Start()
	{
		_timingBoxs = new Vector2[_timingTrans.Length];
		for (int i = 0; i < _timingTrans.Length; i++)
		{
			float minX = _timingTrans[i].position.x - (_timingTrans[i].localScale.x / 2f);
			float maxX = _timingTrans[i].position.x + (_timingTrans[i].localScale.x / 2f);

			_timingBoxs[i].Set(minX, maxX);

			float diff = Mathf.Abs(minX - maxX);
			// print(diff);
			// print(_timingBoxs[i].x + ", " + _timingBoxs[i].y);
		}
	}
	public NoteType JudgeNote()
	{
		if (_woofer.notes == null || _woofer.notes.Count == 0 || _woofer.notes[0] == null)
		{
			print("미스!");
			return NoteType.Bad;
		}
		Note note = _woofer.notes[0];

		double musicTime = AudioSettings.dspTime;// 현재 재생 시간
		double noteTime = note.GetTargetDspTime();
		double timeDiff = Math.Abs(musicTime - noteTime);
		string hitRes;
		NoteType noteType;
		if (timeDiff < 0.2f)
		{
			hitRes = "Perfect";
			noteType = NoteType.Perfect;
		}
		else if (timeDiff < 0.25f)
		{
			hitRes = "Good";
			noteType = NoteType.Good;
		}
		else if (timeDiff < 0.3f)
		{
			hitRes = "Cool";
			noteType = NoteType.Cool;
		}
		else
		{
			hitRes = "Bad";
			noteType = NoteType.Bad;
		}
		print($"Time : {Time.time}, 현재 재생 시간: {musicTime.ToString("f2")}, 노트 재생 시간: {noteTime.ToString("f2")}, timeDiff: {timeDiff.ToString("f2")}, Result: {hitRes}");
		//logText2.text = $"현재 재생 시간: {musicTime.ToString("f2")}, 노트 재생 시간: {noteTime.ToString("f2")}, timeDiff: {timeDiff.ToString("f2")}, Result: {hitRes}";
		return noteType;
	}

	public NoteType CheckTiming()
	{
		if (_woofer.notes == null || _woofer.notes.Count == 0 || _woofer.notes[0] == null)
		{
			print("미스!");
			return NoteType.Bad;
		}

		float notePosX = _woofer.notes[0].transform.position.x;
		for (int i = 0; i < _timingBoxs.Length; i++)
		{
			if (_timingBoxs[i].x <= notePosX && notePosX <= _timingBoxs[i].y)
			{
				NoteType noteType = i == 0 ? NoteType.Perfect :
					i == 1 ? NoteType.Good :
					i == 2 ? NoteType.Cool : NoteType.Bad;
				print(noteType.ToString() + "!");
				return noteType;
			}
		}
		print("미스!");
		return NoteType.Bad;
	}

	private void Init()
	{
		_woofer = GetComponent<Woofer>();
	}
}
