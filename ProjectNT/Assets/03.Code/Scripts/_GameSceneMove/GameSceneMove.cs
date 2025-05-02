using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class MapmovePosTime
{
    public Transform startPos;
    public Transform endPos;
    public float travelTime;
}

public class GameSceneMove : MonoBehaviour
{
    public Transform movingObject;
    public List<MapmovePosTime> mapmovePosTimes;
    private Sequence moveSequence;

    private void SetSequence(MapmovePosTime posNtime)
    {
        if (true == moveSequence.IsActive())
        {
            moveSequence.Kill(true);
        }
        else
        {
            Debug.LogWarning("시퀸스아직 살아있음");
        }
        moveSequence = DOTween.Sequence();
        // 만약 VR로 빌드 시 렉이 걸리면 0.2f => 0으로 변경
        moveSequence.Join(movingObject.DOMove(posNtime.startPos.position, 0.2f).SetEase(Ease.Linear)).
                     Append(movingObject.DOMove(posNtime.endPos.position, posNtime.travelTime).SetEase(Ease.Linear));
        try
        {
            moveSequence.Play();
        }
        catch (Exception e)
        {
            Debug.LogError("시퀸스플레이문제");
            Debug.LogError(e.Message);
        }
    }

    //이 함수불러서 이동 바로시작
    public void MapMoveByPhase(Enums.Phase phase)
    {
        switch (phase)
        {
            case Enums.Phase.Phase1:
                SetSequence(mapmovePosTimes[0]);
                break;
            case Enums.Phase.Phase2:
                SetSequence(mapmovePosTimes[0]);
                break;
            case Enums.Phase.Phase3:
                SetSequence(mapmovePosTimes[0]);
                break;
        }
        mapmovePosTimes.RemoveAt(0);
    }
}

