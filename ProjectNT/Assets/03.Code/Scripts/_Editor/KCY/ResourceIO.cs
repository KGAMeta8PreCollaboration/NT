using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using SFB;
using UnityEngine.UI;

public class ResourceIO : MonoBehaviour
{
    [SerializeField] private KeySoundLoader keySoundLoader;
    [SerializeField] private TMP_InputField phase2Inputfield;
    [SerializeField] private TMP_InputField phase3Inputfield;
    private string bgmSavePath = "Assets/Resources/_SongEditor/LoadedSong";
    private string keySoundsavePath = "Assets/Resources/_SongEditor/KeySoundTemp";

    private void Awake()
    {
        keySoundLoader.LoadKeySound(keySoundsavePath);
        SetBgm();
        AssetDatabase.Refresh();
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

}
