using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TitleManager : MonoBehaviour
{
    public static TitleManager instance;

    [Header("UI")]
    public GameObject singlePlayUI;
    public GameObject rankingBoardUI;
    public GameObject gameSettingUI;
    public GameObject multiPlayUI;

    [Header("Music Source")]
    [Header("게임 음악 샘플 소스")]
    public AudioSource gameMusicAudioSource;
    [Header("배경 음악 소스")]
    public AudioSource backgroundAudioSource;

    [SerializeField]
    private BlurEffectManager blurEffectManager;
    private bool isComplete = false;//페이드아웃 효과 끝났는지 확인
    private bool isUIActive = false;//UI가 활성화 상태인지 확인
    private bool isMultiPlaye = false;//멀티플레이 상태인지 확인

    private GameObject curUI;

    public bool IsComplete { get { return isComplete; } }
    public bool IsUIActive { get { return isUIActive; } }
    //이거로 멀티/싱글 넘어갈때 반대쪽 컨트롤러 오브젝트 잠시 Off시키는게 나을듯
    public bool IsMultiPlaye { get { return isUIActive; } }

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

    private void Start()
    {
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();//배경음악 재생

        blurEffectManager.ResetTitle();
        //서버생기면 여기서 실행후 끝날때 아래함수 실행
        blurEffectManager.FadeOutStart(FadeOutEnd);
    }

    private void Update()
    {
        //if (backgroundAudioSource.isPlaying == true)
        //{
        //    Debug.Log("배경음악 나오는 중");
        //}
        //else
        //{
        //    Debug.Log("배경음악 꺼져있음");
        //}
        //if (gameMusicAudioSource.isPlaying == true)
        //{
        //    Debug.Log("게임 샘플 음악 나오는 중");
        //}
        //else
        //{
        //    Debug.Log("게임 샘플 음악 꺼져있음");
        //}
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
                multiPlayUI.SetActive(true);
                PhotonNetwork.ConnectUsingSettings(); // Photon 서버 연결
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
        if (isMultiPlaye)
        {
            isMultiPlaye = false;
            //멀티플레이에서 나감
        }
        else
        {
            curUI.SetActive(false);
        }
        isUIActive = false;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (gameMusicAudioSource.isPlaying)//현재 재생중이면
        {
            gameMusicAudioSource.Stop();//중지시키고
        }
        gameMusicAudioSource.clip = clip;
        gameMusicAudioSource.Play();//받은 노래 다시시작
    }

    public void StopMusic()
    {
        if (gameMusicAudioSource.isPlaying)
        {
            gameMusicAudioSource.Stop();
            Debug.Log("노래 꺼짐");
        }
    }

    public void MusicLoop(bool musicLoop)
    {
        gameMusicAudioSource.loop = musicLoop;
    }

    public void BackgroundMusicPlay(bool isPlaying)
    {
        if (isPlaying)
        {
            backgroundAudioSource.Play();
        }
        else
        {
            backgroundAudioSource.Stop();
        }
    }
}
