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
                songData = player1SongData;
                print($"{playerModule.name}의 노래 데이터: player1SongData 고름");
            }
            else if (playerModule.playerKey == PlayerKey.Player2)
            {
                songData = player2SongData;
                print($"{playerModule.name}의 노래 데이터: player2SongData 고름");
            }

            playerModule.SetPlayerModuleData(songData);
        }

        print($"플레이어1,2 의 로드된 노트데이터가 같은지?: {playerModules[0].NoteGenerator.loadedNotes.Equals(playerModules[1].NoteGenerator.loadedNotes)}");
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

        print("플레이어 Woofer를 찾을 수 없음!");
        return null;
    }

    public int GetWooferIndex(Woofer woofer, string nickname)
    {
        PlayerModule module = null;

        foreach (PlayerModule playerModule in playerModules)
        {
            if (playerModule.playerKey.ToString().Equals(nickname)) module = playerModule;
        }

        if (module != null)
        {
            for (int i = 0; i < module.woofers.Length; i++)
            {
                if (module.woofers[i] == woofer)
                    return i;
            }
        }

        Debug.LogWarning($"[WooferNetworkSync] 우퍼 인덱스를 찾을 수 없음! 닉네임: {nickname}");
        return -1;
    }
}
