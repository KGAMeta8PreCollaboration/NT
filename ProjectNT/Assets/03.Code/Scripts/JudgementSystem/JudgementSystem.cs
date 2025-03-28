using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class JudgementSystem : MonoBehaviour
{
    [SerializeField] private Woofer _woofer;

    //test
    public TextMeshProUGUI logText2;

    private void Awake()
    {
        //logText2 = GameObject.Find("LogText2").GetComponent<TextMeshProUGUI>();
    }

    public JudgementType JudgeNote()
    {
        if (_woofer.notes == null || _woofer.notes.Count == 0 || _woofer.notes[0] == null)
        {
            print("미스!");
            return JudgementType.MISS;
        }
        Note note = _woofer.notes[0];

        double musicTime = AudioSettings.dspTime;// 현재 재생 시간
        double noteTime = note.GetTargetDspTime();
        double timeDiff = Math.Abs(musicTime - noteTime);
        string hitRes;
        JudgementType noteType;
        if (timeDiff < 0.2f)
        {
            hitRes = "Perfect";
            noteType = JudgementType.PERFECT;
        }
        else if (timeDiff < 0.25f)
        {
            hitRes = "Good";
            noteType = JudgementType.Good;
        }
        else if (timeDiff < 0.3f)
        {
            hitRes = "Cool";
            noteType = JudgementType.Cool;
        }
        else
        {
            hitRes = "Bad";
            noteType = JudgementType.MISS;
        }
        print($"현재 재생 시간: {musicTime.ToString("f2")}, 노트 재생 시간: {noteTime.ToString("f2")}, timeDiff: {timeDiff.ToString("f2")}, Result: {hitRes}");
        //logText2.text = $"현재 재생 시간: {musicTime.ToString("f2")}, 노트 재생 시간: {noteTime.ToString("f2")}, timeDiff: {timeDiff.ToString("f2")}, Result: {hitRes}";
        return noteType;
    }
}
