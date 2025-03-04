using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;
using TMPro;
using UnityEngine;
using SFB;
using UnityEditor;
using UnityEngine.Events;

public class MusicDriver : MonoBehaviour
{
    private static MusicDriver instance;
    public static MusicDriver Instance { get { return instance; } }
    [SerializeField] private string loadFolderPath;
    private string saveDataPath = "\\Night Traveler\\Editor\\Song\\";
    // [SerializeField] private PopUp popUp;
    public Dictionary<Enums.ModeDiff, List<PhaseElement>> keyValuePairs = new Dictionary<Enums.ModeDiff, List<PhaseElement>>();
    private string currentPath;
    public AudioClip audioClip;
    public delegate void SaveDelegate();

    public SaveDelegate saveDelegate;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else DestroyImmediate(gameObject);
    }
    private void Start()
    {
        if (saveDataPath == null)
        {

        }
    }
    public void BrowserForFile()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Sound Files", "mp3", "wav")
        };

        var paths = StandaloneFileBrowser.OpenFilePanel("Open Song File", "", extensions, false);
        Debug.Log(paths[0]);

        string fileName = Path.GetFileName(paths[0]);
        Debug.Log(fileName);
        //Company 하위 경로
        string DestFile = Path.Combine(Application.persistentDataPath + fileName);

        File.Copy(paths[0], DestFile, true);
    }

    public void BrowserForSave()
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("저장 경로 선택", "", false);
        currentPath = paths[0] + saveDataPath;
        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
        }
        saveDelegate?.Invoke();

    }
    // public void FindSaveFiles()
    // {
    //     var paths = StandaloneFileBrowser.OpenFolderPanel("폴더 불러오기", "", false);

    //     string fullPath = Path.Combine(Environment.CurrentDirectory, saveDataPath);

    //     try
    //     {
    //         string content = File.ReadAllText(fullPath);
    //         Debug.Log(content);
    //     }
    //     catch (FileNotFoundException)
    //     {
    //         Debug.LogError($"File not found: {fullPath}");
    //     }
    //     catch (Exception ex)
    //     {
    //         Debug.LogError($"An error occurred: {ex.Message}");
    //     }
    // }
    public void Save(SongData saveInfo, Enums.ModeDiff modeDiff, string fileName)
    {
        string savePath = currentPath + modeDiff.ToString() + "\\";
        string saveData = JsonUtility.ToJson(saveInfo);
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        Debug.Log($"in Save Path : {savePath}");

        File.WriteAllText(savePath + fileName, saveData);
    }
}