using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TitleManager : MonoBehaviour
{ 
    private BlurEffectManager blurEffectManager;
    [SerializeField]
    private TilteSound tilteSound;
    private bool isComplete = false;//페이드아웃 효과 끝났는지 확인
    private bool isUIActive = false;//UI가 활성화 상태인지 확인
    public bool isMultiPlaye = false;//멀티플레이 상태인지 확인

    public bool IsComplete { get { return isComplete; } }
    public bool IsUIActive { get { return isUIActive; } }
    //이거로 멀티/싱글 넘어갈때 반대쪽 컨트롤러 오브젝트 잠시 Off시키는게 나을듯

    private void Awake()
    {
        blurEffectManager = FindObjectOfType<BlurEffectManager>();
        blurEffectManager.ResetTitle();
        //서버생기면 여기서 실행후 끝날때 아래함수 실행
        blurEffectManager.FadeOutStart(FadeOutEnd);
        tilteSound.SetBackgroundSound(true);
    }

    private void FadeOutEnd() 
    {
        isComplete = true;
        Debug.Log("페이드 아웃 완료");
    }

    public void SetUIActive(bool active)//UI켜짐/UI꺼짐
    {
        isUIActive = active;
    }
}
