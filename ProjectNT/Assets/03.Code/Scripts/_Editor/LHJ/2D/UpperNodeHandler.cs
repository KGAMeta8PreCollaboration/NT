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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var node in _upperNodeDic)
            {
                int[] temp = node.Value.ToArray();
                print($"grid : {node.Key}, index : {temp}");
            }
        }
    }

    private void OnToggleChanged(bool isOn, int index)
    {
        if (_currentGridIndex < 0)
        {
            return;
        }

        //상단 노드 추가 및 제거
        if (isOn)
        {
            if (!_upperToggles.Contains(index))
                _upperToggles.Add(index);
            // 토글이 켜질 때 현재 선택된 키 사운드를 해당 노드에 저장
            var node = upperNodes.FirstOrDefault(n => n.Index == index);
            if (node != null)
            {
                node.SetKeySound(EditorDataManager.Instance.CurKeySoundName);
            }
        }
        else
        {
            _upperToggles.Remove(index);
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

        // 모든 상단 노드의 상태 업데이트
        UpdateAllNodes();
    }

    public List<int> GetSelectedToggle()
    {
        return new List<int>(_upperToggles);
    }

    private void UpdateAllNodes()
    {
        for (int i = 0; i < upperNodes.Count; i++)
        {
            upperNodes[i].UpdateState(_upperToggles);
        }
    }

    public void InitializeWithNodeData(List<UpperNodeData> nodeDatas)
    {
        // 기존 데이터 초기화
        _upperNodeDic.Clear();
        _upperToggles.Clear();
        _currentGridIndex = -1;

        if (nodeDatas == null) return;

        foreach (var nodeData in nodeDatas)
        {
            _upperNodeDic[nodeData.gridIndex] = new List<int>(nodeData.nodeIndexs);

            for (int i = 0; i < nodeData.nodeIndexs.Count; i++)
            {
                int nodeIndex = nodeData.nodeIndexs[i];
                string keySound = nodeData.keySounds[i];

                // upperNodes 리스트에서 해당 인덱스의 노드를 찾아 keySound 설정
                var node = upperNodes.FirstOrDefault(n => n.Index == nodeIndex);
                if (node != null)
                {
                    node._keySound = keySound;
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
