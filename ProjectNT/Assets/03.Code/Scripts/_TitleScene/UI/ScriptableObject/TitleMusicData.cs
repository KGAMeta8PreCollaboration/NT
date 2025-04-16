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
    [Header("음악 파일")]
    public string beatMapDataPath;
    [Header("프로젝트 이름")]
    public string projectName;
    [Header("모드")]
    public Enums.ModeDiff modeDiff;
    
}
