using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class KeySoundLoader : MonoBehaviour
{
    [SerializeField] private GameObject keysound_prefab;
    [SerializeField] private ToggleGroup toggleGroup;
    private string keySoundPath = "Assets/Resources/_SongEditor/KeySoundTemp";
    private void Awake()
    {
        LoadKeySound();
    }

    private void Start()
    {

    }

    private void LoadKeySound()
    {
        if (Directory.Exists(keySoundPath))
        {
            Directory.Delete(keySoundPath, true);
        }
        Directory.CreateDirectory(keySoundPath);
        string path = EditorDataManager.Instance.ProjectData.m_KeysoundPath;
        string[] files = Directory.GetFiles(path);

        List<string> fileNameList = new List<string>();
        List<string> filesList = new List<string>();
        filesList.AddRange(files);
        List<string> sortList = filesList.OrderBy(x =>
        {
            var splitResult = x.Split('-');
            var secondValue = splitResult[1].Split('.')[0];
            int value = int.Parse(secondValue);
            return value;
        }).ToList();

        string fileName;
        string destPath;

        foreach (string file in sortList)
        {
            fileName = Path.GetFileName(file);
            fileNameList.Add(Path.GetFileNameWithoutExtension(file));
            destPath = Path.Combine(keySoundPath, fileName);
            File.Copy(file, destPath);
        }
        AssetDatabase.Refresh();
        foreach (string file in fileNameList)
        {
            KeySound keySound = Instantiate(keysound_prefab, transform, false).GetComponent<KeySound>();
            keySound.Toggle.group = toggleGroup;
            keySound.audioSource.clip = Resources.Load<AudioClip>("_SongEditor/KeySoundTemp/" + file);
            keySound.KeysoundName = file;
            keySound.PlayBTN.onClick.AddListener(keySound.audioSource.Play);
        }
    }

}
