using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorUIManager : Singleton<EditorUIManager>
{

    public GameObject editorCanvas;
    public GameObject pathCanvas;
    public PopUp popUp;

    protected override void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += (x, y) =>
        {
            //TODO 씬 전환 기능 추가 시 씬마다 필요한 캔버스 가져오기

        };

    }

}
