using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Detail = Enums.Details;
public class ProjectLoader : MonoBehaviour
{
    [SerializeField] private ProjectIO projectIO;
    [SerializeField] private GameObject newProjectPrefab;
    [SerializeField] private RectTransform project_rect;
    [SerializeField] private Button addProejct_btn;
    [SerializeField] private Button refreah_btn;
    [SerializeField] private Button delete_btn;
    [SerializeField] private Image thumbnail_img;
    [SerializeField] private TMP_InputField projectName_inputfield;
    [SerializeField] private TMP_InputField songArtist_inputfield;
    [SerializeField] private TMP_InputField project_bpm;
    [SerializeField] private TextMeshProUGUI bgmName_tmp;
    [SerializeField] private TextMeshProUGUI thumbnailName_tmp;
    [SerializeField] private Button loadSong_btn;
    [SerializeField] private Button loadThumbnail_btn;
    [SerializeField] private Button edit_btn;
    [SerializeField] private Button save_btn;
    private string bgmTempPath;
    private string thumbnailTempPath;
    public Project currentProject;
    public ToggleGroup projects_Group;
    public List<Project> addedProjects = new List<Project>();

    public string ProjectPath { get { return projectIO.ProjectPath; } }

    public string SetProjectName { set { projectName_inputfield.text = value; } }

    public string SetArtistName { set { songArtist_inputfield.text = value; } }

    public string SetBgmName { set { bgmName_tmp.text = value; } }

    public string SetThumbnailName { set { thumbnailName_tmp.text = value; } }

    public Sprite SetThumbnail { set { thumbnail_img.sprite = value; } }

    public string SetBpm { set { project_bpm.text = value; } }

    private void Awake()
    {
        Initialize();
        LoadProjects();
    }

    private void Initialize()
    {
        if (projectIO == null) projectIO = GetComponentInParent<ProjectIO>();
        newProjectPrefab = Resources.Load<GameObject>("_SongEditor/Prefabs/NewProject");
        addProejct_btn.onClick.AddListener(AddNewProject);
        refreah_btn.onClick.AddListener(Refresh);
        delete_btn.onClick.AddListener(Delete);
        edit_btn.onClick.AddListener(EditProject);
        save_btn.onClick.AddListener(SaveProject);
        loadSong_btn.onClick.AddListener(LoadSong);
        loadThumbnail_btn.onClick.AddListener(LoadThumbnail);
        SetDefault();
    }

    private void EditProject()
    {
        //TODO 다음으로 넘어가기
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
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.FILELOADFAIL);
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
        if (project_bpm.text == "")
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEBPM);
            return;
        }
        if (bgmName_tmp.text == "BGM 선택")
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEBGM);
            return;
        }
        if (thumbnailName_tmp.text == "썸네일 선택")
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONETHUMBNAIL);
            return;
        }

        //프로젝트 데이터 업데이트
        currentProject.projectData.projectName = projectName_inputfield.text;
        currentProject.projectData.artistName = songArtist_inputfield.text;
        currentProject.projectData.bpm = int.Parse(project_bpm.text);

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
        currentProject.projectData.thumbnailName = thumbnailName_tmp.text;

        currentProject.projectData.bgmName = bgmName_tmp.text;

        addProejct_btn.interactable = true;

        edit_btn.interactable = true;

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

                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }   //기존 경로와 같다면
            else if (path == currentProject.projectData.m_Path)
            {
                //바뀌기 전 기존 썸네일 및 음악 삭제
                FindDifferent(path, thumbTemp, bgmTemp);
                DataSave(path);
            }
        }
        else
        {
            //없으면 하나 만들어
            Directory.CreateDirectory(path);
            currentProject.projectData.m_Path = path;
            DataSave(path);
        }
    }

    private void FindDifferent(string path, string thumb, string bgm)
    {
        Debug.Log(thumb);
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
    private void Delete()
    {
        if (currentProject == null) return;
        if (string.IsNullOrEmpty(currentProject.projectData.m_Path))
        {
            Destroy(currentProject.gameObject);
            SetDefault();
            return;
        }
        string[] files = Directory.GetFiles(currentProject.projectData.m_Path);
        foreach (string file in files)
        {
            File.Delete(file);
        }
        Directory.Delete(currentProject.projectData.m_Path);
        Destroy(currentProject.gameObject);
        thumbnail_img.sprite = null;
        projectName_inputfield.text = null;
        songArtist_inputfield.text = null;
        bgmName_tmp.text = null;
        thumbnailName_tmp.text = null;
        SetDefault();
    }

    private void Refresh()
    {
        LoadProjects();
    }

    private void AddNewProject()
    {
        GameObject project = Instantiate(newProjectPrefab, project_rect, false);
        currentProject = project.GetComponent<Project>();
        addedProjects.Add(currentProject);

        projectName_inputfield.onEndEdit.RemoveAllListeners();
        songArtist_inputfield.onEndEdit.RemoveAllListeners();

        projectName_inputfield.onEndEdit.AddListener(currentProject.SetName);
        songArtist_inputfield.onEndEdit.AddListener(currentProject.SetArtist);

        projectName_inputfield.interactable = true;
        songArtist_inputfield.interactable = true;
        loadSong_btn.interactable = true;
        loadThumbnail_btn.interactable = true;

        addProejct_btn.interactable = false;
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
        catch
        {
            Debug.LogWarning("BGM파일이 이미 존재합니다.");
        }
        try
        {
            combinePath = Path.Combine(path, currentProject.projectData.thumbnailName);
            File.Copy(thumbnailTempPath, combinePath);
        }
        catch
        {
            Debug.LogWarning("썸네일파일이 이미 존재합니다.");
        }
    }

    public Sprite MakeSprite(string filePath, Vector2 pivot)
    {
        //경로가 없다면 돌아가기
        if (string.IsNullOrEmpty(filePath) == true) return null;

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

    public void SetDefault()
    {
        edit_btn.interactable = false;
        projectName_inputfield.interactable = false;
        songArtist_inputfield.interactable = false;
        loadSong_btn.interactable = false;
        loadThumbnail_btn.interactable = false;

        addProejct_btn.interactable = true;
    }
}
