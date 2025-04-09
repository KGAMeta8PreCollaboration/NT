using Photon.Pun;
using UnityEngine;

public class WooferNetworkSync : MonoBehaviourPun
{
    [PunRPC]
    private void RPC_Hit(Woofer woofer)
    {
        woofer.Hit();
    }

    [PunRPC]
    private void RPC_Hold(Woofer woofer)
    {
        woofer.Hold();
    }

    [PunRPC]
    private void RPC_Release(Woofer woofer)
    {
        woofer.ReleaseLongNote();
    }

    // 로컬에서 명령 보낼 때 호출
    public void SendHit(Woofer woofer)
    {
        photonView.RPC(nameof(RPC_Hit), RpcTarget.All, woofer);
    }

    public void SendHold(Woofer woofer)
    {
        photonView.RPC(nameof(RPC_Hold), RpcTarget.All, woofer);
    }

    public void SendRelease(Woofer woofer)
    {
        photonView.RPC(nameof(RPC_Release), RpcTarget.All, woofer);
    }
}
