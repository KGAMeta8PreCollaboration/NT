using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using SFB;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using Detail = Enums.Details;
public class EditProject : MonoBehaviour
{

    private static readonly string[] SoundFileExtensions = { "mp3", "wav", "ogg" };
    private static readonly string[] VaildKeySoundExtensions = { ".wav", ".mp3", ".ogg" };
    private Dictionary<Toggle, Enums.GameMode> gameModeDic = new Dictionary<Toggle, Enums.GameMode>();
    private Dictionary<Toggle, Enums.Difficulty> diffDic = new Dictionary<Toggle, Enums.Difficulty>();
    [SerializeField] private TextMeshProUGUI bgmHighlight_tmp;
    [SerializeField] private TextMeshProUGUI bgmName_tmp;
    [SerializeField] private TextMeshProUGUI projectName_tmp;
    [SerializeField] private TextMeshProUGUI artistName_tmp;
    [SerializeField] private TextMeshProUGUI bpm_tmp;
    [SerializeField] private TextMeshProUGUI beatNum_tmp;
    [SerializeField] private TextMeshProUGUI phase1Keysound_tmp;
    [SerializeField] private TextMeshProUGUI phase2Keysound_tmp;
    [SerializeField] private TextMeshProUGUI phase3Keysound_tmp;
    [SerializeField] private Button loadHightlight_btn;
    [SerializeField] private Button loadSong_btn;
    [SerializeField] private Button phase1Keysound_btn;
    [SerializeField] private Button phase2Keysound_btn;
    [SerializeField] private Button phase3Keysound_btn;
    [SerializeField] private Button edit_btn;
    [SerializeField] private Button save_btn;
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI test;
    [SerializeField] private List<Toggle> gameModeTogs;
    [SerializeField] private List<Toggle> difficultyTogs;
    private Enums.GameMode gameMode;
    private Enums.GameMode GameMode
    {
        get { return gameMode; }
        set { gameMode = value; }
    }
    private Enums.Difficulty difficulty;
    private Enums.Difficulty Difficulty
    {
        get { return difficulty; }
        set { difficulty = value; }
    }
    private Enums.ModeDiff modeDiff;
    private string bgmDestPath;
    public Sprite thumbnailSprite
    {
        get { return thumbnail.sprite; }
        set { thumbnail.sprite = value; }
    }
    public Project currProject;
    public string bgmName = "MainTheme";
    public string highlightName = "BGM_Highlight";
    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        LoadProjectInfos();
    }
    private void Initialize()
    {
        // loadHightlight_btn.onClick.AddListener()
        loadSong_btn.onClick.AddListener(LoadSong);
        phase1Keysound_btn.onClick.AddListener(() => KeySoundPathSet(phase1Keysound_tmp));
        phase2Keysound_btn.onClick.AddListener(() => KeySoundPathSet(phase2Keysound_tmp));
        phase3Keysound_btn.onClick.AddListener(() => KeySoundPathSet(phase3Keysound_tmp));
        edit_btn.onClick.AddListener(LoadSongEditorScene);
        save_btn.onClick.AddListener(SaveProjectInfos);
        for (int i = 0; i < difficultyTogs.Count; i++)
        {
            if (i < gameModeTogs.Count)
            {
                gameModeDic.Add(gameModeTogs[i], gameMode);
                gameModeTogs[i].onValueChanged.AddListener(SetGameMode(gameModeTogs[i]));
                gameMode++;
            }
            diffDic.Add(difficultyTogs[i], difficulty);
            difficultyTogs[i].onValueChanged.AddListener(SetDifficulty(difficultyTogs[i]));
            difficulty++;
        }
        gameMode = 0;
        difficulty = 0;
    }

    private UnityAction<bool> SetGameMode(Toggle gameModeTog)
    {
        UnityAction<bool> action = isOn =>
        {
            if (isOn)
            {
                if (gameModeDic.TryGetValue(gameModeTog, out var selectedGameMode))
                {
                    GameMode = selectedGameMode;
                }
            }
        };
        return action;
    }
    private UnityAction<bool> SetDifficulty(Toggle difficultyTog)
    {
        UnityAction<bool> action = isOn =>
        {
            if (isOn)
            {
                if (diffDic.TryGetValue(difficultyTog, out var selectedDiff))
                {
                    Difficulty = selectedDiff;
                }
            }
        };
        return action;
    }
    private void LoadHighlight()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Sound Files", SoundFileExtensions)
        };
        try
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("하이라이트 음원을 선택해주세요.", "", extensions, false);
            bgmHighlight_tmp.text = Path.GetFileName(path[0]);
            currProject.projectData.highlightPath = path[0];
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.FileLoadFail);
        }
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
            bgmName_tmp.text = path[0];
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.FileLoadFail);
        }
    }

    private void KeySoundPathSet(TextMeshProUGUI tmp)
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
                EditorUIManager.Instance.popUp.PopUpOpen(Detail.FileDetectFail);
                return;
            }
            string keysoundPath = Path.GetFullPath(path[0]);
            tmp.text = keysoundPath;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.PathSetError);
        }
    }
    private void LoadSongEditorScene()
    {
        // EditorDataManager.Instance.thumbnail_sprite = thumbnail_img.sprite;
        // EditorDataManager.Instance.ProjectData = currentProject.projectData;
        // EditorDataManager.Instance.SetBgm();
        // EditorLoadScene.SceneLoad("SongEditorScene");
        string path = currProject.projectData.m_Path;
        modeDiff = (Enums.ModeDiff)((int)gameMode * 4 + (int)difficulty);
    }

    private void LoadProjectInfos()
    {
        projectName_tmp.text = currProject.projectData.projectName;
        artistName_tmp.text = currProject.projectData.artistName;
        bpm_tmp.text = currProject.projectData.bpm.ToString();
        beatNum_tmp.text = currProject.projectData.beatNum.ToString();
        if (true == File.Exists(currProject.projectData.bgmPath))
        {
            bgmName_tmp.text = currProject.projectData.bgmPath;
        }
    }
    private void SaveProjectInfos()
    {
        EditorDataManager.Instance.ProjectInfoSave(currProject.projectData);
        SaveBgm();
    }

    private void SaveBgm()
    {
        if (string.IsNullOrEmpty(bgmName_tmp.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NoneBgm);
            return;
        }
        string bgmSavePath = Path.Combine(currProject.projectData.m_Path, "bgmSaveFile");
        string fileName = Path.GetFileName(bgmName_tmp.text);
        string[] extension = fileName.Split('.');
        bgmDestPath = Path.Combine(bgmSavePath, bgmName + '.' + extension[1]);
        Directory.CreateDirectory(bgmSavePath);
        try
        {
            File.Copy(bgmName_tmp.text, bgmDestPath);
            currProject.projectData.bgmPath = bgmDestPath;
        }
        catch (Exception err)
        {
            Debug.LogError(err.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.ThemeAlreadyExist);
        }
        StartCoroutine(InstantiateBGM());
    }

    private IEnumerator InstantiateBGM()
    {
        AudioClip clip;

        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(bgmDestPath, AudioType.WAV);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error loading audio clip : {request.error}");
        }
        clip = DownloadHandlerAudioClip.GetContent(request);
        clip.name = bgmName;
        EditorDataManager.Instance.bgmClip = clip;
        yield return null;
    }
}
