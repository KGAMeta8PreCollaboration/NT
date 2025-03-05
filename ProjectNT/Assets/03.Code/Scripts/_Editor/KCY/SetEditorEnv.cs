using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
using TMPro;
using UnityEngine;
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
    private string savePath = "Assets/Resources/";
    private PATH PATH = new PATH();
    public PATH Path
    {
        get { return PATH; }
    }
    private void Awake()
    {
        LoadPath();
        inputField.text = PATH.Path;
        openFolderBTN.onClick.AddListener(OpenExplorer);
        nextBTN.onClick.AddListener(CheckPath);
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
            if (path[0] == PATH.EditorPath)
            {

            }
            if (path[0] != PATH.EditorPath)
            {

            }
            PATH.CurrentPath = PATH.Path;
            inputField.text = PATH.CurrentPath;
            SavePath();
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

    }
}
