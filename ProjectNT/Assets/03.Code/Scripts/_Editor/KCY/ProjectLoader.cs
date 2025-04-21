using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SFB;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Detail = Enums.Details;
public class ProjectLoader : MonoBehaviour
{
    #region 상수정의

    private const string AlphanumericRegex = @"[^0-9a-zA-Z가-힣]";
    private const string NumericRegex = @"[^0-9]";
    private const int DefaultTextureHeight = 100;
    private const int DefaultTextureWidth = 100;
    #endregion
    #region 인스펙터참조멤버
    [SerializeField] private ProjectIO projectIO;
    [SerializeField] private GameObject newProjectPrefab;
    [SerializeField] private RectTransform project_rect;
    [SerializeField] private Image thumbnail_img;
    [SerializeField] private TMP_InputField projectName_inputfield;
    [SerializeField] private TMP_InputField songArtist_inputfield;
    [SerializeField] private TMP_InputField projectBpm_inputfield;
    [SerializeField] private TMP_InputField projectBeatNum_inputfield;
    [SerializeField] private TextMeshProUGUI thumbnailName_tmp;
    [SerializeField] private TextMeshProUGUI refreshTime_tmp;
    [SerializeField] private Button addProejct_btn;
    [SerializeField] private Button refreah_btn;
    [SerializeField] private Button loadThumbnail_btn;
    [SerializeField] private Button select_btn;
    [SerializeField] private Button save_btn;
    #endregion
    private Action delAction;
    public Project currProject;
    public ToggleGroup projects_Group;
    public List<Project> addedProjects = new List<Project>();

    #region 프로퍼티
    public string ProjectPath { get { return projectIO.ProjectPath; } }
    public string SetProjectTMP { set { projectName_inputfield.text = value; } }
    public string SetArtistTMP { set { songArtist_inputfield.text = value; } }
    public string SetBpm { set { projectBpm_inputfield.text = value; } }
    public string SetBeatNum { set { projectBeatNum_inputfield.text = value; } }
    public string SetThumbnailTMP { set { thumbnailName_tmp.text = value; } }
    public Sprite SetThumbnail { set { thumbnail_img.sprite = value; } }
    public bool EditBtn { set { select_btn.interactable = value; } }
    #endregion

    private void Awake()
    {
        Initialize();
        projectName_inputfield.onValueChanged.AddListener((word) => projectName_inputfield.text = Regex.Replace(word, AlphanumericRegex, ""));
        songArtist_inputfield.onValueChanged.AddListener((word) => songArtist_inputfield.text = Regex.Replace(word, AlphanumericRegex, ""));
        projectBpm_inputfield.onValueChanged.AddListener((word) => projectBpm_inputfield.text = Regex.Replace(word, NumericRegex, ""));
        projectBeatNum_inputfield.onValueChanged.AddListener((word) => projectBeatNum_inputfield.text = Regex.Replace(word, NumericRegex, ""));
    }

    private void OnEnable()
    {
        delAction += Delete;
        LoadProjects();
    }
    private void OnDisable()
    {
        delAction -= Delete;
        currProject = null;
        SetDefault(false);
        addProejct_btn.interactable = true;
        SetProjectDataNull();
    }

    private void Initialize()
    {
        if (projectIO == null) projectIO = GetComponentInParent<ProjectIO>();
        addProejct_btn.onClick.AddListener(AddNewProject);
        refreah_btn.onClick.AddListener(LoadProjects);
        loadThumbnail_btn.onClick.AddListener(LoadThumbnail);
        select_btn.onClick.AddListener(projectIO.EditProjectOpen);
        save_btn.onClick.AddListener(SaveProject);

        SetDefault(false);
        addProejct_btn.interactable = true;
    }

