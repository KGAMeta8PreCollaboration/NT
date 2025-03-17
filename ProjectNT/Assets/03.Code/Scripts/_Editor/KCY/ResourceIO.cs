using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ResourceIO : MonoBehaviour
{
    [SerializeField] private KeySoundLoader keySoundLoader;
    [SerializeField] private TMP_InputField phase2_inputfield;
    [SerializeField] private TMP_InputField phase3_inputfield;
    [SerializeField] private TextMeshProUGUI songName_tmp;
    [SerializeField] private Image thumbnail_img;
    private string bgmDestPath;
    private bool isSaved = true;
    public bool IsSaved
    { get { return isSaved; } set { isSaved = value; } }

    private void Awake()
    {
        keySoundLoader.LoadKeySound();
        SetBgm();

        phase2_inputfield.onValueChanged.AddListener((word) => phase2_inputfield.text = Regex.Replace(word, @"[0-9]", ""));
        phase3_inputfield.onValueChanged.AddListener((word) => phase3_inputfield.text = Regex.Replace(word, @"[0-9]", ""));
        songName_tmp.text = EditorDataManager.Instance.ProjectData.projectName;
        thumbnail_img.sprite = EditorDataManager.Instance.thumbnail_sprite;
        SaveTracker();
    }
    private void Start()
    {
    }

    private IEnumerator InstantiateBGM()
    {
        yield return null;
        AudioClip clip;
        while (true)
        {
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(bgmDestPath, AudioType.WAV);
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error loading audio clip : {request.error}");
                continue;
            }
            clip = DownloadHandlerAudioClip.GetContent(request);
            clip.name = EditorDataManager.Instance.ProjectData.bgmName;
            EditorDataManager.Instance.bgmClip = clip;
        }
    }

    private void SetBgm()
    {
        string bgmSavePath = Path.Combine(Application.persistentDataPath, "bgmSaveFile");
        string bgmPath = Path.Combine(EditorDataManager.Instance.ProjectData.m_Path, EditorDataManager.Instance.ProjectData.bgmName);

        bgmDestPath = Path.Combine(bgmSavePath, EditorDataManager.Instance.ProjectData.bgmName);
        if (Directory.Exists(bgmSavePath))
        {
            Directory.Delete(bgmSavePath, true);
        }
        Directory.CreateDirectory(bgmSavePath);
        File.Copy(bgmPath, bgmDestPath);
        StartCoroutine(InstantiateBGM());
    }

    private void SaveTracker()
    {
        phase2_inputfield.onValueChanged.AddListener(x => IsSaved = false);
        phase3_inputfield.onValueChanged.AddListener(x => IsSaved = false);
    }

}
