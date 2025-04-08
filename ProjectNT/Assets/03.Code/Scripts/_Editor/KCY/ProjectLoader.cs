using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Detail = Enums.Details;
public class ProjectLoader : MonoBehaviour
{
    #region 상수정의
    private static readonly string[] SoundFileExtensions = { "mp3", "wav", "ogg" };
    private static readonly string[] VaildKeySoundExtensions = { ".wav", ".mp3", ".ogg" };
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
    [SerializeField] private TextMeshProUGUI bgmName_tmp;
    [SerializeField] private TextMeshProUGUI thumbnailName_tmp;
    [SerializeField] private TextMeshProUGUI keySound_tmp;
    [SerializeField] private Button addProejct_btn;
    [SerializeField] private Button refreah_btn;
    [SerializeField] private Button delete_btn;
    [SerializeField] private Button loadSong_btn;
    [SerializeField] private Button loadThumbnail_btn;
    [SerializeField] private Button loadKeySound_btn;
    [SerializeField] private Button edit_btn;
    [SerializeField] private Button save_btn;
    #endregion

    private Action delAction;
    public Project currentProject;
    public ToggleGroup projects_Group;
    public List<Project> addedProjects = new List<Project>();

    #region 프로퍼티
    public string ProjectPath { get { return projectIO.ProjectPath; } }
    public string SetProjectTMP { set { projectName_inputfield.text = value; } }
    public string SetArtistTMP { set { songArtist_inputfield.text = value; } }
    public string SetBpm { set { projectBpm_inputfield.text = value; } }
    public string SetBeatNum { set { projectBeatNum_inputfield.text = value; } }
    public string SetBgmTMP { set { bgmName_tmp.text = value; } }
    public string SetThumbnailTMP { set { thumbnailName_tmp.text = value; } }
    public string SetKeySoundTMP { set { keySound_tmp.text = value; } }
    public Sprite SetThumbnail { set { thumbnail_img.sprite = value; } }
    public bool EditBtn { set { edit_btn.interactable = value; } }
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
        currentProject = null;
        SetDefault(false);
        addProejct_btn.interactable = true;
        SetProjectDataNull();
    }

    private void Initialize()
    {
        if (projectIO == null) projectIO = GetComponentInParent<ProjectIO>();
        newProjectPrefab = Resources.Load<GameObject>("_SongEditor/Prefabs/NewProject");
        addProejct_btn.onClick.AddListener(AddNewProject);
        refreah_btn.onClick.AddListener(Refresh);
        delete_btn.onClick.AddListener(DeleteUIOpen);
        loadSong_btn.onClick.AddListener(LoadSong);
        loadThumbnail_btn.onClick.AddListener(LoadThumbnail);
        loadKeySound_btn.onClick.AddListener(KeySoundPathSet);
        edit_btn.onClick.AddListener(EditProject);
        save_btn.onClick.AddListener(SaveProject);

        SetDefault(false);
        addProejct_btn.interactable = true;

    }

    private void EditProject()
    {
        EditorDataManager.Instance.thumbnail_sprite = thumbnail_img.sprite;
        EditorDataManager.Instance.ProjectData = currentProject.projectData;
        EditorDataManager.Instance.SetBgm();
        EditorLoadScene.SceneLoad("SongEditorScene");
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
            currentProject.projectData = projectData;
        }
        addProejct_btn.interactable = true;
        if (addedProjects.Count > 0)
            edit_btn.interactable = true;
    }
    private void AddNewProject()
    {
        GameObject project = Instantiate(newProjectPrefab, project_rect, false);
        if (currentProject != null)
        {
            currentProject = null;
        }
        currentProject = project.GetComponent<Project>();
        addedProjects.Add(currentProject);

        InputFieldReset();

        SetDefault(true);
        edit_btn.interactable = false;

        currentProject.Toggle.interactable = false;
        addProejct_btn.interactable = false;
    }

    private void LoadSong()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Sound Files", SoundFileExtensions)
        };
        try
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("곡을 선택해주세요.", "", extensions, false);
            bgmName_tmp.text = Path.GetFileName(path[0]);
            currentProject.projectData.bgmPath = path[0];
            currentProject.SetBgm(bgmName_tmp.text);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.FILELOADFAIL);
        }
    }

    private void LoadThumbnail()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Image Files", "jpeg","png","jpg")
        };

        string[] path = StandaloneFileBrowser.OpenFilePanel("썸네일을 선택해주세요.", "", extensions, false);

        thumbnailName_tmp.text = Path.GetFileName(path[0]);
        thumbnail_img.sprite =
        ByteToSprite(null, path[0]);
        currentProject.SetThumbnail(thumbnailName_tmp.text);
    }

    private void KeySoundPathSet()
    {
        try
        {
            string[] path = StandaloneFileBrowser.OpenFolderPanel("키음의 디렉토리를 선택해주세요.", "", false);
            string[] files = Directory.GetFiles(path[0]);
            string extention;
            int count = 0;
            foreach (string file in files)
            {
                extention = Path.GetExtension(file);

                if (false == VaildKeySoundExtensions.Contains(extention))
                {
                    count++;
                }
            }
            if (0 < count)
            {
                EditorUIManager.Instance.popUp.PopUpOpen(Detail.FILEDETECTIONFAIL);
                return;
            }
            string keysoundPath = Path.GetFullPath(path[0]);
            keySound_tmp.text = keysoundPath;
            currentProject.SetKeySoundPath(keysoundPath);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.PATHSETERROR);
        }
    }

    private void SaveProject()
    {
        if (string.IsNullOrEmpty(projectName_inputfield.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEPROJECTNAME);
            return;
        }
        if (string.IsNullOrEmpty(songArtist_inputfield.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEARTIST);
            return;
        }
        if (string.IsNullOrEmpty(projectBpm_inputfield.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEBPM);
            return;
        }
        if (string.IsNullOrEmpty(projectBeatNum_inputfield.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEBPM);
            return;
        }
        if (string.IsNullOrEmpty(bgmName_tmp.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEBGM);
            return;
        }
        if (string.IsNullOrEmpty(thumbnailName_tmp.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONETHUMBNAIL);
            return;
        }
        if (string.IsNullOrEmpty(keySound_tmp.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEKEYSOUNDFOLDER);
            return;
        }

        string path = Path.Combine(projectIO.ProjectPath, projectName_inputfield.text);

        //기존 저장 경로가 있을 시
        if (Directory.Exists(currentProject.projectData.m_Path))
        {
            //기존 경로와 다르다면
            if (currentProject.projectData.m_Path != path)
            {
                try
                {
                    //디렉토리 이름 변경 시도
                    Directory.Move(currentProject.projectData.m_Path, path);
                    currentProject.projectData.m_Path = path;
                    currentProject.SetProjectData();
                    DataSave(path);
                    currentProject.ProjectName.text = currentProject.projectData.projectName;
                }
                catch
                {
                    EditorUIManager.Instance.popUp.PopUpOpen(Detail.SAVEFOLDEREXIST);
                }
            }   //기존 경로와 같다면
            else if (path == currentProject.projectData.m_Path)
            {
                string thumbTemp = null;
                string bgmTemp = null;

                if (!string.IsNullOrEmpty(currentProject.projectData.thumbnailName))
                {
                    thumbTemp = currentProject.projectData.thumbnailName;
                }
                if (!string.IsNullOrEmpty(currentProject.projectData.bgmPath))
                {
                    bgmTemp = currentProject.projectData.bgmPath;
                }
                //바뀌기 전 기존 썸네일 및 음악 삭제s
                FindDifferent(path, thumbTemp, bgmTemp);
                currentProject.SetProjectData();
                DataSave(path);
                currentProject.ProjectName.text = currentProject.projectData.projectName;
                EditorUIManager.Instance.popUp.PopUpOpen(Detail.CHANGEPROJECTINFOCOMPLETE);
            }
        }
        else
        {
            bool check = FindSameProjects();
            if (!check)
            {
                EditorUIManager.Instance.popUp.PopUpOpen(Detail.SAVEFOLDEREXIST);
                return;
            }
            //없으면 하나 만들어
            Directory.CreateDirectory(path);
            currentProject.projectData.m_Path = path;
            currentProject.SetProjectData();
            DataSave(path);
            currentProject.ProjectName.text = currentProject.projectData.projectName;
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.MAKEPROJECTCOMPLETE);

        }
        addProejct_btn.interactable = true;
        edit_btn.interactable = true;
    }

    private bool FindSameProjects()
    {
        foreach (Project p in addedProjects)
        {
            if (p == currentProject) continue;
            if (p.projectData.projectName == currentProject.TempName)
            {
                return false;
            }
        }
        return true;
    }

    private void FindDifferent(string path, string thumb, string bgm)
    {
        DeleteFileIfDifferent(path, thumb, currentProject.projectData.thumbnailName);
        DeleteFileIfDifferent(path, bgm, currentProject.projectData.bgmPath);
    }
    private void DeleteFileIfDifferent(string path, string oldFile, string newFile)
    {
        if (null != oldFile && oldFile != newFile)
        {
            string fullPath = Path.Combine(path, oldFile);
            File.Delete(fullPath);
        }
    }

    private void DeleteUIOpen()
    {
        EditorUIManager.Instance.popUp.PopUpOpen(Detail.DELETEPROJECTCHECK, delAction);
    }

    private void Delete()
    {
        if (currentProject == null) return;
        if (string.IsNullOrEmpty(currentProject.projectData.m_Path))
        {
            addedProjects.Remove(currentProject);
            Destroy(currentProject.gameObject);
            if (addedProjects.Count == 0)
            {
                SetDefault(false);
            }
            addProejct_btn.interactable = true;
            return;
        }
        string[] files = Directory.GetFiles(currentProject.projectData.m_Path);
        foreach (string file in files)
        {
            File.Delete(file);
        }
        Directory.Delete(currentProject.projectData.m_Path, true);
        addedProjects.Remove(currentProject);
        Destroy(currentProject.gameObject);

        SetProjectDataNull();

        if (addedProjects.Count == 0)
        {
            SetDefault(false);
            addProejct_btn.interactable = true;
        }
    }

    private void Refresh()
    {
        LoadProjects();
    }

    private void DataSave(string path)
    {
        string combinePath;
        combinePath = Path.Combine(path, "ProjectInfos");
        string json = JsonUtility.ToJson(currentProject.projectData, true);
        File.WriteAllText(combinePath, json);
    }

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
                EditorUIManager.Instance.popUp.PopUpOpen(Detail.LOADIMGFAIL);
                return null;
            }
        }

        if (bytes == null)
        {
            bytes = currentProject.projectData.thumbnailData;
            currentProject.SetThumbnailData(bytes);
        }
        else
        {
            currentProject.SetThumbnailData(bytes);
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

        projectName_inputfield.onEndEdit.AddListener(currentProject.SetName);
        songArtist_inputfield.onEndEdit.AddListener(currentProject.SetArtist);
        projectBpm_inputfield.onEndEdit.AddListener(currentProject.SetBPM);
        projectBeatNum_inputfield.onEndEdit.AddListener(currentProject.SetBeatNum);
    }
    public void SetDefault(bool isTrue)
    {
        var interactableElements = new Selectable[]
        {
        edit_btn, projectName_inputfield, songArtist_inputfield,
        loadSong_btn, loadThumbnail_btn, projectBpm_inputfield,
        projectBeatNum_inputfield, loadKeySound_btn
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
        bgmName_tmp.text = null;
        thumbnailName_tmp.text = null;
        keySound_tmp.text = null;
    }
}
