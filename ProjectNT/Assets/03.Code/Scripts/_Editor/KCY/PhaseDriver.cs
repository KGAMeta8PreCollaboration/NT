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
        if (linkedPhase.Count == 10)
        {
            //TODO : 더이상 페이즈를 생성할 수 없습니다. 팝업 띄우기
            return;
        }
        GameObject newPhase =
        Instantiate(newPhasePrefab, phaseRect, false);
        ReplaceAddBTN();
        PhaseElement temp = newPhase.GetComponent<PhaseElement>();
        temp.modeDiff = currentModeDiff;
        linkedPhase.AddLast(temp);

        if (!byDifficulty.ContainsKey(currentModeDiff))
            byDifficulty.Add(currentModeDiff, linkedPhase.Count);
        else byDifficulty[currentModeDiff] = linkedPhase.Count;
        temp.phaseNum = byDifficulty[currentModeDiff];
        temp.action += temp.SaveAction;
        StartCoroutine(ScrollBarCtrl());
    }
    public void SwapPhaseUp(PhaseElement other)
    {
        if (linkedPhase.Find(other).Previous == null)
        {
            Debug.Log("이전노드 없음");
            return;
        }
        LinkedListNode<PhaseElement> prevNode = linkedPhase.Find(other).Previous;
        linkedPhase.Remove(other);
        linkedPhase.AddBefore(prevNode, other);
        int tempNum = prevNode.Value.phaseNum;
        prevNode.Value.phaseNum = other.phaseNum;
        other.phaseNum = tempNum;
        foreach (PhaseElement phase in linkedPhase)
        {
            phase.gameObject.transform.SetParent(null);
            phase.gameObject.transform.SetParent(phaseRect);
        }
        ReplaceAddBTN();
    }
    public void SwapPhaseDown(PhaseElement other)
    {
        if (linkedPhase.Find(other).Next == null)
        {
            Debug.Log("다음노드 없음");
            return;
        }
        LinkedListNode<PhaseElement> nextNode = linkedPhase.Find(other).Next;
        linkedPhase.Remove(other);
        linkedPhase.AddAfter(nextNode, other);
        int tempNum = nextNode.Value.phaseNum;
        nextNode.Value.phaseNum = other.phaseNum;
        other.phaseNum = tempNum;
        foreach (PhaseElement phase in linkedPhase)
        {
            phase.gameObject.transform.SetParent(null);
            phase.gameObject.transform.SetParent(phaseRect);
        }
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
