using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    #region 인스펙터참조멤버
    [SerializeField] private ProjectIO projectIO;
    [SerializeField] private GameObject newProjectPrefab;
    [SerializeField] private RectTransform project_rect;
    [SerializeField] private Image thumbnail_img;
    [SerializeField] private TMP_InputField projectName_inputfield;
    [SerializeField] private TMP_InputField songArtist_inputfield;
    [SerializeField] private TMP_InputField projectBpm_inputfield;
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
    [SerializeField] private Button back_btn;
    #endregion

    private Action delAction;
    private string bgmTempPath;
    private string thumbnailTempPath;
    public Project currentProject;
    public ToggleGroup projects_Group;
    public List<Project> addedProjects = new List<Project>();

    #region 프로퍼티
    public string ProjectPath { get { return projectIO.ProjectPath; } }
    public string SetProjectTMP { set { projectName_inputfield.text = value; } }
    public string SetArtistTMP { set { songArtist_inputfield.text = value; } }
    public string SetBpm { set { projectBpm_inputfield.text = value; } }
    public string SetBgmTMP { set { bgmName_tmp.text = value; } }
    public string SetThumbnailTMP { set { thumbnailName_tmp.text = value; } }
    public string SetKeySoundTMP { set { keySound_tmp.text = value; } }
    public Sprite SetThumbnail { set { thumbnail_img.sprite = value; } }
    public bool EditBtn { set { edit_btn.interactable = value; } }
    #endregion
    private void Awake()
    {
        Initialize();
        LoadProjects();
        projectName_inputfield.onValueChanged.AddListener((word) => projectName_inputfield.text = Regex.Replace(word, @"[^0-9a-zA-Z가-힣]", ""));
        songArtist_inputfield.onValueChanged.AddListener((word) => songArtist_inputfield.text = Regex.Replace(word, @"[^0-9a-zA-Z가-힣]", ""));
        projectBpm_inputfield.onValueChanged.AddListener((word) => projectBpm_inputfield.text = Regex.Replace(word, @"[^0-9]", ""));
    }

    private void OnEnable()
    {
        delAction += Delete;
    }
    private void OnDisable()
    {
        delAction -= Delete;
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
        loadKeySound_btn.onClick.AddListener(LoadKeySound);
        edit_btn.onClick.AddListener(EditProject);
        save_btn.onClick.AddListener(SaveProject);

        back_btn.onClick.AddListener(Back);
        SetDefault();
    }

    private void Back()
    {
        //TODO 경로 설정 패널로 이동~
    }

    private void EditProject()
    {
        //TODO 다음으로 넘어가기

        EditorDataManager.Instance.ProjectData = currentProject.projectData;
        EditorUIManager.Instance.pathCanvas.SetActive(false);
        EditorUIManager.Instance.editorCanvas.SetActive(true);
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
            Debug.Log(path);
            string dataPath = Path.Combine(path, "ProjectInfos");

            // json 저장 파일이 없으면 다음 디렉토리 확인
            if (!File.Exists(dataPath)) continue;

            string jsonData = File.ReadAllText(dataPath);
            ProjectData projectData = JsonUtility.FromJson<ProjectData>(jsonData);
            addProejct_btn.onClick?.Invoke();
            currentProject.projectData = projectData;
        }
        addProejct_btn.interactable = true;
        if (addedProjects.Count > 0)
            edit_btn.interactable = true;
    }

    private void LoadSong()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Sound Files", "mp3", "wav","ogg")
        };
        try
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("곡을 선택해주세요.", "", extensions, false);
            bgmName_tmp.text = Path.GetFileName(path[0]);
            bgmTempPath = path[0];
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
        try
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("썸네일을 선택해주세요.", "", extensions, false);

            thumbnailName_tmp.text = Path.GetFileName(path[0]);
            thumbnailTempPath = path[0];
            thumbnail_img.sprite = MakeSprite(path[0], Vector2.zero);
            currentProject.SetThumbnail(thumbnailName_tmp.text);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.FILELOADFAIL);
        }
    }

    private void LoadKeySound()
    {
        try
        {
            string[] path = StandaloneFileBrowser.OpenFolderPanel("키음의 디렉토리를 선택해주세요.", "", false);
            string[] files = Directory.GetFiles(path[0]);
            string extention;
            int count = 0;
            foreach (string l in files)
            {
                extention = Path.GetExtension(l);
                if (extention != ".wav" || extention != ".mp3" || extention != "ogg")
                {
                    count++;
                }
            }
            if (count > 0)
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
        if (projectName_inputfield.text == "")
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEPROJECTNAME);
            Debug.LogWarning("곡 이름을 기입해주세요.");
            return;
        }
        if (songArtist_inputfield.text == "")
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEARTIST);
            return;
        }
        if (projectBpm_inputfield.text == "")
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEBPM);
            return;
        }
        if (bgmName_tmp.text == "")
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEBGM);
            return;
        }
        if (thumbnailName_tmp.text == "")
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONETHUMBNAIL);
            return;
        }
        if (keySound_tmp.text == "")
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
                    Debug.Log("디렉토리 경로 변경");
                    currentProject.projectData.m_Path = path;
                    currentProject.SetProjectData();
                    DataSave(path);
                    currentProject.ProjectName.text = currentProject.projectData.projectName;
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
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
                if (!string.IsNullOrEmpty(currentProject.projectData.bgmName))
                {
                    bgmTemp = currentProject.projectData.bgmName;
                }
                //바뀌기 전 기존 썸네일 및 음악 삭제
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
            if (p.projectData.projectName == currentProject.projectData.projectName)
            {
                return false;
            }
        }
        return true;
    }

    private void FindDifferent(string path, string thumb, string bgm)
    {
        if (thumb != null)
        {
            if (thumb != currentProject.projectData.thumbnailName)
            {
                string p = Path.Combine(path, thumb);
                File.Delete(p);
            }
        }
        if (bgm != null)
        {
            if (bgm != currentProject.projectData.bgmName)
            {
                string p = Path.Combine(path, bgm);
                File.Delete(p);
            }
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
            if (addedProjects.Count == 0) SetDefault();
            addProejct_btn.interactable = true;
            return;
        }
        string[] files = Directory.GetFiles(currentProject.projectData.m_Path);
        foreach (string file in files)
        {
            File.Delete(file);
        }
        Directory.Delete(currentProject.projectData.m_Path);
        addedProjects.Remove(currentProject);
        Destroy(currentProject.gameObject);
        thumbnail_img.sprite = null;
        projectName_inputfield.text = null;
        songArtist_inputfield.text = null;
        bgmName_tmp.text = null;
        thumbnailName_tmp.text = null;
        projectBpm_inputfield.text = null;
        if (addedProjects.Count == 0) SetDefault();
    }

    private void Refresh()
    {
        LoadProjects();
    }

    private void AddNewProject()
    {
        GameObject project = Instantiate(newProjectPrefab, project_rect, false);
        if (currentProject != null) currentProject = null;
        currentProject = project.GetComponent<Project>();
        addedProjects.Add(currentProject);

        InputFieldReset();

        projectName_inputfield.interactable = true;
        songArtist_inputfield.interactable = true;
        loadSong_btn.interactable = true;
        loadThumbnail_btn.interactable = true;
        projectBpm_inputfield.interactable = true;

        currentProject.Toggle.interactable = false;
        addProejct_btn.interactable = false;
    }
    public void InputFieldReset()
    {
        projectName_inputfield.onEndEdit.RemoveAllListeners();
        songArtist_inputfield.onEndEdit.RemoveAllListeners();
        projectBpm_inputfield.onEndEdit.RemoveAllListeners();

        projectName_inputfield.text = "";
        songArtist_inputfield.text = "";
        projectBpm_inputfield.text = "";

        projectName_inputfield.onEndEdit.AddListener(currentProject.SetName);
        songArtist_inputfield.onEndEdit.AddListener(currentProject.SetArtist);
        projectBpm_inputfield.onEndEdit.AddListener(currentProject.SetBPM);
    }
    private void DataSave(string path)
    {
        string combinePath;
        combinePath = Path.Combine(path, "ProjectInfos");
        string json = JsonUtility.ToJson(currentProject.projectData, true);
        File.WriteAllText(combinePath, json);
        try
        {
            combinePath = Path.Combine(path, currentProject.projectData.bgmName);
            File.Copy(bgmTempPath, combinePath);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            Debug.Log("BGM 변경사항 없음");
        }
        try
        {
            combinePath = Path.Combine(path, currentProject.projectData.thumbnailName);
            File.Copy(thumbnailTempPath, combinePath);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            Debug.Log("썸네일 변경사항 없음");
        }
    }

    public Sprite MakeSprite(string filePath, Vector2 pivot)
    {
        //경로가 없다면 돌아가기
        if (string.IsNullOrEmpty(filePath) == true) return null;

        try
        {
            //이미지 읽어오기
            byte[] bytes = File.ReadAllBytes(filePath);
            //텍스쳐 만들기
            Texture2D texture = new Texture2D(100, 100);
            texture.LoadImage(bytes);
            //스프라이트 만들기
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot);
            sprite.name = texture.name;
            return sprite;

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.LOADIMGFAIL);
        }
        return null;
    }

    public void SetDefault()
    {
        edit_btn.interactable = false;
        projectName_inputfield.interactable = false;
        songArtist_inputfield.interactable = false;
        loadSong_btn.interactable = false;
        loadThumbnail_btn.interactable = false;
        projectBpm_inputfield.interactable = false;
        addProejct_btn.interactable = true;
    }
}
