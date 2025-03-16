using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNodeInfo : MonoBehaviour
{
    public string CurrentNodeInfo => _currentNodeInfo;

    public string _currentNodeInfo;

    private TestButtonType[] _testButtonType;

    private void Awake()
    {
        _testButtonType = FindObjectsOfType<TestButtonType>();
        foreach (var button in _testButtonType)
        {
            button.keySound += SetCurrentNodeInfo;
        }
    }
    private void SetCurrentNodeInfo(string keySound)
    {
        _currentNodeInfo = keySound;
    }
}
