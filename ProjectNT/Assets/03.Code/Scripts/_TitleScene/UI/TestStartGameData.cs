using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStartGameData : MonoBehaviour
{
    public static TestStartGameData Instance;

    public string musicName;
    public int difficulty;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
