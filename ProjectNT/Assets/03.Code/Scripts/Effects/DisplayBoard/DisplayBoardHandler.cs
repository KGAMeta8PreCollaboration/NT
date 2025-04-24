using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBoardHandler : MonoBehaviour
{
    public Action displayBoardCallback;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            displayBoardCallback?.Invoke();
        }   
    }
}
