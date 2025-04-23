using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicChangeAndSelect : MonoBehaviour
{
    [SerializeField]
    private TitleSound tilteSound;

    public Image musicImage;
    public TextMeshProUGUI musicNameText;
    public TextMeshProUGUI musicArtistText;

    public Button changeLeftButton;
    public Button changeRightButton;
    private LinkedList<TitleMusicData> musicList;
    public LinkedListNode<TitleMusicData> currentMusicNode { get; private set; }
    public TitleMusicData CurMusicData { get { return currentMusicNode.Value; } }

    public void Init(List<TitleMusicData> titleMusicData)
    {
        musicList = new LinkedList<TitleMusicData>(titleMusicData);
        tilteSound = FindObjectOfType<TitleSound>(true);
        currentMusicNode = musicList.First;
        SetInternalData(currentMusicNode.Value);
    }

    private void OnEnable()
    {
        if (tilteSound?.backgroundAudioSource != null)
            tilteSound.SetBackgroundSound(false);
    }

    private void OnDisable()
    {
        if (tilteSound)
        {
            tilteSound.StopGameSound();
            if (tilteSound.backgroundAudioSource != null)
                tilteSound.SetBackgroundSound(true);
        }
    }

    private void SetInternalData(TitleMusicData data)
    {
        if (musicImage) musicImage.sprite = data.musicAlbumArtSprit; 
        if (musicArtistText) musicArtistText.text = data.musicArtist;
        if (musicNameText) musicNameText.text = data.musicName;
        if (tilteSound) tilteSound.SetMusicClip(data.musicClip);
    }

    public void ReplayMusic()
    {
        // Debug.Log("Music Replay 노래 처음부터 시작");
        tilteSound.PlayGameSound(currentMusicNode.Value.musicClip);
    }

    public void ChangeMusic(string direction, Action action = null)
    {
        // print("MusicChangeAndSelect ChangeMusic : " + direction);
        currentMusicNode = direction switch
        {
            "first" => musicList.First,
            "next" => currentMusicNode.Next ?? musicList.First,
            "previous" => currentMusicNode.Previous ?? musicList.Last,
            _ => throw new ArgumentException("Invalid direction")
        };
        SetInternalData(currentMusicNode.Value);
        tilteSound.PlayGameSound();

        action?.Invoke();
        // PrintCurrentMusicInfo();
    }

}