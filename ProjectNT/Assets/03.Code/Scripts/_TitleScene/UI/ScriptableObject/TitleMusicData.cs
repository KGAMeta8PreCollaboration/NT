using System.IO;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TitleMusicData", menuName = "ScriptableObjects/TitleMusicData", order = int.MaxValue)]
public class TitleMusicData : ScriptableObject//타이틀씬 음악 샘플파일
{
    [Header("음악 이름")]
    public string musicName;
    [Header("음악 아티스트")]
    public string musicArtist;
    [Header("앨범 아트 이미지")]
    public Sprite musicAlbumArtSprit;
    [Header("음악 파일")]
    public AudioClip musicClip;
    [Header("프로젝트 이름")]
    public string projectName;
    [Header("모드")]
    public Enums.ModeDiff modeDiff;

    public void Init(ProjectData projectData)
    {
        musicName = projectData.projectName;
        musicArtist = projectData.artistName;
        musicAlbumArtSprit = Utility.ByteToSprite(projectData.thumbnailData);
        string path = Path.Combine(Application.persistentDataPath, "Projects", projectData.projectName);
        musicClip = GameManager.Instance.projectToLoadedData.GetBgmAudioClip(path, "BGM_Highlight.wav");
        Debug.Log("Init TitleMusicData musicClip 이름 : " + musicClip.name);
        projectName = projectData.projectName;
        modeDiff = projectData.modeDiff;
    }
    
    public void PrintInfo()
    {
        Debug.Log($"Music Name: {musicName}, Artist: {musicArtist}, Project Name: {projectName}, Mode: {modeDiff}, Clip Name: {musicClip?.name}");
    }
}
