using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INodeState
{
    void OnLeftClick(Vector2Int position);
    void OnRightClick(Vector2Int position);
    void OnMiddleClick(Vector2Int position);
    void UpdatePreview(Vector2Int position);
    string GetStateName();
}
