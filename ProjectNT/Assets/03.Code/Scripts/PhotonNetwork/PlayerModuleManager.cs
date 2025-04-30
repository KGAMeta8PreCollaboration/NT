using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerModuleManager : MonoBehaviour
{
    public PlayerModule[] playerModules;

    public void SetPlayerModuleData(List<LoadedNoteData> player1SongData, List<LoadedNoteData> player2SongData)
    {
        GameManager.Instance.noteManagers = new NoteManager[playerModules.Length];
        GameManager.Instance.noteGenerators = new NoteGenerator[playerModules.Length];

        foreach (PlayerModule playerModule in playerModules)
        {
            List<LoadedNoteData> songData = null;
            if (playerModule.playerKey == PlayerKey.Player1)
            {
                playerModule.NoteManager.topNoteTag = PlayerKey.Player1.ToString();

                songData = player1SongData;
            }
            else if (playerModule.playerKey == PlayerKey.Player2)
            {
                playerModule.NoteManager.topNoteTag = PlayerKey.Player2.ToString();

                songData = player2SongData;
            }

            playerModule.SetPlayerModuleData(songData);
        }

    }


    public Woofer GetPlayerWoofer(int wooferIndex, string nickname)
    {
        PlayerModule module = null;

        foreach (PlayerModule playerModule in playerModules)
        {
            if (playerModule.playerKey.ToString().Equals(nickname)) module = playerModule;
        }

        if (module != null)
        {
            return module.woofers[wooferIndex];
        }

        return null;
    }

    public int GetWooferIndex(Woofer woofer, string nickname)
    {
        PlayerModule module = null;

        foreach (PlayerModule playerModule in playerModules)
        {
            if (playerModule.playerKey.ToString().Equals(nickname))
            {
                module = playerModule;
            }
        }

        if (module != null)
        {
            for (int i = 0; i < module.woofers.Length; i++)
            {
                if (module.woofers[i] == woofer)
                {
                    return i;
                }
            }
        }
        else
        {
            Debug.LogError("설마 여길 들어올 리는 없겠지");
        }

        Debug.LogWarning($"[WooferNetworkSync] 우퍼 인덱스를 찾을 수 없음! 닉네임: {nickname}, Woofer: {woofer}");
        return -1;
    }

    public TopNote GetPlayerTopNote(int index, string nickname)
    {
        foreach (PlayerModule module in playerModules)
        {
            if (module.playerKey.ToString() == nickname)
            {
                LinkedList<Note> noteList = module.TopNoteRails[index].GetNoteList();
                if (noteList.First != null && noteList.First.Value is TopNote topNote)
                    return topNote;
            }
        }

        Debug.LogWarning($"[MultiGameController] TopNote를 찾을 수 없습니다. 인덱스: {index}, 닉네임: {nickname}");
        return null;
    }

    public int GetTopNoteIndex(TopNote topNote, string nickname)
    {
        foreach (PlayerModule module in playerModules)
        {
            if (module.playerKey.ToString().Equals(nickname))
            {
                for (int i = 0; i < module.TopNoteRails.Count; i++)
                {
                    LinkedList<Note> noteList = module.TopNoteRails[i].GetNoteList();
                    foreach (Note note in noteList)
                    {
                        if (note == topNote)
                            return i;
                    }
                }
            }
        }

        Debug.LogWarning($"[MultiGameController] TopNote 인덱스를 찾을 수 없습니다. 닉네임: {nickname}");
        return -1;
    }

    public PlayerModule GetPlayerModuleByNick(string nickname)
    {
        foreach (PlayerModule module in playerModules)
        {
            if (module.playerKey.ToString() == nickname)
            {
                return module;
            }
        }

        Debug.LogError($"플레이어 모듈을 찾을 수 없음: {nickname}");
        return null;
    }
}
