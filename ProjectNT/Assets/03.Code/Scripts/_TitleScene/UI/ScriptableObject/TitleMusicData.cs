using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TitleMusicData", menuName = "ScriptableObjects/TitleMusicData", order = int.MaxValue)]
public class TitleMusicData : ScriptableObject//타이틀씬 음악 샘플파일
{
    [Header("음악 이름")]
    public string musicName;
    [Header("음악 설명")]
    public string musicDescription;
    [Header("앨범 아트 이미지")]
    public Sprite musicAlbumArtSprit;
    [Header("음악 파일")]
    public AudioClip musicClip;
}
