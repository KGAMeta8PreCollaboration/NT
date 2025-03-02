using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;
using SFB;
using TMPro;
using UnityEngine;

public class MusicDriver : MonoBehaviour
{
    [SerializeField] private string loadFolderPath;

    public void OpenFolderBrowser() =>
        StandaloneFileBrowser.OpenFolderPanelAsync("", "", false, paths =>
        {
            if (paths.Length <= 0) return;

            var installation = paths[0];
            loadFolderPath = installation;
            Debug.Log(installation);
        });
}