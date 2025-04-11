using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerModuleManager : MonoBehaviour
{
    public PlayerModule[] playerModules;

    public void SetPlayerModuleData(List<LoadedNoteData> player1SongData, List<LoadedNoteData> player2SongData)
    {
        foreach (PlayerModule playerModule in playerModules)
        {
            List<LoadedNoteData> songData = PhotonNetwork.LocalPlayer.NickName == "Player1" ? player1SongData : player2SongData;
            playerModule.SetPlayerModuleData(songData);
        }
    }

    public Woofer GetPlayerWoofer(int wooferIndex, string nickname)
    {
        PlayerModule module = null;

        foreach (PlayerModule playerModule in playerModules)
        {
            if (playerModule.owner.Equals(nickname)) module = playerModule;
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
            if (playerModule.owner.Equals(nickname)) module = playerModule;
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
