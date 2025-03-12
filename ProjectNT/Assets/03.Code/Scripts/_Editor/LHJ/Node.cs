using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private int _column;
    private float _beatTime;

    public int Column => _column;
    public float BeatTime => _beatTime;

    public void Initialize(int column, float beatTime)
    {
        _column = column;
        _beatTime = beatTime;
    }
}
