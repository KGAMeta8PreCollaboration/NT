using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBoardHandler : MonoBehaviour
{
    public Action changeDisplayBoardCallback;
    public Action turnOnDisplayBoardCallback;
    public Action setDisplayOriginalCallback;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            changeDisplayBoardCallback?.Invoke();
        }   
        if (Input.GetKeyDown(KeyCode.L))
        {
            turnOnDisplayBoardCallback?.Invoke();
        }
    }

    public void ChangeDisplayBoard()
    {
        changeDisplayBoardCallback?.Invoke();
    }

    public void TurnOnDisplayBoard()
    {
        turnOnDisplayBoardCallback?.Invoke();
    }

    public void SetDisplayOriginal()
    {
        setDisplayOriginalCallback?.Invoke();
    }
}
