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

public class ResourceIO : MonoBehaviour
{

    private static ResourceIO instance;
    public static ResourceIO Instance { get { return instance; } }
    [SerializeField] private string loadFolderPath;
    private string saveDataPath = "\\Night Traveler\\Editor\\Song\\";
    private string loadDataPath = "\\Editor\\Song\\";
    // [SerializeField] private PopUp popUp;
    public Dictionary<Enums.ModeDiff, List<SongData>> keyValuePairs = new Dictionary<Enums.ModeDiff, List<SongData>>();
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
    // public void BrowserForFile()
    // {
    //     var extensions = new[]
    //     {
    //         new ExtensionFilter("Sound Files", "mp3", "wav")
    //     };

    //     var paths = StandaloneFileBrowser.OpenFilePanel("Open Song File", "", extensions, false);
    //     Debug.Log(paths[0]);

    //     string fileName = Path.GetFileName(paths[0]);
    //     Debug.Log(fileName);
    //     //Company 하위 경로
    //     string DestFile = Path.Combine(Application.persistentDataPath + fileName);

    //     File.Copy(paths[0], DestFile, true);
    // }

    public void BrowserForSave()
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("저장 경로 선택", "", false);
        currentPath = paths[0] + saveDataPath;
        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
        }
        saveDelegate?.Invoke();
        Save("Phase");
    }

    public void BrowserForLoad()
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("불러올 경로 선택", "", false);
        string[] loadPath = Directory.GetDirectories(paths[0] + loadDataPath);
        //TODO 차후 Enums.ModeDiff 상수 사용
        string tempPath;
        Enums.ModeDiff modeDiff = Enums.ModeDiff.SOLO_EASY;
        for (int i = 0; i < 4; i++)
        {
            modeDiff += i;
            try
            {
                tempPath = loadPath + modeDiff.ToString();

                // string[] fromJsonData = Directory.GetFiles(currentPath);
                // Debug.Log(fromJsonData[0]);
                // SongData songData = JsonUtility.FromJson<SongData>(fromJsonData[0]);
                // Debug.Log(songData.a);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

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

    public void Save(string fileName)
    {
        string savePath = currentPath + fileName + "\\";
        // string saveData = JsonUtility.ToJson(saveInfo);
        string saveData = DictionaryJsonUtility.ToJson(keyValuePairs, true);
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        Debug.Log($"in Save Path : {savePath}");

        File.WriteAllText(savePath + fileName, saveData);
    }
}