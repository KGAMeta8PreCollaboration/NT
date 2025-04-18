using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditProject : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI projectName_tmp;
    [SerializeField] private TextMeshProUGUI artistName_tmp;
    [SerializeField] private TextMeshProUGUI bpm_tmp;
    [SerializeField] private TextMeshProUGUI beatNum_tmp;
    [SerializeField] private Button edit_btn;
    [SerializeField] private Button keysound_btn;
    [SerializeField] private Toggle easy;
    [SerializeField] private Toggle normal;
    [SerializeField] private Toggle hard;
    [SerializeField] private Toggle extreme;
    [SerializeField] private Toggle solo;
    [SerializeField] private Toggle duo_1;
    [SerializeField] private Toggle duo_2;

}
