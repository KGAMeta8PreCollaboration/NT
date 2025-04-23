using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Michsky.UI.Heat;
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
    [SerializeField] private TextMeshProUGUI singleBgmName_tmp;
    [SerializeField] private TextMeshProUGUI multiBgmName_tmp;
    [SerializeField] private TextMeshProUGUI projectName_tmp;
    [SerializeField] private TextMeshProUGUI artistName_tmp;
    [SerializeField] private TextMeshProUGUI bpm_tmp;
    [SerializeField] private TextMeshProUGUI beatNum_tmp;
    [SerializeField] private TextMeshProUGUI phase1Keysound_tmp;
    [SerializeField] private TextMeshProUGUI phase2Keysound_tmp;
    [SerializeField] private TextMeshProUGUI phase3Keysound_tmp;
    [SerializeField] private Button loadHightlight_btn;
    [SerializeField] private Button loadSingleSong_btn;
    [SerializeField] private Button loadMultiSong_btn;
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
    public Sprite thumbnailSprite
    {
        get { return thumbnail.sprite; }
        set { thumbnail.sprite = value; }
    }
    public Project currProject;
    private string singleBgmName = "Single_Theme";
    private string multiBgmName = "Multi_Theme";
    private string highlightName = "BGM_Highlight";
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
        loadHightlight_btn.onClick.AddListener(() => LoadSongData(bgmHighlight_tmp, ref currProject.projectData.highlightPath, "하이라이트 음원을 선택해주세요."));
        loadSingleSong_btn.onClick.AddListener(() => LoadSongData(singleBgmName_tmp, ref currProject.projectData.singleBgmPath, "곡을 선택해주세요."));
        loadMultiSong_btn.onClick.AddListener(() => LoadSongData(multiBgmName_tmp, ref currProject.projectData.multiBgmPath, "곡을 선택해주세요."));
        phase1Keysound_btn.onClick.AddListener(() =>
        KeySoundPathSet(phase1Keysound_tmp, ref currProject.projectData.phase1KeysoundPath));
        phase2Keysound_btn.onClick.AddListener(() =>
        KeySoundPathSet(phase2Keysound_tmp, ref currProject.projectData.phase2KeysoundPath));
        phase3Keysound_btn.onClick.AddListener(() =>
        KeySoundPathSet(phase3Keysound_tmp, ref currProject.projectData.phase3KeysoundPath));
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
    private void LoadSongData(TextMeshProUGUI tmp, ref string savePath, string panelText = null)
    {
        var extensions = new[]
        {
            new ExtensionFilter("Sound Files", SoundFileExtensions)
        };
        try
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel(panelText, "", extensions, false);
            savePath = path[0];
            tmp.text = Path.GetFileName(savePath);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.FileLoadFail);
        }
    }
    private void KeySoundPathSet(TextMeshProUGUI tmp, ref string originPath)
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
            originPath = keysoundPath;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.PathSetError);
        }
    }
    private void LoadSongEditorScene()
    {
        if (string.IsNullOrEmpty(currProject.projectData.singleBgmPath) ||
            string.IsNullOrEmpty(currProject.projectData.multiBgmPath) ||
            string.IsNullOrEmpty(currProject.projectData.highlightPath) ||
            string.IsNullOrEmpty(currProject.projectData.phase1KeysoundPath) ||
            string.IsNullOrEmpty(currProject.projectData.phase2KeysoundPath) ||
            string.IsNullOrEmpty(currProject.projectData.phase3KeysoundPath))
        {
            Debug.LogError("모든 정보가 입력되지 않음");
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.FileLoadFail);
            return;
        }
        currProject.projectData.modeDiff = (Enums.ModeDiff)((int)gameMode * 4 + (int)difficulty);
        EditorDataManager.Instance.thumbnail_sprite = currProject.Sprite;
        EditorDataManager.Instance.ProjectData = currProject.projectData;
        switch (gameMode)
        {
            case Enums.GameMode.Solo:
                StartCoroutine(InstantiateBGM(Path.Combine(currProject.projectData.m_Path, "bgmSaveFile", singleBgmName + ".wav")));
                break;
            case Enums.GameMode.Duo1:
            case Enums.GameMode.Duo2:
                StartCoroutine(InstantiateBGM(Path.Combine(currProject.projectData.m_Path, "bgmSaveFile", multiBgmName + ".wav")));
                break;
            default:
                Debug.LogError("게임 모드 설정 오류");
                break;
        }
    }

    private void LoadProjectInfos()
    {
        projectName_tmp.text = currProject.projectData.projectName;
        artistName_tmp.text = currProject.projectData.artistName;
        bpm_tmp.text = currProject.projectData.bpm.ToString();
        beatNum_tmp.text = currProject.projectData.beatNum.ToString();
        FileExistCheck(currProject.projectData.singleBgmPath, singleBgmName_tmp);
        FileExistCheck(currProject.projectData.multiBgmPath, multiBgmName_tmp);
        FileExistCheck(currProject.projectData.highlightPath, bgmHighlight_tmp);
        phase1Keysound_tmp.text = currProject.projectData.phase1KeysoundPath;
        phase2Keysound_tmp.text = currProject.projectData.phase2KeysoundPath;
        phase3Keysound_tmp.text = currProject.projectData.phase3KeysoundPath;
    }

    private void SaveProjectInfos()
    {
        if (Directory.Exists(Path.Combine(currProject.projectData.m_Path, "bgmSaveFile")))
        {
            Directory.Delete(Path.Combine(currProject.projectData.m_Path, "bgmSaveFile"), true);
        }
        SaveSong(singleBgmName, currProject.projectData.singleBgmPath);
        SaveSong(multiBgmName, currProject.projectData.multiBgmPath);
        SaveSong(highlightName, currProject.projectData.highlightPath);
        EditorDataManager.Instance.ProjectInfoSave(currProject.projectData);
    }

    private void SaveSong(string fileName, string originPath)
    {
        if (string.IsNullOrEmpty(originPath))
        {
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.NoneBgm);
            return;
        }
        string bgmSavePath = Path.Combine(currProject.projectData.m_Path, "bgmSaveFile");
        string originName = Path.GetFileName(originPath);
        string[] extension = originName.Split('.');
        string bgmDestPath = Path.Combine(bgmSavePath, fileName + '.' + extension[1]);
        Directory.CreateDirectory(bgmSavePath);
        try
        {
            File.Copy(originPath, bgmDestPath);
        }
        catch (Exception err)
        {
            Debug.LogError(err.Message);
            EditorUIManager.Instance.popUp.PopUpOpen(Detail.ThemeAlreadyExist);
        }
    }

    private IEnumerator InstantiateBGM(string bgmDestPath)
    {
        AudioClip clip;
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(bgmDestPath, AudioType.WAV);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error loading audio clip : {request.error}");
        }
        clip = DownloadHandlerAudioClip.GetContent(request);
        clip.name = Path.GetFileName(bgmDestPath);
        EditorDataManager.Instance.bgmClip = clip;
        print(EditorDataManager.Instance.bgmClip.name);
        yield return null;
        EditorLoadScene.SceneLoad("SongEditorScene");
    }

    private void FileExistCheck(string path, TextMeshProUGUI tmp)
    {
        if (true == File.Exists(path))
        {
            tmp.text = Path.GetFileName(path);
        }
        else
        {
            tmp.text = "";
        }
    }
}
