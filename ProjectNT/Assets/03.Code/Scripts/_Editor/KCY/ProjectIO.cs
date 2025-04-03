using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectIO : MonoBehaviour
{
    [SerializeField] private SetEditorEnv editorEnv;
    [SerializeField] private Button return_BTN;
    public TextMeshProUGUI pathVisual_TMP;
    public string ProjectPath
    {
        get { return editorEnv.ProjectPath; }
    }
    private void Awake()
    {
        if (Directory.Exists(editorEnv.ProjectPath))
        {
            Debug.Log("파일 경로 확인");
        }
        else
        {
            Debug.LogError("파일 경로를 읽어올 수 없습니다.");
        }
    }

    private void OnEnable()
    {
        pathVisual_TMP.text = editorEnv.ProjectPath;
    }
    //TODO 프로젝트의 저장정보를 ProjectLoader에게 보내기
}
