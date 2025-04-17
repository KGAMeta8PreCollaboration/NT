using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
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
    public Woofer[] woofers { get; private set; }
    public List<NoteRail> TopNoteRails { get; private set; }

    private void Awake()
    {
        SpawnPointPlayer = TransformUtil.FindDeepChildComponent<Transform>(transform, "PlayerSpawnPoint");
        NoteManager = TransformUtil.FindDeepChildComponent<NoteManager>(transform, "NoteManager");
        NoteGenerator = TransformUtil.FindDeepChildComponent<NoteGenerator>(transform, "NoteManager");
        ScoreManager = TransformUtil.FindDeepChildComponent<ScoreManager>(transform, "NoteManager");

        TopNoteRails = new List<NoteRail>();

        foreach (NoteRail noteRail in NoteManager.noteRails)
        {
            if (noteRail is TopNoteRail)
            {
                TopNoteRails.Add(noteRail);
            }
        }

        woofers = new Woofer[4];
        for (int i = 0; i < 4; i++)
        {
            woofers[i] = TransformUtil.FindDeepChildComponent<Woofer>(NoteManager.noteRails[i].transform, "Woofer");
        }

    }

    private void Start()
    {
        switch (playerKey)
        {
            case PlayerKey.Player1:
                NoteManager.playMode = Enums.PlayMode.Player1;
                break;
            case PlayerKey.Player2:
                NoteManager.playMode = Enums.PlayMode.Player2;
                break;
            default:
                Debug.Log("Unknown player key");
                break;
        }
    }

    public void SetPlayerModuleData(List<LoadedNoteData> playerSongData)
    {
        NoteGenerator.Init(playerSongData);
        //NoteGenerator.Init(NoteGenerator.loadedNotes); //테스트를 위한 임시 메서드
        GameManager.Instance.noteGenerators[(int)playerKey] = NoteGenerator;
        GameManager.Instance.noteManagers[(int)playerKey] = NoteManager;
        if (PhotonNetwork.LocalPlayer.NickName == playerKey.ToString())
        {
            owner = PhotonNetwork.LocalPlayer.NickName;
            GameManager.Instance.PhotonManager.SpawnPlayer("Multi/GamePlayer", SpawnPointPlayer);
        }
    }
}
