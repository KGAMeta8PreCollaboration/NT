using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpperNodeHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;
    //[SerializeField] private RectTransform leftNodeGroup; 
    //[SerializeField] private RectTransform rightNodeGroup;
    //[SerializeField] private Camera mainCamera;

    //private PlayBar _playBar;

    //public Toggle[] toggles;
    private List<UpperNode> upperNodes = new List<UpperNode>();

    public int _currentGridIndex = -1;
    public Dictionary<int, List<int>> _upperNodeDic = new Dictionary<int, List<int>>();
    private List<int> _upperToggles = new List<int>();

    private void Awake()
    {
        //_upperToggles = new List<int>();
        //toggles = GetComponentsInChildren<Toggle>();
        upperNodes = GetComponentsInChildren<UpperNode>().ToList();

        //_playBar = FindObjectOfType<PlayBar>();

        for (int i = 0; i < upperNodes.Count; i++)
        {
            //Toggle toggle = toggles[i];
            int index = i;
            //상단 노드들에게 index부여
            upperNodes[i].SetUpperNodeIndex(index);
            //TextMeshProUGUI text = toggle.GetComponentInChildren<TextMeshProUGUI>();
            //text.text = toggle.name;

            Toggle toggle = upperNodes[i].GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, index));
        }
    }

    //private void Start()
    //{
    //    Init();
    //}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        foreach (var node in _upperNodeDic)
    //        {
    //            int[] temp = node.Value.ToArray();
    //            print($"grid : {node.Key}, index : {temp}");
    //        }
    //    }
    //}

    //private void Init()
    //{
    //    if (_playBar == null) return;

    //    Vector3 playBarLeft = mainCamera.WorldToScreenPoint(_playBar.transform.position - (_playBar.transform.localScale.x * 0.5f * Vector3.right));
    //    Vector3 playBarRight = mainCamera.WorldToScreenPoint(_playBar.transform.position + (_playBar.transform.localScale.x * 0.5f * Vector3.right));

    //    // Canvas 좌표로 변환
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        (RectTransform)transform,
    //        playBarLeft,
    //        null,
    //        out Vector2 leftAnchoredPosition);

    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        (RectTransform)transform,
    //        playBarRight,
    //        null,
    //        out Vector2 rightAnchoredPosition);

    //    // 노드 그룹의 위치 업데이트
    //    if (leftNodeGroup != null)
    //        leftNodeGroup.anchoredPosition = leftAnchoredPosition;

    //    if (rightNodeGroup != null)
    //        rightNodeGroup.anchoredPosition = rightAnchoredPosition;
    //}

    private Dictionary<int, Dictionary<int, string>> _gridKeySoundDic = new Dictionary<int, Dictionary<int, string>>();
    private void OnToggleChanged(bool isOn, int index)
    {
        if (string.IsNullOrEmpty(EditorDataManager.Instance.CurKeySoundName))
        {
            Debug.LogWarning("키음이 없음");
            return;
        }

        if (_currentGridIndex < 0)
            return;

        if (!_gridKeySoundDic.ContainsKey(_currentGridIndex))
        {
            _gridKeySoundDic[_currentGridIndex] = new Dictionary<int, string>();
        }

        //상단 노드 추가 및 제거
        if (isOn)
        {
            if (!_upperToggles.Contains(index))
            {
                _upperToggles.Add(index);
                _gridKeySoundDic[_currentGridIndex][index] = EditorDataManager.Instance.CurKeySoundName;
            }
        }
        else
        {
            _upperToggles.Remove(index);
            _gridKeySoundDic[_currentGridIndex].Remove(index);
        }

        // 현재 토글 상태를 Dictionary에 저장
        if (_upperToggles.Count > 0)
        {
            _upperNodeDic[_currentGridIndex] = new List<int>(_upperToggles);
        }
        else
        {
            _upperNodeDic.Remove(_currentGridIndex);
        }

        UpdateAllNodes();
    }

    public void GetGridIndex(int grid)
    {
        if (_currentGridIndex == grid || grid < 0) return;

        _currentGridIndex = grid;
        text.text = grid.ToString();

        //새로운 그리드일때 한번 초기화
        _upperToggles.Clear();
        if (_upperNodeDic.TryGetValue(grid, out var toggles))
        {
            _upperToggles.AddRange(toggles);
        }

        // 현재 그리드의 키음 정보 복원
        if (_gridKeySoundDic.TryGetValue(grid, out var keySounds))
        {
            foreach (var node in upperNodes)
            {
                if (keySounds.TryGetValue(node.Index, out string keySound))
                {
                    node.SetKeySound(keySound);
                }
            }
        }

        UpdateAllNodes();
    }

    public List<int> GetSelectedToggle()
    {
        return new List<int>(_upperToggles);
    }

    private void UpdateAllNodes()
    {
        foreach (var node in upperNodes)
        {
            bool isActive = _upperToggles.Contains(node.Index);
            if (isActive && _gridKeySoundDic.TryGetValue(_currentGridIndex, out var keySounds))
            {
                keySounds.TryGetValue(node.Index, out string keySound);
                node.SetKeySound(keySound ?? "");
            }
            node.UpdateState(_upperToggles);
        }
    }

    public void InitializeWithNodeData(List<UpperNodeData> nodeDatas)
    {
        // 기존 데이터 초기화
        _upperNodeDic.Clear();
        _upperToggles.Clear();
        _gridKeySoundDic.Clear();
        _currentGridIndex = -1;

        if (nodeDatas == null) return;

        foreach (var nodeData in nodeDatas)
        {
            _upperNodeDic[nodeData.gridIndex] = new List<int>(nodeData.nodeIndexs);

            // 각 그리드의 키음 정보 초기화
            if (!_gridKeySoundDic.ContainsKey(nodeData.gridIndex))
            {
                _gridKeySoundDic[nodeData.gridIndex] = new Dictionary<int, string>();
            }

            for (int i = 0; i < nodeData.nodeIndexs.Count; i++)
            {
                int nodeIndex = nodeData.nodeIndexs[i];
                string keySound = nodeData.keySounds[i];
                _gridKeySoundDic[nodeData.gridIndex][nodeIndex] = keySound;

                // 노드에 키음 설정
                var node = upperNodes.FirstOrDefault(n => n.Index == nodeIndex);
                if (node != null)
                {
                    node.SetKeySound(keySound);
                }
            }
        }

        // 첫 번째 그리드 인덱스로 초기화
        if (_upperNodeDic.Count > 0)
        {
            GetGridIndex(_upperNodeDic.Keys.First());
        }

        UpdateAllNodes();
    }

    public string GetNodeKeySoundByIndex(int nodeIndex)
    {
        // upperNodes 리스트에서 해당 인덱스의 노드를 찾음
        var node = upperNodes.FirstOrDefault(n => n.Index == nodeIndex);
        if (node != null)
        {
            return node._keySound;
        }
        return "";
    }
}
