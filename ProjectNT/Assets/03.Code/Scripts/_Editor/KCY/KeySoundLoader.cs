using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class KeySoundLoader : MonoBehaviour
{
    [SerializeField] private GameObject keysound_prefab;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Transform instancingTrans;

    private List<string> fileNameList = new List<string>();
    private Toggle firstElem_toggle;
    private string keySoundPath;

    public void LoadKeySound(string path, string folderName)
    {
        keySoundPath = Path.Combine(EditorDataManager.Instance.ProjectData.m_Path, "KeySounds", folderName);
        if (Directory.Exists(keySoundPath))
        {
            Directory.Delete(keySoundPath, true);
        }
        Directory.CreateDirectory(keySoundPath);
        string[] files = null;
        try
        {
            files = Directory.GetFiles(path);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
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
            fileNameList.Add(Path.GetFileName(file));
            destPath = Path.Combine(keySoundPath, fileName);
            File.Copy(file, destPath);
        }
        StartCoroutine(InstantiateKeySound());
    }

    // 1따봉 드립니다 :)
    private IEnumerator InstantiateKeySound()
    {
        yield return null;

        string filePath;
        AudioClip clip;
        foreach (string file in fileNameList)
        {
            KeySound keySound = Instantiate(keysound_prefab, instancingTrans, false).GetComponent<KeySound>();
            keySound.Toggle.group = toggleGroup;

            filePath = Path.Combine(keySoundPath, file);
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV);
            if (firstElem_toggle == null)
            {
                firstElem_toggle = keySound.Toggle;
            }
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error loading audio clip: {request.error}");
                continue;
            }
            clip = DownloadHandlerAudioClip.GetContent(request);
            clip.name = file;
            keySound.audioSource.clip = clip;
            keySound.KeysoundName = file;
            keySound.PlayBTN.onClick.AddListener(keySound.audioSource.Play);

            if (request.isDone)
            {
                firstElem_toggle.onValueChanged?.Invoke(true);
            }
        }
        gameObject.SetActive(false);
    }
}
