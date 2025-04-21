using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [TextArea]
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
            popUpInfo[Enums.Details.SavePathChoice + i] = detailInfos[i];
        }
    }

    public void PopUpOpen(Enums.Details details, Action action = null)
    {
        popupObj.SetActive(true);
        switch (details)
        {
            case Enums.Details.SavePathChoice:
            case Enums.Details.FileSaveFail:
            case Enums.Details.NoneProjectName:
            case Enums.Details.NoneArtist:
            case Enums.Details.NoneBpm:
            case Enums.Details.NoneBgm:
            case Enums.Details.NoneThumbnail:
            case Enums.Details.NoneKeySoundFolder:
            case Enums.Details.FileLoadFail:
            case Enums.Details.PathSetError:
            case Enums.Details.SaveFolderExist:
            case Enums.Details.ThemeAlreadyExist:
            case Enums.Details.LoadImageFail:
            case Enums.Details.MakeProjectComplete:
            case Enums.Details.ChangeProjectInfoComplete:
            case Enums.Details.FileDetectFail:
            case Enums.Details.NoneBeatNum:
                detail_tmp.text = popUpInfo[details];
                CheckBtnOn();
                break;
            case Enums.Details.DeleteProjectCheck:
            case Enums.Details.EditorQuit:
                detail_tmp.text = popUpInfo[details];
                CheckBtnOn();
                CancleBtnOn();
                temp = action;
                break;
            case Enums.Details.SaveWarning:

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
