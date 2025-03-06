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

public class ProjectLoader : MonoBehaviour
{
    [SerializeField] private ProjectIO projectIO;
    [SerializeField] private GameObject newProjectPrefab;
    [SerializeField] private RectTransform project_Rect;
    [SerializeField] private Button addProejct_BTN;
    [SerializeField] private Button refreah_BTN;
    [SerializeField] private Button delete_BTN;
    [SerializeField] private Image thumbnail_IMG;
    [SerializeField] private TMP_InputField project_Name;
    [SerializeField] private TMP_InputField song_Artist;
    [SerializeField] private TMP_InputField project_BPM;
    [SerializeField] private TextMeshProUGUI bgmName_TMP;
    [SerializeField] private TextMeshProUGUI thumbnailName_TMP;
    [SerializeField] private Button loadSong_BTN;
    [SerializeField] private Button loadThumbnail_BTN;
    [SerializeField] private Button edit_BTN;
    [SerializeField] private Button save_BTN;
    private string bgmTempPath;
    private string thumbnailTempPath;
    public Project currentProject;
    public ToggleGroup projects_Group;
    public List<Project> addedProjects = new List<Project>();
    public string ProjectPath
    {
        get { return projectIO.ProjectPath; }
    }
    public string SetProjectName
    {
        set { project_Name.text = value; }
    }
    public string SetArtistName
    {
        set { song_Artist.text = value; }
    }
    public string SetBgmName
    {
        set { bgmName_TMP.text = value; }
    }
    public string SetThumbnailName
    {
        set { thumbnailName_TMP.text = value; }
    }
    public Sprite SetThumbnail
    {
        set { thumbnail_IMG.sprite = value; }
    }
    private void Awake()
    {
        Initialize();
        LoadProjects();
    }

    private void Initialize()
    {

        if (projectIO == null) projectIO = GetComponentInParent<ProjectIO>();
        newProjectPrefab = Resources.Load<GameObject>("_SongEditor/Prefabs/NewProject");
        addProejct_BTN.onClick.AddListener(AddNewProject);
        refreah_BTN.onClick.AddListener(Refresh);
        delete_BTN.onClick.AddListener(Delete);
        edit_BTN.onClick.AddListener(EditProject);
        save_BTN.onClick.AddListener(SaveProject);
        loadSong_BTN.onClick.AddListener(LoadSong);
        loadThumbnail_BTN.onClick.AddListener(LoadThumbnail);
        SetDefault();
    }

    private void EditProject()
    {
        //TODO 다음으로 넘어가기
    }

    private void LoadProjects()
    {
        string[] paths = Directory.GetDirectories(ProjectPath);
        foreach (string path in paths)
        {
            Debug.Log(path);
            string dataPath = Path.Combine(path, "ProjectInfos");

            // json 저장 파일이 없으면 다음 디렉토리 확인
            if (!File.Exists(dataPath)) continue;

            string jsonData = File.ReadAllText(dataPath);
            ProjectData projectData = JsonUtility.FromJson<ProjectData>(jsonData);
            addProejct_BTN.onClick?.Invoke();
            currentProject.projectData = projectData;
        }
        addProejct_BTN.interactable = true;
        edit_BTN.interactable = true;
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
            bgmName_TMP.text = Path.GetFileName(path[0]);
            bgmTempPath = path[0];
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            Debug.LogWarning("파일을 다시 선택해주세요.");
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

            thumbnailName_TMP.text = Path.GetFileName(path[0]);
            thumbnailTempPath = path[0];
            thumbnail_IMG.sprite = MakeSprite(path[0], Vector2.zero);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            Debug.LogWarning("파일을 다시 선택해주세요.");
        }
    }

    private void SaveProject()
    {
        if (project_Name.text == "")
        {
            Debug.LogWarning("곡 이름을 기입해주세요.");
            return;
        }
        if (song_Artist.text == "")
        {
            Debug.LogWarning("아티스트 이름을 기입해주세요.");
            return;
        }
        if (bgmName_TMP.text == "BGM 선택")
        {
            Debug.LogWarning("곡을 선택해주세요.");
            return;
        }
        if (thumbnailName_TMP.text == "썸네일 선택")
        {
            Debug.LogWarning("썸네일을 선택해주세요.");
            return;
        }

        //프로젝트 데이터 업데이트
        currentProject.projectData.projectName = project_Name.text;
        currentProject.projectData.artistName = song_Artist.text;
        currentProject.projectData.bpm = int.Parse(project_BPM.text);

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
        currentProject.projectData.thumbnailName = thumbnailName_TMP.text;

        currentProject.projectData.bgmName = bgmName_TMP.text;

        addProejct_BTN.interactable = true;

        edit_BTN.interactable = true;

        string path = Path.Combine(projectIO.ProjectPath, project_Name.text);

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
        thumbnail_IMG.sprite = null;
        project_Name.text = null;
        song_Artist.text = null;
        bgmName_TMP.text = null;
        thumbnailName_TMP.text = null;
        SetDefault();
    }

    private void Refresh()
    {
        Debug.Log("미구현");
    }

    private void AddNewProject()
    {
        GameObject project = Instantiate(newProjectPrefab, project_Rect, false);
        currentProject = project.GetComponent<Project>();
        addedProjects.Add(currentProject);

        project_Name.onEndEdit.RemoveAllListeners();
        song_Artist.onEndEdit.RemoveAllListeners();

        project_Name.onEndEdit.AddListener(currentProject.SetName);
        song_Artist.onEndEdit.AddListener(currentProject.SetArtist);

        project_Name.interactable = true;
        song_Artist.interactable = true;
        loadSong_BTN.interactable = true;
        loadThumbnail_BTN.interactable = true;

        addProejct_BTN.interactable = false;

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
        edit_BTN.interactable = false;
        project_Name.interactable = false;
        song_Artist.interactable = false;
        loadSong_BTN.interactable = false;
        loadThumbnail_BTN.interactable = false;

        addProejct_BTN.interactable = true;
    }
}
