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
    [SerializeField] private string loadFolderPath;
    [SerializeField] private string saveDataPath = null;
    // [SerializeField] private PopUp popUp;

    public AudioClip audioClip;
    public string Dest;

    // public void OpenFolderBrowser() =>
    //     StandaloneFileBrowser.OpenFolderPanelAsync("", "", false, paths =>
    //     {
    //         if (paths.Length <= 0) return;

    //         var installation = paths[0];
    //         loadFolderPath = installation;
    //     });
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
}