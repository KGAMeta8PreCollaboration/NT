using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorUIManager : MonoBehaviour
{
    private static EditorUIManager instance;
    public static EditorUIManager Instance { get { return instance; } }
    public GameObject editorCanvas;
    public GameObject pathCanvas;
    public GameObject popupCanvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else DestroyImmediate(gameObject);

        SceneManager.sceneLoaded += (x, y) =>
        {
            //TODO 씬 전환 기능 추가 시 씬마다 필요한 캔버스 가져오기

        };

    }

}