    private void LoadProjects()
    {
        if (addedProjects.Count > 0)
        {
            foreach (Project p in addedProjects)
            {
                Destroy(p.gameObject);
            }
            addedProjects.Clear();
        }
        string[] paths = Directory.GetDirectories(ProjectPath);
        foreach (string path in paths)
        {
            string dataPath = Path.Combine(path, "ProjectInfos");

            // json 저장 파일이 없으면 다음 디렉토리 확인
            if (!File.Exists(dataPath))
            {
                continue;
            }

            string jsonData = File.ReadAllText(dataPath);
            ProjectData projectData = JsonUtility.FromJson<ProjectData>
            (jsonData);
            addProejct_btn.onClick?.Invoke();
            currProject.projectData = projectData;
        }

        addProejct_btn.interactable = true;

        if (addedProjects.Count > 0)
        {
            select_btn.interactable = true;
        }

        refreshTime_tmp.text = "Last Refreshed " + DateTime.Now.ToString("HH:mm:ss");
    }
    private void AddNewProject()
    {
        GameObject project = Instantiate(newProjectPrefab, project_rect, false);
        if (currProject != null)
        {
            currProject = null;
        }
        currProject = project.GetComponent<Project>();
        addedProjects.Add(currProject);

        InputFieldReset();

        SetDefault(true);
        select_btn.interactable = false;

        currProject.Toggle.interactable = false;
        addProejct_btn.interactable = false;
    }

