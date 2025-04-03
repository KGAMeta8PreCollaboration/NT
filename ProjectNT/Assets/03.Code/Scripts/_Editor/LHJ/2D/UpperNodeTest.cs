using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpperNodeTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;
    //public Toggle[] toggles;
    private List<UpperNode> upperNodes = new List<UpperNode>();

    public int _currentGridIndex = -1;
    private Dictionary<int, List<int>> _upperToggleDic = new Dictionary<int, List<int>>();
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
            foreach (var node in _upperToggleDic)
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
        }
        else
        {
            _upperToggles.Remove(index);
        }

        // 현재 토글 상태를 Dictionary에 저장
        if (_upperToggles.Count > 0)
        {
            _upperToggleDic[_currentGridIndex] = new List<int>(_upperToggles);
        }
        else
        {
            _upperToggleDic.Remove(_currentGridIndex);
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
        if (_upperToggleDic.TryGetValue(grid, out var toggles))
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
}
