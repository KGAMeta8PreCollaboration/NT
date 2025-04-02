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
    public Toggle[] toggles;
    private UpperNode[] upperNodes;

    public int _currentGridIndex = -1;
    private Dictionary<int, List<int>> _upperToggleDic = new Dictionary<int, List<int>>();
    private List<int> _upperToggles = new List<int>();

    private void Awake()
    {
        upperNodes = GetComponentsInChildren<UpperNode>();
        for (int i = 0; i < upperNodes.Length; i++)
        {
            upperNodes[i].SetUpperNodeIndex(i);
        }

        _upperToggles = new List<int>();
        toggles = GetComponentsInChildren<Toggle>();

        for (int i = 0; i < toggles.Length; i++)
        {
            Toggle toggle = toggles[i];
            int index = i;

            toggle.group = null;
            toggle.isOn = false;

            TextMeshProUGUI text = toggle.GetComponentInChildren<TextMeshProUGUI>();
            text.text = toggle.name;

            Image toggleImage = toggle.gameObject.GetComponent<Image>();
            toggleImage.color = normalColor;

            toggle.onValueChanged.AddListener((isOn) => OnClickToggle(isOn, toggle, i));
        }
    }

    private void OnClickToggle(bool isOn, Toggle toggle, int index)
    {
        if (_currentGridIndex < 0)
        {
            // 유효하지 않은 그리드에서는 토글 상태 변경 취소
            toggle.isOn = false;
            toggle.gameObject.GetComponent<Image>().color = normalColor;
            return;
        }

        Image toggleImage = toggle.gameObject.GetComponent<Image>();
        toggleImage.color = isOn ? selectedColor : normalColor;

        //상단 노드 추가 및 제거
        if (isOn)
        {
            if (!_upperToggles.Contains(index))
                _upperToggles.Add(index);
            print($"인덱스 : {index}");
        }
        else
        {
            _upperToggles.Remove(index);
        }

        // 현재 토글 상태를 Dictionary에 저장
        if (!_upperToggleDic.ContainsKey(_currentGridIndex))
        {
            _upperToggleDic[_currentGridIndex] = new List<int>();
        }
        _upperToggleDic[_currentGridIndex] = new List<int>(_upperToggles);
    }

    public void SetText(int grid)
    {
        if (_currentGridIndex == grid)
            return;

        _currentGridIndex = grid;
        text.text = grid.ToString();

        // 그리드가 유효하지 않으면 토글 초기화만 하고 리턴
        if (grid < 0)
        {
            ClearToggles();
            return;
        }

        _currentGridIndex = grid;
        text.text = grid.ToString();

        // 새로운 그리드의 이전 토글 상태 복원
        if (_upperToggleDic.ContainsKey(grid))
        {
            _upperToggles = new List<int>(_upperToggleDic[grid]);
            foreach (int toggleIndex in _upperToggles)
            {
                toggles[toggleIndex].isOn = true;
                // 토글 색상도 업데이트
                toggles[toggleIndex].gameObject.GetComponent<Image>().color = selectedColor;
            }
        }
        else
        {
            ClearToggles();
        }
    }

    public List<int> GetSelectedToggle()
    {
        return new List<int>(_upperToggles);
    }

    public void ClearToggles()
    {
        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = false;
            // 토글 색상도 초기화
            toggle.gameObject.GetComponent<Image>().color = normalColor;
        }
        _upperToggles.Clear();
    }
}