    private void LoadThumbnail()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Image Files", "jpeg","png","jpg")
        };

        string[] path = StandaloneFileBrowser.OpenFilePanel("썸네일을 선택해주세요.", "", extensions, false);

        thumbnailName_tmp.text = Path.GetFileName(path[0]);
        thumbnail_img.sprite = ByteToSprite(null, path[0]);
        currProject.SetThumbnail(thumbnailName_tmp.text);
    }

    private void SaveProject()
    {
        if (string.IsNullOrEmpty(projectName_inputfield.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NoneProjectName);
            return;
        }
        if (string.IsNullOrEmpty(songArtist_inputfield.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NoneArtist);
            return;
        }
        if (string.IsNullOrEmpty(projectBpm_inputfield.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NoneBpm);
            return;
        }
        if (string.IsNullOrEmpty(projectBeatNum_inputfield.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NoneBpm);
            return;
        }
        if (string.IsNullOrEmpty(thumbnailName_tmp.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NoneThumbnail);
            return;
        }

        string path = Path.Combine(projectIO.ProjectPath, projectName_inputfield.text);

        //기존 저장 경로가 있을 시
        if (Directory.Exists(currProject.projectData.m_Path))
        {
            //기존 경로와 다르다면
            if (currProject.projectData.m_Path != path)
            {
                try
                {
                    //디렉토리 이름 변경 시도
                    Directory.Move(currProject.projectData.m_Path, path);
                    currProject.projectData.m_Path = path;
                    currProject.SetProjectData();
                    EditorDataManager.Instance.ProjectInfoSave(currProject.projectData);
                    currProject.ProjectName.text = currProject.projectData.projectName;
                }
                catch
                {
                    EditorUIManager.Instance.popUp.PopUpOpen(Detail.SaveFolderExist);
                }
            }   //기존 경로와 같다면
            else if (path == currProject.projectData.m_Path)
            {
                string thumbTemp = null;
                string bgmTemp = null;

                if (!string.IsNullOrEmpty(currProject.projectData.thumbnailName))
                {
                    thumbTemp = currProject.projectData.thumbnailName;
                }
                if (!string.IsNullOrEmpty(currProject.projectData.bgmPath))
                {
                    bgmTemp = currProject.projectData.bgmPath;
                }
                //바뀌기 전 기존 썸네일 및 음악 삭제s
                FindDifferent(path, thumbTemp, bgmTemp);
                currProject.SetProjectData();
                EditorDataManager.Instance.ProjectInfoSave(currProject.projectData);
                currProject.ProjectName.text = currProject.projectData.projectName;
                EditorUIManager.Instance.popUp.PopUpOpen(Detail.ChangeProjectInfoComplete);
            }
        }
        // 기존 저장 경로가 없을 시
        else
        {
            bool check = FindSameProjects();
            if (!check)
            {
                EditorUIManager.Instance.popUp.PopUpOpen(Detail.SaveFolderExist);
                return;
            }
            Directory.CreateDirectory(path);
            currProject.projectData.m_Path = path;
            currProject.SetProjectData();
            EditorDataManager.Instance.ProjectInfoSave(currProject.projectData);
            currProject.ProjectName.text = currProject.projectData.projectName;
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.MakeProjectComplete);

        }
        addProejct_btn.interactable = true;
        select_btn.interactable = true;
    }

    private bool FindSameProjects()
    {
        foreach (Project p in addedProjects)
        {
            if (p == currProject) continue;
            if (p.projectData.projectName == currProject.TempName)
            {
                return false;
            }
        }
        return true;
    }

    private void FindDifferent(string path, string thumb, string bgm)
    {
        DeleteFileIfDifferent(path, thumb, currProject.projectData.thumbnailName);
        DeleteFileIfDifferent(path, bgm, currProject.projectData.bgmPath);
    }
    private void DeleteFileIfDifferent(string path, string oldFile, string newFile)
    {
        if (null != oldFile && oldFile != newFile)
        {
            string fullPath = Path.Combine(path, oldFile);
            File.Delete(fullPath);
        }
    }

    public void DeleteUIOpen()
    {
        EditorUIManager.Instance.popUp.PopUpOpen(Detail.DeleteProjectCheck, delAction);
    }

    private void Delete()
    {
        if (currProject == null) return;
        if (string.IsNullOrEmpty(currProject.projectData.m_Path))
        {
            addedProjects.Remove(currProject);
            Destroy(currProject.gameObject);
            if (addedProjects.Count == 0)
            {
                SetDefault(false);
            }
            addProejct_btn.interactable = true;
            return;
        }
        string[] files = Directory.GetFiles(currProject.projectData.m_Path);
        foreach (string file in files)
        {
            File.Delete(file);
        }
        Directory.Delete(currProject.projectData.m_Path, true);
        addedProjects.Remove(currProject);
        Destroy(currProject.gameObject);

        SetProjectDataNull();

        if (addedProjects.Count == 0)
        {
            SetDefault(false);
            addProejct_btn.interactable = true;
        }
    }

    // private void DataSave(string path)
    // {
    //     string combinePath;
    //     combinePath = Path.Combine(path, "ProjectInfos");
    //     string json = JsonUtility.ToJson(currProject.projectData, true);
    //     File.WriteAllText(combinePath, json);
    // }

    public Sprite ByteToSprite(byte[] bytes = null, string filePath = null)
    {
        if (false == string.IsNullOrEmpty(filePath))
        {
            try
            {
                bytes = File.ReadAllBytes(filePath);
            }
            catch
            {
                EditorUIManager.Instance.popUp.PopUpOpen(Detail.LoadImageFail);
                return null;
            }
        }

        if (bytes == null)
        {
            bytes = currProject.projectData.thumbnailData;
            currProject.SetThumbnailData(bytes);
        }
        else
        {
            currProject.SetThumbnailData(bytes);
        }
        Texture2D texture = new Texture2D(DefaultTextureWidth, DefaultTextureHeight);
        texture.LoadImage(bytes);
        //스프라이트 만들기
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        sprite.name = texture.name;
        return sprite;
    }
    public void InputFieldReset()
    {
        projectName_inputfield.onEndEdit.RemoveAllListeners();
        songArtist_inputfield.onEndEdit.RemoveAllListeners();
        projectBpm_inputfield.onEndEdit.RemoveAllListeners();
        projectBeatNum_inputfield.onEndEdit.RemoveAllListeners();

        projectName_inputfield.text = string.Empty;
        songArtist_inputfield.text = string.Empty;
        projectBpm_inputfield.text = string.Empty;
        projectBeatNum_inputfield.text = string.Empty;

        projectName_inputfield.onEndEdit.AddListener(currProject.SetName);
        songArtist_inputfield.onEndEdit.AddListener(currProject.SetArtist);
        projectBpm_inputfield.onEndEdit.AddListener(currProject.SetBPM);
        projectBeatNum_inputfield.onEndEdit.AddListener(currProject.SetBeatNum);
    }
    public void SetDefault(bool isTrue)
    {
        var interactableElements = new Selectable[]
        {
        select_btn, projectName_inputfield, songArtist_inputfield,
        loadThumbnail_btn, projectBpm_inputfield,
        projectBeatNum_inputfield,
        };
        foreach (var element in interactableElements)
        {
            element.interactable = isTrue;
        }
    }
    private void SetProjectDataNull()
    {
        thumbnail_img.sprite = null;
        projectName_inputfield.text = null;
        songArtist_inputfield.text = null;
        projectBpm_inputfield.text = null;
        projectBeatNum_inputfield.text = null;
        thumbnailName_tmp.text = null;
    }
}
