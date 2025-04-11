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

    public Woofer GetPlayerWoofer(int playerModuleIndex, int wooferIndex)
    {
        if (playerModuleIndex >= 0 && playerModuleIndex < playerModules.Length)
        {
            PlayerModule playerModule = playerModules[playerModuleIndex];
            if (wooferIndex >= 0 && wooferIndex < playerModule.woofers.Length)
            {
                return playerModule.woofers[wooferIndex];
            }
        }
        return null;
    }
}
