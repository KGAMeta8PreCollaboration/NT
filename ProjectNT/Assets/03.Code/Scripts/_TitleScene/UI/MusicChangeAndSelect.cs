using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
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
        tilteSound = FindObjectOfType<TitleSound>();
        currentMusicNode = musicList.First;
        SetInternalData(currentMusicNode.Value);
    }

    private void OnEnable()
    {
        if (tilteSound.backgroundAudioSource != null)
            tilteSound.SetBackgroundSound(false);
    }

    private void OnDisable()
    {
        tilteSound.StopGameSound();
        if (tilteSound.backgroundAudioSource != null)
            tilteSound.SetBackgroundSound(true);
    }

    private void SetInternalData(TitleMusicData data)
    {
        Debug.Log($"{data.musicName}");
        musicImage.sprite = data.musicAlbumArtSprit;
        musicNameText.text = data.musicName;
        // musicArtistText.text = data.musicArtist;
        tilteSound.PlayGameSound(data.musicClip);
    }

    public void ReplayMusic()
    {
        // Debug.Log("Music Replay 노래 처음부터 시작");
        tilteSound.PlayGameSound(currentMusicNode.Value.musicClip);
    }

    private void PrintCurrentMusicInfo()
    {
        print(currentMusicNode.Value.musicName);
        print(currentMusicNode.Value.musicArtist);
        print(currentMusicNode.Value.musicClip);
        
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
        action?.Invoke();
        // PrintCurrentMusicInfo();
    }

}