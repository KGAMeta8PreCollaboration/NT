using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorLoadScene : MonoBehaviour
{
    public static string nextScene;

    private void Start()
    {
        StartCoroutine(LoadSceneCorou());
    }
    public static void SceneLoad(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene(nextScene);
    }
    private IEnumerator LoadSceneCorou()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            if (op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }
}
