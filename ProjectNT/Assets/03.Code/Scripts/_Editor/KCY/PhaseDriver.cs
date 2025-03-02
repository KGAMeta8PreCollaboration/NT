using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhaseDriver : MonoBehaviour
{
    [SerializeField] private GameObject newPhasePrefab;
    [SerializeField] private Button addPhase;
    [SerializeField] private RectTransform phaseRect;
    [SerializeField] private Scrollbar phaseScrollBar;
    [SerializeField] private RectTransform addPhaseRect;
    private LinkedList<PhaseElement> linkedPhase = new LinkedList<PhaseElement>();
    public event Action<float> OnRectChanged;
    private void Awake()
    {
        if (newPhasePrefab == null)
        {
            newPhasePrefab = Resources.Load<GameObject>("_SongEditor/Prefabs/AudioSource_Info");
        }
        addPhase.onClick.AddListener(AddNewPhase);
    }
    int num = 0;
    private void AddNewPhase()
    {
        if (linkedPhase.Count == 10)
        {
            //TODO : 더이상 페이즈를 생성할 수 없습니다. 팝업 띄우기
            return;
        }
        GameObject newPhase =
        Instantiate(newPhasePrefab, phaseRect, false);
        SetAddBTN();
        PhaseElement temp = newPhase.GetComponent<PhaseElement>();
        temp.num = num;
        linkedPhase.AddLast(temp);
        StartCoroutine(ScrollBarCtrl());
        num++;
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

        foreach (PhaseElement phase in linkedPhase)
        {
            phase.gameObject.transform.SetParent(null);
        }
        foreach (PhaseElement phase in linkedPhase)
        {
            phase.gameObject.transform.SetParent(phaseRect);
        }
        SetAddBTN();
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

        foreach (PhaseElement phase in linkedPhase)
        {
            phase.gameObject.transform.SetParent(null);
        }
        foreach (PhaseElement phase in linkedPhase)
        {
            phase.gameObject.transform.SetParent(phaseRect);
        }
        SetAddBTN();
    }
    private IEnumerator ScrollBarCtrl()
    {
        yield return null;
        yield return null;
        phaseScrollBar.value = 0;
    }

    private void SetAddBTN()
    {
        addPhase.transform.SetParent(null);
        addPhase.transform.SetParent(phaseRect);
    }

}
