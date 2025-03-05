using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
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
    public LinkedList<Phase> linkedPhase = new LinkedList<Phase>();

    private Dictionary<Enums.ModeDiff, int> byDifficulty = new Dictionary<Enums.ModeDiff, int>();

    private Enums.ModeDiff m_ModeDiff;

    public Enums.ModeDiff modeDiff
    {
        get { return m_ModeDiff; }
        set { m_ModeDiff = value; }
    }

    private void Awake()
    {

    }

    private void Start()
    {
        //세이브로드 이벤트 구독
        // ResourceIO.Instance.saveDelegate += AddDataList;
        // ResourceIO.Instance.loadDelegate += LoadData;
    }

    private void OnDestroy()
    {
        //세이브로드 이벤트 구독 해제
        //     ResourceIO.Instance.saveDelegate -= AddDataList;
        //     ResourceIO.Instance.loadDelegate -= LoadData;
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
        Phase temp = newPhase.GetComponent<Phase>();
        //현재 난이도에 따라 페이즈 난이도 설정
        temp.modeDiff = m_ModeDiff;
        linkedPhase.AddLast(temp);
        //페이즈 별 번호 추가
        if (!byDifficulty.ContainsKey(m_ModeDiff))
        {
            byDifficulty.Add(m_ModeDiff, linkedPhase.Count);
        }
        else
        {
            byDifficulty[m_ModeDiff] = linkedPhase.Count;
        }
        temp.phaseNum = byDifficulty[m_ModeDiff];
        //저장 델리게이트 등록

        //새로운 페이즈 추가시 스크롤바 Value변경
        if (gameObject.activeSelf) StartCoroutine(ScrollBarCtrl());
    }
    public void SwapPhaseUp(Phase other)
    {
        //이전페이즈 없을시 리턴
        if (linkedPhase.Find(other).Previous == null)
        {
            Debug.Log("이전페이즈 없음");
            return;
        }

        //변경할 페이즈 찾음
        LinkedListNode<Phase> prevNode = linkedPhase.Find(other).Previous;
        linkedPhase.Remove(other);
        linkedPhase.AddBefore(prevNode, other);

        //페이즈 번호 스왑
        int tempNum = prevNode.Value.phaseNum;
        prevNode.Value.phaseNum = other.phaseNum;
        other.phaseNum = tempNum;

        //페이즈 RectTransform 정렬
        foreach (Phase phase in linkedPhase)
        {
            phase.gameObject.transform.SetParent(null);
            phase.gameObject.transform.SetParent(phaseRect);
        }

        //페이즈 추가버튼 정렬
        ReplaceAddBTN();
    }
    public void SwapPhaseDown(Phase other)
    {
        //다음페이즈 없을시 리턴
        if (linkedPhase.Find(other).Next == null)
        {
            Debug.Log("다음페이즈 없음");
            return;
        }

        //변경할 페이즈 찾음
        LinkedListNode<Phase> nextNode = linkedPhase.Find(other).Next;
        linkedPhase.Remove(other);
        linkedPhase.AddAfter(nextNode, other);

        //페이즈 번호 스왑
        int tempNum = nextNode.Value.phaseNum;
        nextNode.Value.phaseNum = other.phaseNum;
        other.phaseNum = tempNum;

        //페이즈 RectTransform 정렬
        foreach (Phase phase in linkedPhase)
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

    public void Initialize()
    {
        if (newPhasePrefab == null)
            newPhasePrefab = Resources.Load<GameObject>("_SongEditor/Prefabs/AudioSource_Info");

        addPhase.onClick.AddListener(AddNewPhase);
    }
    public void AddDataList()
    {
        List<SongData> dataList = new List<SongData>();
        foreach (Phase phase in linkedPhase)
        {
            dataList.Add(phase.m_SongData);
        }
        ResourceIO.Instance.Phase_Dic[m_ModeDiff] = dataList;
    }
    public void LoadData()
    {
        if (!ResourceIO.Instance.Phase_Dic.ContainsKey(m_ModeDiff)) return;
        List<SongData> dataList = new List<SongData>();
        dataList = ResourceIO.Instance.Phase_Dic[m_ModeDiff];
        for (int i = 0; i < dataList.Count; i++)
        {
            AddNewPhase();
        }
        int temp = 0;
        foreach (Phase phase in linkedPhase)
        {
            phase.m_SongData = dataList[temp];
            temp++;
        }
    }
}
