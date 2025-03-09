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
    [SerializeField] private Button cancle;

    public Dictionary<Enums.Details, string> popUpInfo = new Dictionary<Enums.Details, string>();

    private void Awake()
    {
        check.onClick.AddListener(CheckBtnOff);
        check.onClick.AddListener(PopupOff);

        cancle.onClick.AddListener(CancleBtnOff);
        cancle.onClick.AddListener(PopupOff);

        for (int i = 0; i < detailInfos.Count; i++)
        {
            popUpInfo[Enums.Details.SAVEPATHCHOICE + i] = detailInfos[i];
        }
    }

    public void PopUpOpen(Enums.Details details)
    {
        switch (details)
        {
            case Enums.Details.SAVEPATHCHOICE:
            case Enums.Details.FILESAVEFAIL:
            case Enums.Details.NONEPROJECTNAME:
            case Enums.Details.NONEARTIST:
            case Enums.Details.NONEBPM:
            case Enums.Details.NONEBGM:
            case Enums.Details.NONETHUMBNAIL:
            case Enums.Details.FILELOADFAIL:
            case Enums.Details.PATHSETERROR:
                detail_tmp.text = popUpInfo[details];
                CheckBtnOn();
                break;
            case Enums.Details.SAVEWARNING:
                detail_tmp.text = popUpInfo[details];
                break;
        }
        popupObj.SetActive(true);
    }

    private void CheckBtnOn()
    {
        check.gameObject.SetActive(true);
    }
    private void CancleBtnOn()
    {
        cancle.gameObject.SetActive(true);
    }
    private void CheckBtnOff()
    {
        check.gameObject.SetActive(false);
    }
    private void CancleBtnOff()
    {
        cancle.gameObject.SetActive(false);
    }
    private void PopupOff()
    {
        popupObj.SetActive(false);
    }
}
