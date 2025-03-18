using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeySound : MonoBehaviour
{
    [SerializeField] private Button play_btn;
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI keysound_name;
    public AudioSource audioSource;
    public Toggle Toggle
    { get { return toggle; } set { toggle = value; } }
    public string KeysoundName
    { get { return keysound_name.text; } set { keysound_name.text = value; } }
    public Button PlayBTN
    { get { return play_btn; } set { play_btn = value; } }

    private void Start()
    {
        // keysound_name.text = clip.name;
    }
}
