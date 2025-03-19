using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{

    [SerializeField] private List<string> detailInfos;
    [SerializeField] private TextMeshProUGUI detail_tmp;
    [SerializeField] private GameObject popupObj;
    [SerializeField] private Button check;
    [SerializeField] private TextMeshProUGUI check_tmp;
    [SerializeField] private Button cancle;
    [SerializeField] private Button notSave;
    public Dictionary<Enums.Details, string> popUpInfo = new Dictionary<Enums.Details, string>();
    private Action temp;
    private void Awake()
    {
        check.onClick.AddListener(CheckBtnOff);
        check.onClick.AddListener(PopupOff);
        check.onClick.AddListener(CheckClick);

        cancle.onClick.AddListener(CancleBtnOff);
        cancle.onClick.AddListener(PopupOff);
        cancle.onClick.AddListener(CancleClick);

        notSave.onClick.AddListener(NotSaveBtnOff);
        notSave.onClick.AddListener(PopupOff);

        for (int i = 0; i < detailInfos.Count; i++)
        {
            popUpInfo[Enums.Details.SAVEPATHCHOICE + i] = detailInfos[i];
        }
    }

    public void PopUpOpen(Enums.Details details, Action action = null)
    {
        popupObj.SetActive(true);
        switch (details)
        {
            case Enums.Details.SAVEPATHCHOICE:
            case Enums.Details.FILESAVEFAIL:
            case Enums.Details.NONEPROJECTNAME:
            case Enums.Details.NONEARTIST:
            case Enums.Details.NONEBPM:
            case Enums.Details.NONEBGM:
            case Enums.Details.NONETHUMBNAIL:
            case Enums.Details.NONEKEYSOUNDFOLDER:
            case Enums.Details.FILELOADFAIL:
            case Enums.Details.PATHSETERROR:
            case Enums.Details.SAVEFOLDEREXIST:
            case Enums.Details.LOADIMGFAIL:
            case Enums.Details.MAKEPROJECTCOMPLETE:
            case Enums.Details.CHANGEPROJECTINFOCOMPLETE:
            case Enums.Details.FILEDETECTIONFAIL:
                detail_tmp.text = popUpInfo[details];
                CheckBtnOn();
                break;
            case Enums.Details.DELETEPROJECTCHECK:
            case Enums.Details.EDITORQUIT:
                detail_tmp.text = popUpInfo[details];
                CheckBtnOn();
                CancleBtnOn();
                temp = action;
                break;
            case Enums.Details.SAVEWARNING:

                break;
            default:
                Debug.LogError("지정되지 않은 케이스입니다.");
                break;
        }
    }

    private void CheckBtnOn()
    {
        check.gameObject.SetActive(true);
    }
    private void CheckBtnOff()
    {
        check.gameObject.SetActive(false);
    }
    private void CheckClick()
    {
        temp?.Invoke();
        temp = null;
    }
    private void CancleBtnOn()
    {
        cancle.gameObject.SetActive(true);
    }

    private void CancleBtnOff()
    {
        cancle.gameObject.SetActive(false);
    }

    private void CancleClick()
    {
        temp = null;
    }
    private void NotSaveBtnOn()
    {
        notSave.gameObject.SetActive(true);
    }
    private void NotSaveBtnOff()
    {
        notSave.gameObject.SetActive(false);
    }
    private void NotSaveClick()
    {

    }
    private void PopupOff()
    {
        popupObj.SetActive(false);
    }

}
