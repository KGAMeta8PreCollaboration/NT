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
public class EditorPATH
{
    public string defaultPath;
    public string topLevelPath;
    public string projectPath;
    public string topLevelDir_Name = "Night Traveler_Editor";
    public string projectDir_Name = "Projects";
}

public class SetEditorEnv : MonoBehaviour
{
    [SerializeField] private RectTransform defaultPath;
    [SerializeField] private RectTransform project;
    [SerializeField] private TextMeshProUGUI path_tmp;
    [SerializeField] private TextMeshProUGUI path_placeholder;
    [SerializeField] private Button openFolderBTN;
    [SerializeField] private Button nextBTN;
    [SerializeField] private ProjectIO projectIO;
    [SerializeField] private Button exit_BTN;
    [SerializeField] private Button back_btn;

    private string savePath;
    private EditorPATH editorPath = new EditorPATH();
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
        path_tmp.text = editorPath.defaultPath;
        if (editorPath.defaultPath != null) CheckPath();

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
        if (false == Directory.Exists(editorPath.projectPath))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.PATHSETERROR);
            return;
        }
        projectPath = editorPath.projectPath;
        defaultPath.gameObject.SetActive(false);
        projectIO.gameObject.SetActive(true);
    }

    private void OpenExplorer()
    {
        string p;
        // 기존에 저장된 경로 폴더 삭제
        p = Path.Combine(Application.persistentDataPath, "EditorPath");
        if (true == Directory.Exists(p))
        {
            Directory.Delete(p, true);
        }

        var path = StandaloneFileBrowser.OpenFolderPanel("에디터 경로 선택", "", false);
        try
        {
            string[] dirs = Directory.GetDirectories(path[0]);

            // 이미 에디터 폴더가 있는 경우
            foreach (string dir in dirs)
            {
                if (true == string.IsNullOrEmpty(dir))
                {
                    Debug.LogError("!");
                    continue;
                }
                // 최상위 폴더가 선택한 경로에 있을 경우
                p = Path.Combine(path[0], editorPath.topLevelDir_Name);
                if (dir == p)
                {
                    // 현재 경로를 기본 경로로 설정
                    editorPath.defaultPath = path[0];
                    editorPath.topLevelPath = p;

                    // 프로젝트 폴더 생성 및 경로 지정
                    p = Path.Combine(p, editorPath.projectDir_Name);
                    if (false == Directory.Exists(p)) { Directory.CreateDirectory(p); }
                    editorPath.projectPath = p;
                    SavePath();
                    return;
                }

                // 프로젝트 폴더가 선택한 경로에 있을 경우
                p = Path.Combine(path[0], editorPath.projectDir_Name);
                if (dir == p)
                {
                    // 최상위 폴더의 부모 디렉토리를 가져옴
                    DirectoryInfo dirInfo = Directory.GetParent(p);

                    // 부모 디렉토리의 이름이 최상위 폴더와 같으면
                    if (dirInfo.Name == editorPath.topLevelDir_Name)
                    {
                        // 기본 폴더경로 설정
                        editorPath.defaultPath = Directory.GetParent(dirInfo.FullName).FullName;
                        // 최상위 폴더 경로 설정
                        editorPath.topLevelPath = path[0];

                        // 프로젝트 폴더 생성 및 경로 지정
                        p = Path.Combine(path[0], editorPath.projectDir_Name);
                        if (false == Directory.Exists(p)) { Directory.CreateDirectory(p); }
                        editorPath.projectPath = p;
                        SavePath();
                        return;
                    }
                }
            }

            // 기존 폴더들이 없는 경우 새로 생성
            editorPath.defaultPath = path[0];
            p = Path.Combine(path[0], editorPath.topLevelDir_Name);
            Directory.CreateDirectory(p);
            editorPath.topLevelPath = p;
            p = Path.Combine(p, editorPath.projectDir_Name);
            Directory.CreateDirectory(p);
            editorPath.projectPath = p;
            SavePath();
            return;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.PATHSETERROR);
        }
    }

    private void SavePath()
    {
        string data = JsonUtility.ToJson(editorPath, true);
        savePath = Path.Combine(Application.persistentDataPath, "EditorPath");
        if (false == Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        savePath = Path.Combine(savePath, "PathSaveFile");
        File.WriteAllText(savePath, data);
        path_tmp.text = editorPath.defaultPath;
        path_placeholder.gameObject.SetActive(false);
    }
    private void LoadPath()
    {
        savePath = Path.Combine(Application.persistentDataPath, "EditorPath");
        if (false == Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        savePath = Path.Combine(savePath, "PathSaveFile");
        if (false == File.Exists(savePath)) { return; }
        string data = File.ReadAllText(savePath);
        editorPath = JsonUtility.FromJson<EditorPATH>(data);
        savePath = Path.Combine(editorPath.defaultPath, editorPath.topLevelDir_Name, editorPath.projectDir_Name);
        if (false == Directory.Exists(savePath))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.PATHSETERROR);
            editorPath.defaultPath = null;
            editorPath.topLevelPath = null;
            editorPath.projectPath = null;
        }
    }
}
