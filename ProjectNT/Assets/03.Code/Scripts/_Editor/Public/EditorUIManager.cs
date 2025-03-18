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
            if (SceneManager.GetActiveScene().name == "EditorPathScene")
            {
                if (pathCanvas == null)
                    pathCanvas = FindObjectOfType<SetEditorEnv>().gameObject;
            }
            if (SceneManager.GetActiveScene().name == "SongEditorScene")
            {
                if (editorCanvas == null)
                {
                    editorCanvas = FindObjectOfType<ResourceIO>().gameObject;
                }
            }
        };

    }

}
