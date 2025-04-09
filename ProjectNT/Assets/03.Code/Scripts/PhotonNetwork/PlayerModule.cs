using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerKey
{
    Player1, Player2
}

public class PlayerModule : MonoBehaviour
{
    public string owner;
    public PlayerKey playerKey;
    public Transform SpawnPointPlayer { get; private set; }
    public NoteManager NoteManager { get; private set; }
    public NoteGenerator NoteGenerator { get; private set; }
    public ScoreManager ScoreManager { get; private set; }

    private void Awake()
    {
        SpawnPointPlayer = TransformUtil.FindDeepChildComponent<Transform>(transform, "PlayerSpawnPoint");
        NoteManager = TransformUtil.FindDeepChildComponent<NoteManager>(transform, "NoteManager");
        NoteGenerator = TransformUtil.FindDeepChildComponent<NoteGenerator>(transform, "NoteManager");
        ScoreManager = TransformUtil.FindDeepChildComponent<ScoreManager>(transform, "NoteManager");
    }

    public void SetPlayerModuleData(List<LoadedNoteData> playerSongData)
    {
        //NoteGenerator.Init(playerSongData);
        NoteGenerator.Init(NoteGenerator.loadedNotes); //테스트를 위한 임시 메서드
        if (PhotonNetwork.LocalPlayer.NickName == playerKey.ToString())
        {
            owner = PhotonNetwork.LocalPlayer.NickName;
            GameManager.Instance.PhotonManager.SpawnPlayer("Multi/GamePlayer", SpawnPointPlayer);
        }
    }
}
