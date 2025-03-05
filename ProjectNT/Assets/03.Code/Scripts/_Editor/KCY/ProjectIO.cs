using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectIO : MonoBehaviour
{
    [SerializeField] private GameObject newProjectPrefab;
    [SerializeField] private RectTransform project_Rect;
    [SerializeField] private TextMeshProUGUI pathVisual_TMP;
    [SerializeField] private Button return_BTN;
    [SerializeField] private Button addProejct_BTN;
    [SerializeField] private Button refreah_BTN;
    [SerializeField] private Button delete_BTN;

    private GameObject currenctProject;

}
