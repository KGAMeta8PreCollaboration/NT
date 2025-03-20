using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LongNoteSpawner : MonoBehaviour
{
    public Transform spawnPoint; // 노트 생성 위치
    public Transform target; // 노트가 이동할 목표 지점
    public LongNote longNotePrefab; // 롱노트 프리팹
    public AudioClip hitSound; // 롱노트 히트 사운드

    private IEnumerator Start()
    {
        // while (true)
        //{
        SpawnLongNote(AudioSettings.dspTime + 1, AudioSettings.dspTime + 1, AudioSettings.dspTime + 5);
        yield return new WaitForSeconds(5f);
        //}
    }

    public void SpawnLongNote(double spawnDspTime, double startTargetDspTime, double endTargetDspTime)
    {
        // 지정된 DspTime에서 정확하게 스폰
        StartCoroutine(SpawnRoutine(spawnDspTime, startTargetDspTime, endTargetDspTime));
    }

    private IEnumerator SpawnRoutine(double spawnDspTime, double startTargetDspTime, double endTargetDspTime)
    {
        // 현재 시간보다 스폰 시간이 뒤라면 대기
        while (AudioSettings.dspTime < spawnDspTime)
        {
            yield return null;
        }

        LongNote newNote = Instantiate(longNotePrefab, spawnPoint.position, Quaternion.identity);
        newNote.Init(target, spawnDspTime, startTargetDspTime, endTargetDspTime, hitSound);

        Debug.Log($"롱노트 생성됨! 생성 시간: {spawnDspTime:F2}, 목표 시작 시간: {startTargetDspTime:F2}, 목표 종료 시간: {endTargetDspTime:F2}");
    }
}
