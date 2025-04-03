using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopNoteIndicater : MonoBehaviour
{
    public Action OnHit;

    private void OnEnable()
    {
        OnHit += Push;
    }
    private void OnDisable()
    {
        OnHit -= Push;
    }
    private void Push()
    {
        PoolManager.Instance.topNoteIndicaterPool.Push(this);
    }
}
