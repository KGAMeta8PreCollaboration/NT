using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using TMPro;
using UnityEngine;
using UnityEditor;

using UnityEngine.UI;

public class ResourceIO : MonoBehaviour
{
    [SerializeField] private KeySoundLoader keySoundLoader;
    [SerializeField] private TMP_InputField phase2_inputfield;
    [SerializeField] private TMP_InputField phase3_inputfield;
    [SerializeField] private TextMeshProUGUI songName_tmp;
    [SerializeField] private Image thumbnail_img;
    private string bgmSavePath = "Assets/Resources/_SongEditor/LoadedSong";
    private string keySoundsavePath = "Assets/Resources/_SongEditor/KeySoundTemp";
    private bool isSaved = true;
    public bool IsSaved
    { get { return isSaved; } set { isSaved = value; } }

    private void Awake()
    {
        keySoundLoader.LoadKeySound(keySoundsavePath);
        SetBgm();
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        phase2_inputfield.onValueChanged.AddListener((word) => phase2_inputfield.text = Regex.Replace(word, @"[0-9]", ""));
        phase3_inputfield.onValueChanged.AddListener((word) => phase3_inputfield.text = Regex.Replace(word, @"[0-9]", ""));

        SaveTracker();
    }
    private void Start()
    {
        EditorDataManager.Instance.LoadBeatMapData();
    }

    private void SetBgm()
    {
        string bgmPath = Path.Combine(EditorDataManager.Instance.ProjectData.m_Path, EditorDataManager.Instance.ProjectData.bgmName);
        string bgmDestPath = Path.Combine(bgmSavePath, EditorDataManager.Instance.ProjectData.bgmName);
        if (Directory.Exists(bgmSavePath))
        {
            Directory.Delete(bgmSavePath, true);
        }
        Directory.CreateDirectory(bgmSavePath);
        File.Copy(bgmPath, bgmDestPath);
    }

    private void SaveTracker()
    {
        phase2_inputfield.onValueChanged.AddListener(x => IsSaved = false);
        phase3_inputfield.onValueChanged.AddListener(x => IsSaved = false);
    }

}
