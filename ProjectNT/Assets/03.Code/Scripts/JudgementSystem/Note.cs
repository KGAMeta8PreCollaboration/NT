using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("노트의 등장 시간")]
    public double noteTime;  // 노트가 등장하는 초 단위 시간
    public double noteBeat;  // 노트의 비트 위치
    public double noteSpeed; // 노트가 이동하는 속도

    public double lengthInBeats = 4; // 4/4박자 = 4비트
    public double railLength = 10f; // 레일의 길이
    public double offset;

    private void FixedUpdate()
    {
        //print("fixedDeltaTime : " + Time.fixedDeltaTime);
        transform.Translate(Vector3.back * Time.fixedDeltaTime * (float)noteSpeed);
    }

    public void CalculateBeat(double bpm)
    {
        double secPerBeat = 60f / bpm; // 1비트당 시간
        noteBeat = noteTime * secPerBeat; // 노트가 몇 번째 비트에서 등장하는지 계산

        double noteTravelTime = lengthInBeats * secPerBeat; // 노트가 레일을 통과하는 데 걸리는 시간
        noteSpeed = railLength / noteTravelTime; // 속도 계산
        //noteSpeed -= offset;
        //noteSpeed = railLength / noteBeat; // 속도 계산
    }
}
