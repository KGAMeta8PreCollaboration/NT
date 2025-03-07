using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
[Serializable]
public class PATH
{
    public string Path;
    public string EditorPath;
    public string ProjectPath;
    public string CurrentPath;
    public string EditorDIR_Name = "\\Night Traveler_Editor";
    public string ProjectDIR_Name = "\\Projects";
}

public class SetEditorEnv : MonoBehaviour
{
    [SerializeField] private RectTransform defaultPath;
    [SerializeField] private RectTransform project;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button openFolderBTN;
    [SerializeField] private Button nextBTN;
    [SerializeField] private ProjectIO projectIO;
    [SerializeField] private Button exit_BTN;
    private string savePath = "Assets/Resources/";
    private PATH PATH = new PATH();
    private string projectPath;
    public string ProjectPath
    {
        get { return projectPath; }
    }
    private void Awake()
    {
        LoadPath();
        if (PATH.Path != null) CheckPath();
        inputField.text = PATH.Path;
        openFolderBTN.onClick.AddListener(OpenExplorer);
        nextBTN.onClick.AddListener(CheckPath);
        exit_BTN.onClick.AddListener(Exit_BTN);
    }

    private void Exit_BTN()
    {
        //TODO  세이브
#if UNITY_EDITOR
        //유니티 플레이 종료
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //어플리케이션 종료
        Application.Quit(); 
#endif
    }

    private void CheckPath()
    {
        if (!Directory.Exists(PATH.Path + PATH.EditorDIR_Name))
        {
            Directory.CreateDirectory(PATH.Path + PATH.EditorDIR_Name);
            PATH.EditorPath = PATH.Path + PATH.EditorDIR_Name;
            PATH.CurrentPath = PATH.EditorPath;
            SavePath();
            Debug.Log("에디터 폴더 생성 및 경로 저장");
        }
        else PATH.EditorPath = PATH.Path + PATH.EditorDIR_Name;
        if (!Directory.Exists(PATH.EditorPath + PATH.ProjectDIR_Name))
        {
            Directory.CreateDirectory(PATH.EditorPath + PATH.ProjectDIR_Name);
            PATH.ProjectPath = PATH.EditorPath + PATH.ProjectDIR_Name;
            PATH.CurrentPath = PATH.ProjectPath;
            SavePath();
            Debug.Log("프로젝트 폴더 생성 및 경로 저장");
        }
        else PATH.ProjectPath = PATH.EditorPath + PATH.ProjectDIR_Name;
        if (Directory.Exists(PATH.ProjectPath))
        {
            projectPath = PATH.ProjectPath;
            defaultPath.gameObject.SetActive(false);
            projectIO.gameObject.SetActive(true);

        }
        else
        {
            Debug.LogWarning($"프로젝트 폴더 경로 오류\n{PATH.ProjectPath}");
        }

    }

    private void OpenExplorer()
    {
        var path = StandaloneFileBrowser.OpenFolderPanel("에디터 경로 선택", "", false);
        try
        {
            //에디터 폴더를 직접 선택한 경우
            if (Directory.Exists(path[0] + PATH.ProjectDIR_Name))
            {
                PATH.EditorPath = path[0];
                //에디터 폴더에 프로젝트 폴더가 존재하는지 확인
                if (Directory.Exists(PATH.EditorPath + PATH.ProjectDIR_Name))
                {
                    //프로젝트 폴더 경로 재설정
                    PATH.ProjectPath = PATH.EditorPath + PATH.ProjectDIR_Name;
                }
                PATH.Path = path[0].Replace(PATH.EditorDIR_Name, "");
                Debug.Log(path[0]);
                Debug.Log(PATH.Path);
                PATH.CurrentPath = PATH.Path;
                inputField.text = PATH.CurrentPath;
                SavePath();
            }
            else
            {
                PATH.Path = path[0];
                PATH.CurrentPath = PATH.Path;
                inputField.text = PATH.CurrentPath;
                SavePath();
            }
        }
        catch
        {
            Debug.LogWarning("경로 설정 중 문제");
        }
    }

    private void SavePath()
    {
        string data = JsonUtility.ToJson(PATH, true);
        File.WriteAllText(savePath + "path", data);
    }
    private void LoadPath()
    {
        if (!File.Exists(savePath + "path")) return;
        string data = File.ReadAllText(savePath + "path");
        PATH = JsonUtility.FromJson<PATH>(data);
        if (!Directory.Exists(PATH.ProjectPath))
        {
            Debug.LogError("파일 경로를 다시 설정해주세요.");
            PATH.Path = null;
            PATH.CurrentPath = null;
            PATH.EditorPath = null;
            PATH.ProjectPath = null;
        }

    }
}
