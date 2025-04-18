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
    [SerializeField] private TextMeshProUGUI bgmName_tmp;
    [SerializeField] private TextMeshProUGUI keySound_tmp;
    [SerializeField] private TextMeshProUGUI projectName_tmp;
    [SerializeField] private TextMeshProUGUI artistName_tmp;
    [SerializeField] private TextMeshProUGUI bpm_tmp;
    [SerializeField] private TextMeshProUGUI beatNum_tmp;
    [SerializeField] private TextMeshProUGUI soloBgm_tmp;
    [SerializeField] private TextMeshProUGUI duo1Bgm_tmp;
    [SerializeField] private TextMeshProUGUI duo2Bgm_tmp;
    [SerializeField] private Button loadSong_btn;
    [SerializeField] private Button loadKeysound_btn;
    [SerializeField] private Button edit_btn;
    [SerializeField] private Button save_btn;
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI test;
    [SerializeField] private List<Toggle> gameModeTogs;
    [SerializeField] private List<Toggle> difficultyTogs;
    private TextMeshProUGUI currBgm_tmp;
    private Enums.GameMode gameMode;
    private Enums.GameMode GameMode
    {
        get { return gameMode; }
        set
        {
            gameMode = value;
            Debug.LogWarning(gameMode);
            switch (gameMode)
            {
                case Enums.GameMode.Solo:
                    currBgm_tmp = soloBgm_tmp;
                    bgmName = soloBgmName;
                    break;
                case Enums.GameMode.Duo1:
                    currBgm_tmp = duo1Bgm_tmp;
                    bgmName = duo1BgmName;
                    break;
                case Enums.GameMode.Duo2:
                    currBgm_tmp = duo2Bgm_tmp;
                    bgmName = duo2BgmName;
                    break;
                default:
                    Debug.Log("No specific game mode selected.");
                    break;
            }
        }
    }
    private Enums.Difficulty difficulty;
    private Enums.Difficulty Difficulty
    {
        get { return difficulty; }
        set
        {
            difficulty = value;
            Debug.LogWarning(difficulty);
        }
    }
    private Enums.ModeDiff modeDiff;
    private string bgmDestPath;
    private string bgmName;
    public Sprite thumbnailSprite
    {
        get { return thumbnail.sprite; }
        set { thumbnail.sprite = value; }
    }
    public Project currProject;
    public string soloBgmName = "Solo_Theme";
    public string duo1BgmName = "Duo1_Theme";
    public string duo2BgmName = "Duo2_Theme";
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
        loadSong_btn.onClick.AddListener(LoadSong);
        loadKeysound_btn.onClick.AddListener(KeySoundPathSet);
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
        currBgm_tmp = soloBgm_tmp;
        bgmName = soloBgmName;
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

    private void LoadSong()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Sound Files", SoundFileExtensions)
        };
        try
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("곡을 선택해주세요.", "", extensions, false);
            currBgm_tmp.text = Path.GetFileName(path[0]);
            currProject.projectData.bgmPath = path[0];
            currProject.SetBgm(bgmName_tmp.text);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.FILELOADFAIL);
        }
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
            currProject.SetKeySoundPath(keysoundPath);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.PATHSETERROR);
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
    }
    private void SaveProjectInfos()
    {
        SetBgm(bgmName);
    }

    public void SetBgm(string bgmName)
    {
        if (string.IsNullOrEmpty(currBgm_tmp.text))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NONEBGM);
            return;
        }
        string bgmSavePath = Path.Combine(currProject.projectData.m_Path, "bgmSaveFile");
        string fileName = Path.GetFileName(currBgm_tmp.text);
        string[] extension = fileName.Split('.');
        bgmDestPath = Path.Combine(bgmSavePath, bgmName + '.' + extension[1]);
        Directory.CreateDirectory(bgmSavePath);
        try
        {
            File.Copy(currProject.projectData.bgmPath, bgmDestPath);
        }
        catch (Exception err)
        {
            Debug.LogError(err.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.THEMEALREADYEXIST);
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
