using Photon.Pun;
using System.Collections.Generic;
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
}
