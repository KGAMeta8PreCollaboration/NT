using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TitleMusicData", menuName = "ScriptableObjects/TitleMusicData", order = int.MaxValue)]
public class TitleMusicData : ScriptableObject, IComparable<TitleMusicData>//타이틀씬 음악 샘플파일
{
    [Header("음악 이름")]
    public string musicName;
    [Header("음악 아티스트")]
    public string musicArtist;
    [Header("앨범 아트 이미지")]
    public Sprite musicAlbumArtSprite;
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
        musicAlbumArtSprite = Utility.ByteToSprite(projectData.thumbnailData);
        string path = Path.Combine(Application.persistentDataPath, "Projects", projectData.projectName);
        musicClip = GameManager.Instance.projectToLoadedData.GetBgmAudioClip(path, "BGM_Highlight.wav");
        projectName = projectData.projectName;
        modeDiff = projectData.modeDiff;
    }

    public int CompareTo(TitleMusicData other)
    {
        return string.Compare(musicName, other.musicName, StringComparison.Ordinal);
    }
}
