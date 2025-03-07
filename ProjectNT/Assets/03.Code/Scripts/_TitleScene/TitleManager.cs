using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public static TitleManager instance;

    public GameObject singlePlayUI;
    public GameObject multiPlayUI;
    public GameObject rankingBoardUI;
    public GameObject gameSettingUI;

    public BlurEffectManager blurEffectManager;
    public bool isComplete = false;//페이드아웃 효과 끝났는지 확인
    public bool isUIActive = false;//UI가 활성화 상태인지 확인

    private GameObject curUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        blurEffectManager.ResetTitle();
        //서버생기면 여기서 실행후 끝날때 아래함수 실행
        blurEffectManager.FadeOutStart(FadeOutEnd);
    }

    private void FadeOutEnd()
    {
        isComplete = true;
        Debug.Log("페이드 아웃 완료");
    }

    public void OpenUI(TitleUIName uiName)
    {
        if (!isComplete || isUIActive) return;
        switch (uiName)
        {
            case TitleUIName.SinglePlay:
                Debug.Log($"{uiName} UI 활성화");
                singlePlayUI.SetActive(true);
                curUI = singlePlayUI;
                break;
            case TitleUIName.MultiPlay:
                Debug.Log($"{uiName} UI 활성화");
                multiPlayUI.SetActive(true);
                curUI = multiPlayUI;
                break;
            case TitleUIName.RankingBoard:
                Debug.Log($"{uiName} UI 활성화");
                rankingBoardUI.SetActive(true);
                curUI = rankingBoardUI;
                break;
            case TitleUIName.GameSetting:
                Debug.Log($"{uiName} UI 활성화");
                gameSettingUI.SetActive(true);
                curUI = gameSettingUI;
                break;
            default:
                Debug.LogWarning($"{uiName}, 맞는 이름 없음");
                break;
        }
        isUIActive = true;
    }

    public void CloseUI()
    {
        Debug.Log("Close버튼 클릭 현재 UI 닫기");
        curUI.SetActive(false);
        isUIActive = false;
    }
}
