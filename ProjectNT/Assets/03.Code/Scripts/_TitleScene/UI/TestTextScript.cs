using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestTextScript : MonoBehaviour
{
    public TextMeshProUGUI testText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Test();
    }

    public void Test()
    {
        testText.text = "노래제목 : " + TestStartGameData.Instance.musicName + "\n"
            + "난이도 : " + TestStartGameData.Instance.difficulty;
    }
}
