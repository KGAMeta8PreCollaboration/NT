using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectIO : MonoBehaviour
{
    [SerializeField] private SetEditorEnv editorEnv;
    [SerializeField] private Button return_BTN;
    public TextMeshProUGUI pathVisual_TMP;

    private void Awake()
    {
        Debug.Log(editorEnv.Path.ProjectPath);
        pathVisual_TMP.text = editorEnv.Path.ProjectPath;
    }
    //TODO 프로젝트의 저장정보를 ProjectLoader에게 보내기
}
