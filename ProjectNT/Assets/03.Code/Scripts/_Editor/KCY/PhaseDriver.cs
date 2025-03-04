using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PhaseDriver : MonoBehaviour
{
    [SerializeField] private GameObject newPhasePrefab;
    [SerializeField] private Button addPhase;
    [SerializeField] private RectTransform phaseRect;
    [SerializeField] private Scrollbar phaseScrollBar;
    [SerializeField] private RectTransform addBTNRect;
    public LinkedList<PhaseElement> linkedPhase = new LinkedList<PhaseElement>();

    private Dictionary<Enums.ModeDiff, int> byDifficulty = new Dictionary<Enums.ModeDiff, int>();

    private Enums.ModeDiff currentModeDiff;

    public Enums.ModeDiff modeDiff
    {
        get { return currentModeDiff; }

        set { currentModeDiff = value; }
    }

    private void Awake()
    {
        Initialize();
    }
    private void AddNewPhase()
    {
        //페이즈 10개 생성 시 추가생성 불가
        if (linkedPhase.Count == 10)
        {
            //TODO : 더이상 페이즈를 생성할 수 없습니다. 팝업 띄우기
            return;
        }

        GameObject newPhase =
        Instantiate(newPhasePrefab, phaseRect, false);
        ReplaceAddBTN();
        PhaseElement temp = newPhase.GetComponent<PhaseElement>();
        //현재 난이도에 따라 페이즈 난이도 설정
        temp.modeDiff = currentModeDiff;
        linkedPhase.AddLast(temp);
        //페이즈 별 번호 추가
        if (!byDifficulty.ContainsKey(currentModeDiff))
        {
            byDifficulty.Add(currentModeDiff, linkedPhase.Count);
        }
        else
        {
            byDifficulty[currentModeDiff] = linkedPhase.Count;
        }
        temp.phaseNum = byDifficulty[currentModeDiff];
        //저장 액션 등록
        temp.action += temp.SaveAction;

        //새로운 페이즈 추가시 스크롤바 Value변경
        StartCoroutine(ScrollBarCtrl());
    }
    public void SwapPhaseUp(PhaseElement other)
    {
        //이전노드 없을시 리턴
        if (linkedPhase.Find(other).Previous == null)
        {
            Debug.Log("이전노드 없음");
            return;
        }

        //변경할 노드 찾음
        LinkedListNode<PhaseElement> prevNode = linkedPhase.Find(other).Previous;
        linkedPhase.Remove(other);
        linkedPhase.AddBefore(prevNode, other);

        //페이즈 번호 스왑
        int tempNum = prevNode.Value.phaseNum;
        prevNode.Value.phaseNum = other.phaseNum;
        other.phaseNum = tempNum;

        //페이즈 RectTransform 정렬
        foreach (PhaseElement phase in linkedPhase)
        {
            phase.gameObject.transform.SetParent(null);
            phase.gameObject.transform.SetParent(phaseRect);
        }

        //페이즈 추가버튼 정렬
        ReplaceAddBTN();
    }
    public void SwapPhaseDown(PhaseElement other)
    {
        //다음노드 없을시 리턴
        if (linkedPhase.Find(other).Next == null)
        {
            Debug.Log("다음노드 없음");
            return;
        }

        //변경할 노드 찾음
        LinkedListNode<PhaseElement> nextNode = linkedPhase.Find(other).Next;
        linkedPhase.Remove(other);
        linkedPhase.AddAfter(nextNode, other);

        //페이즈 번호 스왑
        int tempNum = nextNode.Value.phaseNum;
        nextNode.Value.phaseNum = other.phaseNum;
        other.phaseNum = tempNum;

        //페이즈 RectTransform 정렬
        foreach (PhaseElement phase in linkedPhase)
        {
            phase.gameObject.transform.SetParent(null);
            phase.gameObject.transform.SetParent(phaseRect);
        }

        //페이즈 추가버튼 정렬
        ReplaceAddBTN();
    }
    private IEnumerator ScrollBarCtrl()
    {
        yield return null;
        yield return null;
        phaseScrollBar.value = 0;
    }

    private void ReplaceAddBTN()
    {
        addPhase.transform.SetParent(null);
        addPhase.transform.SetParent(phaseRect);
    }

    private void Initialize()
    {
        if (newPhasePrefab == null)
            newPhasePrefab = Resources.Load<GameObject>("_SongEditor/Prefabs/AudioSource_Info");

        addPhase.onClick.AddListener(AddNewPhase);
        currentModeDiff = Enums.ModeDiff.SOLO_EASY;
    }
}
