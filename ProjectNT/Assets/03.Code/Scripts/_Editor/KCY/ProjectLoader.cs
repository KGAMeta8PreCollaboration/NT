using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectLoader : MonoBehaviour
{
    [SerializeField] private GameObject newProjectPrefab;
    [SerializeField] private RectTransform project_Rect;
    [SerializeField] private Button addProejct_BTN;
    [SerializeField] private Button refreah_BTN;
    [SerializeField] private Button delete_BTN;
    [SerializeField] private Image thumbnail_IMG;
    [SerializeField] private TMP_InputField song_Name;
    [SerializeField] private TMP_InputField song_Artist;
    [SerializeField] private TextMeshProUGUI bgmName_TMP;
    [SerializeField] private TextMeshProUGUI thumbnailName_TMP;
    [SerializeField] private Button loadSong_BTN;
    [SerializeField] private Button loadThumbnail_BTN;
    [SerializeField] private Button edit_BTN;
    [HideInInspector] public Project currentProject;

    public ToggleGroup projects_Group;

    private void Awake()
    {
        newProjectPrefab = Resources.Load<GameObject>("_SongEditor/Prefabs/NewProject");
        addProejct_BTN.onClick.AddListener(AddNewProject);
        refreah_BTN.onClick.AddListener(Refresh);
        delete_BTN.onClick.AddListener(Delete);
        edit_BTN.onClick.AddListener(EditProject);
        loadSong_BTN.onClick.AddListener(LoadSong);
        loadThumbnail_BTN.onClick.AddListener(LoadThumbnail);
    }

    private void LoadSong()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Sound Files", "mp3", "wav","ogg")
        };
        string[] path = StandaloneFileBrowser.OpenFilePanel("곡을 선택해주세요.", "", extensions, false);

        bgmName_TMP.text = Path.GetFileName(path[0]);

        Directory.CreateDirectory(Application.persistentDataPath + "\\Audio");

        string DestFilePath =
        Path.Combine(Application.persistentDataPath + "Audio" + bgmName_TMP.text);

        File.Copy(path[0], DestFilePath, true);
    }

    private void LoadThumbnail()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Image Files", "jpeg","png","jpg")
        };
        string[] path = StandaloneFileBrowser.OpenFilePanel("썸네일을 선택해주세요.", "", extensions, false);

        thumbnailName_TMP.text = Path.GetFileName(path[0]);

        Directory.CreateDirectory(Application.persistentDataPath + "\\Thumbnails");

        string DestFilePath =
        Path.Combine(Application.persistentDataPath + "Thumbnails" + thumbnailName_TMP.text);
    }

    private void EditProject()
    {
        if (song_Name.text == null)
        {
            Debug.LogWarning("곡 이름을 기입해주세요.");
            return;
        }
        if (song_Artist.text == null)
        {
            Debug.LogWarning("아티스트 이름을 기입해주세요.");
            return;
        }
        if (thumbnailName_TMP.text == null)
        {
            Debug.LogWarning("썸네일을 선택해주세요.");
            return;
        }
        if (bgmName_TMP.text == null)
        {
            Debug.LogWarning("곡을 선택해주세요.");
            return;
        }

        currentProject.projectData.songName = song_Name.text;
        currentProject.projectData.artistName = song_Artist.text;
        currentProject.projectData.thumbnailName = thumbnailName_TMP.text;
        currentProject.projectData.bgmName = bgmName_TMP.text;
    }

    private void Delete()
    {

    }

    private void Refresh()
    {
        Debug.Log("미구현");
    }

    private void AddNewProject()
    {
        GameObject project = Instantiate(newProjectPrefab, project_Rect, false);
        currentProject = project.GetComponent<Project>();
    }
}
