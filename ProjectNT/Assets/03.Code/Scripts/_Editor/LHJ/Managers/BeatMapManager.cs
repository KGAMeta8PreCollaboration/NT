using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BeatMapManager : MonoBehaviour
{
    [Header("매니저 참조")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private NodeManager nodeManager;
    [SerializeField] private TimelineManager timelineManager;
    [SerializeField] private InputManager inputManager;

    private BeatMapData _currentBeatMap;
    private string _currentBeatMapPath;
    private bool _isEditing = false;

    //에디터 상태
    private bool _isPaused = true;
    private NodeType _currentNodeType = NodeType.None;

    private void InitializeEdior()
    {
        //_currentBeatMap = new BeatMapData
        //{

        //}
    }
}
