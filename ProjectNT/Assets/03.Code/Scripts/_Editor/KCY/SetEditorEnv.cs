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
    public string currentPath;
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
        // 새로운 에디터 경로 저장을 위해 기존 저장된 경로데이터 삭제
        string p = Path.Combine(Application.persistentDataPath, "EditorPath");
        if (Directory.Exists(p))
        {
            Directory.Delete(p, true);
        }
        path_tmp.text = "";
        path_placeholder.gameObject.SetActive(true);
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
        // 경로 설정 오류
        if (path_tmp.text == "")
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.PATHSETERROR);
            return;
        }
        // 기존 기본경로에 에디터 폴더가 없으면
        if (false == Directory.Exists(Path.Combine(editorPath.defaultPath, editorPath.topLevelDir_Name)))
        {
            // 에디터 폴더 생성
            Directory.CreateDirectory(Path.Combine(editorPath.defaultPath, editorPath.topLevelDir_Name));
            // 에디터 경로 저장
            editorPath.topLevelPath = Path.Combine(editorPath.defaultPath, editorPath.topLevelDir_Name);
            editorPath.currentPath = editorPath.topLevelPath;
            SavePath();
        }
        else editorPath.topLevelPath = Path.Combine(editorPath.defaultPath, editorPath.topLevelDir_Name);
        if (!Directory.Exists(Path.Combine(editorPath.topLevelPath, editorPath.projectDir_Name)))
        {
            Directory.CreateDirectory(Path.Combine(editorPath.topLevelPath, editorPath.projectDir_Name));
            editorPath.projectPath = Path.Combine(editorPath.topLevelPath, editorPath.projectDir_Name);
            editorPath.currentPath = editorPath.projectPath;
            SavePath();
        }
        else editorPath.projectPath = Path.Combine(editorPath.topLevelPath, editorPath.projectDir_Name);
        if (Directory.Exists(editorPath.projectPath))
        {
            projectPath = editorPath.projectPath;
            defaultPath.gameObject.SetActive(false);
            projectIO.gameObject.SetActive(true);

        }
        else
        {
            Debug.LogWarning($"프로젝트 폴더 경로 오류\n{editorPath.projectPath}");
        }

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
            #region X
            // // 사용자가 선택한 경로가 비어있을 경우
            // if (string.IsNullOrEmpty(path[0]))
            // {
            //     EditorUIManager.Instance.popUp.PopUpOpen(Detail.PATHSETERROR);
            //     return;
            // }
            // // 선택한 경로가 기존 기본 폴더 경로와 같은 경우
            // if (path[0] == editorPath.defaultPath)
            // {
            //     // 최상위 폴더가 존재하는지 확인
            //     p = Path.Combine(path[0], editorPath.topLevelDir_Name);
            //     if (true == Directory.Exists(p))
            //     {
            //         // 있다면 기본 경로 설정 후 return
            //         path_placeholder.gameObject.SetActive(false);
            //         path_tmp.text = editorPath.defaultPath;
            //         print("최상위 폴더 확인");
            //         return;
            //     }
            //     else
            //     {
            //         // 없으면 최상위/프로젝트폴더 경로저장 및 생성 후 return
            //         editorPath.topLevelPath = p;
            //         p = Path.Combine(p, editorPath.projectDir_Name);
            //         Directory.CreateDirectory(p);
            //         editorPath.projectPath = p;
            //         SavePath();
            //         return;
            //     }
            // }
            // // 선택한 경로가 기존 최상위 폴더 경로와 같은 경우
            // if (path[0] == editorPath.topLevelPath)
            // {
            //     // 최상위/프로젝트 폴더 있는지 확인
            //     p = Path.Combine(path[0], editorPath.projectDir_Name);
            //     if (true == Directory.Exists(p))
            //     {
            //         // 있으면 기존 기본폴더 경로 설정 후 return
            //         path_placeholder.gameObject.SetActive(false);
            //         path_tmp.text = editorPath.defaultPath;
            //         return;
            //     }
            //     else
            //     {
            //         // 없으면 프로젝트 폴더 생성 후 기존 기본폴더 경로 설정 후 return
            //         Directory.CreateDirectory(p);
            //         editorPath.projectPath = p;
            //         SavePath();
            //         return;
            //     }
            // }
            // // 선택한 경로가 기존 프로젝트 폴더 경로와 같은 경우
            // if (path[0] == editorPath.projectPath)
            // {

            // }
            #endregion

            p = Path.Combine(path[0], editorPath.topLevelPath);
            if (true == Directory.Exists(p))
            {

            }
            //에디터 폴더를 직접 선택한 경우
            if (true == Directory.Exists(Path.Combine(path[0], editorPath.projectDir_Name)))
            {
                editorPath.topLevelPath = path[0];
                //에디터 폴더에 프로젝트 폴더가 존재하는지 확인
                if (Directory.Exists(Path.Combine(editorPath.topLevelPath, editorPath.projectDir_Name)))
                {
                    //프로젝트 폴더 경로 재설정
                    editorPath.projectPath = Path.Combine(editorPath.topLevelPath, editorPath.projectDir_Name);
                }
                editorPath.defaultPath = path[0].Replace(editorPath.topLevelDir_Name, "");
                path_tmp.text = editorPath.defaultPath;
                path_placeholder.gameObject.SetActive(false);
                SavePath();
            }
            else
            {
                editorPath.defaultPath = path[0];
                editorPath.currentPath = editorPath.defaultPath;
                path_tmp.text = editorPath.currentPath;
                path_placeholder.gameObject.SetActive(false);
                SavePath();
            }
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
        editorPath = JsonUtility.FromJson<EditorPATH>(data);
        savePath = Path.Combine(editorPath.currentPath, editorPath.topLevelDir_Name, editorPath.projectDir_Name);
        if (!Directory.Exists(savePath))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.PATHSETERROR);
            editorPath.defaultPath = null;
            editorPath.currentPath = null;
            editorPath.topLevelPath = null;
            editorPath.projectPath = null;
        }
    }
}
