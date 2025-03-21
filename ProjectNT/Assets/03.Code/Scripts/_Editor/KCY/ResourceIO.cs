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
    [SerializeField] private Button save_btn;
    [SerializeField] private Button exit_btn;
    [SerializeField] private Button copy_btn;

    private void Awake()
    {
        keySoundLoader.LoadKeySound();

        phase2_inputfield.onValueChanged.AddListener((word) => phase2_inputfield.text = Regex.Replace(word, @"[^0-9]", ""));

        phase3_inputfield.onValueChanged.AddListener((word) => phase3_inputfield.text = Regex.Replace(word, @"[^0-9]", ""));

        phase2_inputfield.onEndEdit.AddListener((x) => EditorDataManager.Instance.CurBeatMap.songData.phase2 = int.Parse(phase2_inputfield.text));

        phase3_inputfield.onEndEdit.AddListener((x) => EditorDataManager.Instance.CurBeatMap.songData.phase3 = int.Parse(phase3_inputfield.text));

        songName_tmp.text = EditorDataManager.Instance.ProjectData.projectName;
        thumbnail_img.sprite = EditorDataManager.Instance.thumbnail_sprite;

        save_btn.onClick.AddListener(EditorDataManager.Instance.SaveBeatMap);
        exit_btn.onClick.AddListener(GoToTitle);
    }

    private void GoToTitle()
    {
        EditorDataManager.Instance.beatMapLoadAction = null;
        EditorLoadScene.SceneLoad("EditorPathScene");
    }

    private void Start()
    {
        EditorDataManager.Instance.phaseDataAction?.Invoke();
    }
    private void OnEnable()
    {
        EditorDataManager.Instance.phaseDataAction += GetPhaseData;
    }
    private void OnDisable()
    {
        EditorDataManager.Instance.phaseDataAction -= GetPhaseData;
    }


    private void GetPhaseData()
    {
        phase2_inputfield.text = EditorDataManager.Instance.CurBeatMap.songData.phase2.ToString();
        phase3_inputfield.text = EditorDataManager.Instance.CurBeatMap.songData.phase3.ToString();
    }
}
