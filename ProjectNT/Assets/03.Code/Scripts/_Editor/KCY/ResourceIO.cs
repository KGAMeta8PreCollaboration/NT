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
    private const string FloatNumericRegex = @"[^0-9.]";
    [SerializeField] private KeySoundLoader phase1KeysoundLoader;
    [SerializeField] private KeySoundLoader phase2KeysoundLoader;
    [SerializeField] private KeySoundLoader phase3KeysoundLoader;
    [SerializeField] private TMP_InputField phase2_inputfield;
    [SerializeField] private TMP_InputField phase3_inputfield;
    [SerializeField] private TextMeshProUGUI songName_tmp;
    [SerializeField] private Image thumbnail_img;
    [SerializeField] private Button save_btn;
    [SerializeField] private Button exit_btn;
    [SerializeField] private Button copy_btn;

    private void Awake()
    {
        phase1KeysoundLoader.LoadKeySound(EditorDataManager.Instance.ProjectData.phase1KeysoundPath, "Phase1");
        phase2KeysoundLoader.LoadKeySound(EditorDataManager.Instance.ProjectData.phase2KeysoundPath, "Phase2");
        phase3KeysoundLoader.LoadKeySound(EditorDataManager.Instance.ProjectData.phase3KeysoundPath, "Phase3");

        phase2_inputfield.onValueChanged.AddListener((word) => phase2_inputfield.text = Regex.Replace(word, FloatNumericRegex, ""));

        phase3_inputfield.onValueChanged.AddListener((word) => phase3_inputfield.text = Regex.Replace(word, FloatNumericRegex, ""));

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
        // phase2_inputfield.text = EditorDataManager.Instance.CurBeatMap.songData.phase2.ToString();
        // phase3_inputfield.text = EditorDataManager.Instance.CurBeatMap.songData.phase3.ToString();
    }
}
