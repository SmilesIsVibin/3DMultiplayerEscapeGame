using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WinLevelTrigger : MonoBehaviour
{
    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pv.RPC(nameof(WinLevel), RpcTarget.AllBufferedViaServer);
        }
    }

    [PunRPC]
    private void WinLevel()
    {
        GameManager.Instance.WinLevel();
    }
}
