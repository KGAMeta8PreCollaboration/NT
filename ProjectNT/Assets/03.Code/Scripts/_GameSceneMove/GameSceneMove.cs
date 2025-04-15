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
    // public GameSceneLightController lightController;

    [SerializeField]
    public List<MapmovePosTime> mapmovePosTimes;

    private int testNum = 0;
    private Action action;

    private Sequence moveSequence;

    // private void Start()
    // {
    //     GameSceneMoveAndLightStart();
    // }
    private void Awake()
    {
    }
    private void SetSequence(MapmovePosTime posNtime)
    {
        movingObject.DOMove(posNtime.startPos.position, 0f).SetEase(Ease.Linear);
        movingObject.DOMove(posNtime.endPos.position, posNtime.travelTime).SetEase(Ease.Linear);
    }
    public void GameSceneMoveAndLightStart(Enums.Phase phase)//이 함수불러서 이동 바로시작
    {
        //StartCoroutine(TestOnLight());//얜 테스트용입니다
        // StartCoroutine(MoveStart());
        //게임시작과동시에 실행할때 다른것도하는게많아서그런지
        //코루틴이 조금늦게시작함 한 2초정도
        //빛을키고싶을때 lightController.OnLight(); 를 호출하면 됨
        //빛강도, 시간은 인스펙터에서 조정가능
        switch (phase)
        {
            case Enums.Phase.Phase1:
                SetSequence(mapmovePosTimes[0]);
                Debug.LogError("1");
                break;
            case Enums.Phase.Phase2:
                SetSequence(mapmovePosTimes[0]);
                Debug.LogError("2");
                break;
            case Enums.Phase.Phase3:
                SetSequence(mapmovePosTimes[0]);
                Debug.LogError("3");
                break;
        }
        mapmovePosTimes.RemoveAt(0);
        Debug.LogError(mapmovePosTimes.Count);
    }
}

// private IEnumerator MoveStart()
// {
//     Debug.Log("시작");

//     Debug.Log("1페이즈 시작");
//     //1페이즈 오브젝트 추가
//     lightController.AddFirstLightObject();
//     //95%정도 이동하면 2페이즈 오브젝트도 추가
//     action += lightController.AddSecondLightObject;
//     yield return StartCoroutine(MoveToPos(first));
//     //1페이즈 끝나면 1페이즈 오브젝트 제거
//     lightController.RemoveFirstLightObject();

//     Debug.Log("2페이즈 시작");
//     //95%정도 이동하면 3페이즈 오브젝트도 추가
//     action += lightController.AddThirdLightObject;
//     yield return StartCoroutine(MoveToPos(second));
//     //2페이즈 끝나면 2페이즈 오브젝트 제거
//     lightController.RemoveSecondLightObject();

//     Debug.Log("3페이즈 시작");
//     yield return StartCoroutine(MoveToPos(third));
//     Debug.Log("종료");
// }

//     public IEnumerator MoveToPos(MapmovePosTime posAndTime)
//     {
//         testNum++;
//         Debug.Log($"{testNum}번째 이동 시작");

//         Vector3 startPos = posAndTime.endPos.transform.position;
//         Vector3 endPos = posAndTime.endPos.transform.position;
//         float travelTime = posAndTime.travelTime;

//         float timeElapsed = 0f;

//         float distance = Vector3.Distance(startPos, endPos);//시작지점과 도착지점의 총 거리

//         float speed = distance / travelTime;//1초동안 이동할 거리

//         Vector3 curPos = startPos;//시작지점을 startPos로

//         while (timeElapsed < travelTime)
//         {
//             timeElapsed += Time.deltaTime;

//             Vector3 dir = (curPos - endPos).normalized;//이동방향

//             float distanceToMove = speed * Time.deltaTime;

//             curPos += dir * distanceToMove;//현재위치를 이동

//             if (action != null && Vector3.Distance(curPos, endPos) < distance * 0.05f)//95% 진행했을 때
//             {
//                 action?.Invoke();//다음 페이즈 오브젝트 추가
//                 action = null;
//             }

//             if (Vector3.Distance(curPos, endPos) < 0.1f)
//             {
//                 curPos = endPos;
//                 break;
//             }

//             movingObject.transform.localPosition = curPos;//오브젝트를 이동

//             yield return null;
//         }
//         movingObject.transform.localPosition = endPos;

//         Debug.Log($"{testNum}번째 이동 종료");
//     }

//     [Header("테스트용")]
//     public int lightDuration;
//     private float testTime = 0f;

//     public IEnumerator TestOnLight()//테스트용
//     {
//         float lastTestTime = 0f;

//         while (true)
//         {
//             testTime += Time.deltaTime;

//             // testTime이 lightDuration초마다 증가할 때마다 호출
//             if (Mathf.FloorToInt(testTime / lightDuration) > Mathf.FloorToInt(lastTestTime / lightDuration))
//             {
//                 lightController.OnLight();
//             }

//             lastTestTime = testTime;

//             yield return null;
//         }
//     }
// }
