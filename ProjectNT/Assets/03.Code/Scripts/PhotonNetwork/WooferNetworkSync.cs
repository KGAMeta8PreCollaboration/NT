using Photon.Pun;
using UnityEngine;

public class WooferNetworkSync : MonoBehaviourPun
{
    [PunRPC]
    private void RPC_Hit(int wooferIndex, string nickname)
    {
        Woofer woofer = GameManager.Instance.MultiGameController.GetPlayerWoofer(wooferIndex, nickname);
        woofer.Hit();
    }

    [PunRPC]
    private void RPC_Hold(int wooferIndex, string nickname)
    {
        Woofer woofer = GameManager.Instance.MultiGameController.GetPlayerWoofer(wooferIndex, nickname);
        woofer.Hold();
    }

    [PunRPC]
    private void RPC_Release(int wooferIndex, string nickname)
    {
        Woofer woofer = GameManager.Instance.MultiGameController.GetPlayerWoofer(wooferIndex, nickname);
        woofer.ReleaseLongNote();
    }

    [PunRPC]
    private void RPC_TopNoteHit(int railIndex, string nickname)
    {
        TopNote topNote = GameManager.Instance.MultiGameController.GetPlayerTopNote(railIndex, nickname);
        topNote.Hit();
    }

    // 로컬에서 명령 보낼 때 호출
    public void SendHit(Woofer woofer, string nickname)
    {
        int wooferIndex = GameManager.Instance.MultiGameController.GetWooferIndex(woofer, nickname);
        photonView.RPC(nameof(RPC_Hit), RpcTarget.All, wooferIndex, nickname);
    }

    public void SendHold(Woofer woofer, string nickname)
    {
        int wooferIndex = GameManager.Instance.MultiGameController.GetWooferIndex(woofer, nickname);
        photonView.RPC(nameof(RPC_Hold), RpcTarget.All, wooferIndex, nickname);
    }

    public void SendRelease(Woofer woofer, string nickname)
    {
        int wooferIndex = GameManager.Instance.MultiGameController.GetWooferIndex(woofer, nickname);
        photonView.RPC(nameof(RPC_Release), RpcTarget.All, wooferIndex, nickname);
    }

    public void SendTopNoteHit(TopNote topNote, string nickname)
    {
        int railIndex = GameManager.Instance.MultiGameController.GetTopNoteIndex(topNote, nickname);
        photonView.RPC(nameof(RPC_TopNoteHit), RpcTarget.All, railIndex, nickname);
    }
}
