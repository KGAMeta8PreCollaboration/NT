using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class JudgementSystem : MonoBehaviour
{
    public double bpm = 120f;
    public float offset = 0.2f;
    public GameObject hitPrefab;
    [SerializeField] private Note _note;
    [SerializeField] private TextMeshProUGUI hitResultText;

    private double _secondPerBeat; // 1비트당 시간 계산
    private RhythmGameManager _rhythmGameManager;

    private void Awake()
    {
        Init();
        _note.CalculateBeat(bpm);
    }

    void Start()
    {
        _secondPerBeat = 60f / bpm; // BPM을 초 단위로 변환
    }
    float curr = 5.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JudgeNote(_note);
        }
        double musicTime = AudioSettings.dspTime - _rhythmGameManager.musicStartTime - offset; // 현재 재생 시간
        print(musicTime.ToString("f2") + "초");
        if (musicTime >= curr - 0.02f && musicTime <= curr + 0.02f)
        {
            JudgeNote(_note);
            curr += 5.0f;
        }

    }

    public void JudgeNote(Note note)
    {
        double musicTime = AudioSettings.dspTime - _rhythmGameManager.musicStartTime - offset; // 현재 재생 시간
        double currentBeat = musicTime * _secondPerBeat; // 현재 비트 계산
        double timeDiff = Math.Abs(currentBeat - note.noteBeat); // 노트의 비트와 현재 비트 비교
        string hitRes;
        Vector3 notePos = note.transform.position;
        if (timeDiff < 0.1f * _secondPerBeat)
        {
            hitRes = "Perfect";
            hitResultText.text = "Perfect!";
        }
        else if (timeDiff < 0.2f * _secondPerBeat)
        {
            hitRes = "Great";
            hitResultText.text = "Great!";
        }
        else if (timeDiff < 0.3f * _secondPerBeat)
        {
            hitRes = "Good";
            hitResultText.text = "Good!";
        }
        else
        {
            hitRes = "Miss";
            hitResultText.text = "Miss...";
        }
        Instantiate(hitPrefab, note.transform.position, Quaternion.identity);
        //Instantiate(hitPrefab, notePos, Quaternion.identity);
        print($"Time : {Time.time}, 현재 재생 시간: {musicTime.ToString("f2")}, 현재 비트: {currentBeat.ToString("f2")}, 노트의 비트: {note.noteBeat.ToString("f2")}, timeDiff: {timeDiff.ToString("f2")}, Result: {hitRes}");
    }

    private void Init()
    {
        _rhythmGameManager = FindObjectOfType<RhythmGameManager>();
    }
}
