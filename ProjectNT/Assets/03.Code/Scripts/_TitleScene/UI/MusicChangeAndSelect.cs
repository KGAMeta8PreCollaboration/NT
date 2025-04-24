using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicChangeAndSelect : MonoBehaviour
{
    [SerializeField]
    private TitleSound tilteSound;
    [Header("인스펙터 할당")]
    public GamePlayUI parentGamePlayUI;

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
        if (musicImage) musicImage.sprite = data.musicAlbumArtSprite; 
        if (musicArtistText) musicArtistText.text = data.musicArtist;
        if (musicNameText) musicNameText.text = data.musicName;
        if (tilteSound) tilteSound.SetMusicClip(data.musicClip);
        
        Difficulty difficulty = Difficulty.None;
        if (parentGamePlayUI.gameType == UIGameType.Single)
        {
            if ((data.modeDiff & Enums.ModeDiff.SOLO_EASY) != 0)
                difficulty |= Difficulty.Easy;
            if ((data.modeDiff & Enums.ModeDiff.SOLO_NORMAL) != 0)
                difficulty |= Difficulty.Normal;
            if ((data.modeDiff & Enums.ModeDiff.SOLO_HARD) != 0)
                difficulty |= Difficulty.Hard;
            if ((data.modeDiff & Enums.ModeDiff.SOLO_EXTREAM) != 0)
                difficulty |= Difficulty.SuperHard;
        }
        else
        {
            if ((data.modeDiff & Enums.ModeDiff.DUO1_EASY) != 0 && 
                (data.modeDiff & Enums.ModeDiff.DUO2_EASY) != 0)
                difficulty |= Difficulty.Easy;
            if ((data.modeDiff & Enums.ModeDiff.DUO1_NORMAL) != 0 && 
                (data.modeDiff & Enums.ModeDiff.DUO2_NORMAL) != 0)
                difficulty |= Difficulty.Normal;
            if ((data.modeDiff & Enums.ModeDiff.DUO1_HARD) != 0 && 
                (data.modeDiff & Enums.ModeDiff.DUO2_HARD) != 0)
                difficulty |= Difficulty.Hard;
            if ((data.modeDiff & Enums.ModeDiff.DUO1_EXTREAM) != 0 && 
                (data.modeDiff & Enums.ModeDiff.DUO2_EXTREAM) != 0)
                difficulty |= Difficulty.SuperHard;
        }
        parentGamePlayUI.SetToggleInteractable(difficulty);
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