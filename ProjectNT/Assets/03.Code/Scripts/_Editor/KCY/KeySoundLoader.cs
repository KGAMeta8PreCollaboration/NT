using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class KeySoundLoader : MonoBehaviour
{
    [SerializeField] private GameObject keysound_prefab;
    [SerializeField] private ToggleGroup toggleGroup;
    private string keySoundPath = "Assets/Resources/_SongEditor/KeySoundTemp";
    private int keysound_count;
    private void Awake()
    {
        LoadKeySound();
    }

    private void LoadKeySound()
    {
        string path = EditorDataManager.Instance.ProjectData.m_KeysoundPath;
        string[] files = Directory.GetFiles(path);
        keysound_count = files.Length;
        string fileName;
        string destPath;

        foreach (string file in files)
        {
            fileName = Path.GetFileName(file);
            destPath = Path.Combine(keySoundPath, fileName);
            File.Copy(file, destPath);
            KeySound keySound = Instantiate(keysound_prefab, transform, false).GetComponent<KeySound>();
            keySound.Toggle.group = toggleGroup;
            AssetDatabase.Refresh();
            // keySound.clip
            keySound.audioSource.clip = Resources.Load<AudioClip>(destPath);
            keySound.KeysoundName = fileName;
            keySound.PlayBTN.onClick.AddListener(keySound.audioSource.Play);

        }

        //TODO 확장자 명 빼는 작업 해야함.
    }

}
