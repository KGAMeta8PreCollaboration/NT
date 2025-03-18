using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ResourceIO : MonoBehaviour
{
    [SerializeField] private KeySoundLoader keySoundLoader;
    [SerializeField] private TMP_InputField phase2_inputfield;
    [SerializeField] private TMP_InputField phase3_inputfield;
    [SerializeField] private TextMeshProUGUI songName_tmp;
    [SerializeField] private Image thumbnail_img;
    private bool isSaved = true;
    public bool IsSaved
    { get { return isSaved; } set { isSaved = value; } }

    private void Awake()
    {
        keySoundLoader.LoadKeySound();

        phase2_inputfield.onValueChanged.AddListener((word) => phase2_inputfield.text = Regex.Replace(word, @"[0-9]", ""));
        phase3_inputfield.onValueChanged.AddListener((word) => phase3_inputfield.text = Regex.Replace(word, @"[0-9]", ""));
        songName_tmp.text = EditorDataManager.Instance.ProjectData.projectName;
        thumbnail_img.sprite = EditorDataManager.Instance.thumbnail_sprite;
        SaveTracker();
    }

    private void SaveTracker()
    {
        phase2_inputfield.onValueChanged.AddListener(x => IsSaved = false);
        phase3_inputfield.onValueChanged.AddListener(x => IsSaved = false);
    }

}
