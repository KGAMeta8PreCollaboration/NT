using System.Collections;
using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("노트가 Perfect되어야 하는 기준이 되는 시간")]
    public double noteTime;  // 노트가 Perfect되어야 하는 기준이 되는 시간
    public double noteBeat;  // 노트의 비트 위치
    public double noteSpeed; // 노트가 이동하는 속도
    public float noteFallTime;
    public double secPerBeat; // 1박자 당 시간
    public float railLength;

    private RhythmGameManager _rhythmGameManager;
    private double noteStartTime; // 노트가 움직이기 시작하는 DSP 시간

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        // 노트 시작 시간을 설정 (음악 시작 시간 기준)
        noteStartTime = _rhythmGameManager.musicStartTime + (noteTime - noteFallTime);
        StartCoroutine(MoveNote());
    }

    public void CalculateBeat(double bpm)
    {
        secPerBeat = 60.0 / bpm;  // 1박자 당 시간 계산
        noteBeat = noteTime / secPerBeat;  // 노트의 비트 위치 계산

        // 노트 속도 계산 (레일 길이 / 낙하 시간)
        noteSpeed = (float)railLength / noteFallTime;
    }

    public IEnumerator MoveNote()
    {
        Vector3 startPos = new Vector3(0, 0, 27); // 시작 위치
        Vector3 endPos = new Vector3(0, 0, 7); // 도착 위치

        while (true)
        {
            double curDspTime = AudioSettings.dspTime; // 현재 오디오 시간
            double elapsedTime = curDspTime - noteStartTime; // 노트가 시작한 후 경과한 시간

            if (elapsedTime < 0)
            {
                // 아직 노트가 출발할 시간이 안 되었으면 시작 위치에 고정
                transform.position = startPos;
            }
            else
            {
                // 노트 이동 시작
                float progress = Mathf.Clamp01((float)(elapsedTime / noteFallTime));
                transform.position = Vector3.Lerp(startPos, endPos, progress);

                // 노트가 도착하면 루프 종료
                if (elapsedTime >= noteFallTime)
                {
                    transform.position = endPos;
                    break;
                }
            }

            yield return null;
        }
    }


    public void Init()
    {
        _rhythmGameManager = FindObjectOfType<RhythmGameManager>();
    }
}
