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

public class ResourceIO : MonoBehaviour
{

    private static ResourceIO instance;
    public static ResourceIO Instance { get { return instance; } }
    [SerializeField] private string loadFolderPath;
    private string saveDataPath = "\\Night Traveler\\Editor\\Song\\";
    private string loadDataPath = "\\Editor\\Song\\Phase\\";
    private string fileName = "SaveData";
    public Dictionary<Enums.ModeDiff, List<SongData>> Phase_Dic =
    new Dictionary<Enums.ModeDiff, List<SongData>>();

    private string dataPath;
    public AudioClip audioClip;
    public delegate void SaveDelegate();
    public delegate void LoadDelegate();

    public SaveDelegate saveDelegate;
    public LoadDelegate loadDelegate;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else DestroyImmediate(gameObject);
    }

    public void BrowserForSave()
    {
        if (dataPath == null)
        {
            try
            {
                var paths = StandaloneFileBrowser.OpenFolderPanel("저장 경로 선택", "", false);
                string[] temp = Directory.GetDirectories(paths[0]);
                dataPath = paths[0] + saveDataPath;

                if (!Directory.Exists(dataPath))
                {
                    Directory.CreateDirectory(dataPath);
                }

            }
            catch
            {
                Debug.LogError("경로를 불러오지 못했습니다");
            }
            saveDelegate?.Invoke();
            Save(fileName);
        }
        else
        {
            saveDelegate?.Invoke();
            Save(fileName);
        }

    }

    public void BrowserForLoad()
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("불러올 경로 선택", "", false);
        Debug.Log("하위 모든 파일을 읽어옵니다.");
        string[] allfiles = Directory.GetFiles(paths[0], "*.*", SearchOption.AllDirectories);
        foreach (string s in allfiles)
        {
            try
            {
                string jsonFile = File.ReadAllText(s);
                Debug.Log(s);
                Debug.Log("위 파일을 읽어오는중");
                Phase_Dic = DictionaryJsonUtility.FromJson<Enums.ModeDiff, List<SongData>>(jsonFile);
                loadDelegate?.Invoke();
                Debug.Log("데이터 적용 성공");
            }
            catch
            {
                Debug.LogWarning(s);
                Debug.LogWarning("불러오기 실패 다른 파일을 읽습니다.");
                continue;
            }
            finally
            {
                Debug.Log("로드할 파일이 존재하지 않거나 이미 불러왔습니다.");
            }
        }

    }

    public void Save(string fileName)
    {
        string savePath = dataPath + fileName + "\\";
        string saveData = DictionaryJsonUtility.ToJson(Phase_Dic, true);
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        Debug.Log($"in Save Path : {savePath}");
        File.WriteAllText(savePath + fileName, saveData);
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
}
