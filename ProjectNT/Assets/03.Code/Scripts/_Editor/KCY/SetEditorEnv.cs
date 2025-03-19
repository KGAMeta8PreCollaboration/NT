using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using Detail = Enums.Details;
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
    [SerializeField] private Button back_btn;

    private string savePath;
    private PATH PATH = new PATH();
    private string projectPath;

    private Action quitAction;

    public string ProjectPath
    {
        get { return projectPath; }
    }
    private void Awake()
    {
        openFolderBTN.onClick.AddListener(OpenExplorer);
        nextBTN.onClick.AddListener(CheckPath);
        exit_BTN.onClick.AddListener(Exit_BTN);
        back_btn.onClick.AddListener(Back);

    }
    private IEnumerator Start()
    {
        yield return null;
        LoadPath();
        inputField.text = PATH.Path;
        if (PATH.Path != null) CheckPath();

    }
    private void OnEnable()
    {
#if UNITY_EDITOR
        quitAction += () => UnityEditor.EditorApplication.isPlaying = false;
#else
        quitAction += () => Application.Quit();
#endif
    }
    private void Back()
    {
        string p = Path.Combine(Application.persistentDataPath, "EditorPath");
        if (Directory.Exists(p)) Directory.Delete(p, true);
        inputField.text = "";
        projectIO.gameObject.SetActive(false);
        defaultPath.gameObject.SetActive(true);
    }

    private void Exit_BTN()
    {
        //TODO  세이브
#if UNITY_EDITOR
        //유니티 플레이 종료
        EditorUIManager.Instance.popUp.PopUpOpen(Detail.EDITORQUIT, quitAction);
#else
        //어플리케이션 종료
        EditorUIManager.Instance.popUp.PopUpOpen(Detail.EDITORQUIT, quitAction);
#endif
    }

    private void CheckPath()
    {
        if (inputField.text == "")
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.PATHSETERROR);
            return;
        }
        if (!Directory.Exists(PATH.Path + PATH.EditorDIR_Name))
        {
            Directory.CreateDirectory(PATH.Path + PATH.EditorDIR_Name);
            PATH.EditorPath = PATH.Path + PATH.EditorDIR_Name;
            PATH.CurrentPath = PATH.EditorPath;
            SavePath();
        }
        else PATH.EditorPath = PATH.Path + PATH.EditorDIR_Name;
        if (!Directory.Exists(PATH.EditorPath + PATH.ProjectDIR_Name))
        {
            Directory.CreateDirectory(PATH.EditorPath + PATH.ProjectDIR_Name);
            PATH.ProjectPath = PATH.EditorPath + PATH.ProjectDIR_Name;
            PATH.CurrentPath = PATH.ProjectPath;
            SavePath();
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
        string p = Path.Combine(Application.persistentDataPath, "EditorPath");
        if (Directory.Exists(p)) Directory.Delete(p, true);
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
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.PATHSETERROR);
        }
    }

    private void SavePath()
    {
        string data = JsonUtility.ToJson(PATH, true);
        savePath = Path.Combine(Application.persistentDataPath, "EditorPath");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        savePath = Path.Combine(savePath, "PathSaveFile");
        File.WriteAllText(savePath, data);
    }
    private void LoadPath()
    {
        savePath = Path.Combine(Application.persistentDataPath, "EditorPath");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        savePath = Path.Combine(savePath, "PathSaveFile");
        if (!File.Exists(savePath)) return;
        string data = File.ReadAllText(savePath);
        PATH = JsonUtility.FromJson<PATH>(data);
        savePath = Path.Combine(PATH.CurrentPath, PATH.EditorDIR_Name, PATH.ProjectDIR_Name);
        if (!Directory.Exists(savePath))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.PATHSETERROR);
            PATH.Path = null;
            PATH.CurrentPath = null;
            PATH.EditorPath = null;
            PATH.ProjectPath = null;
        }

    }
}
