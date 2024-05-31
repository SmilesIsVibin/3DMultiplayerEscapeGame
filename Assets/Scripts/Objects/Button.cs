using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Button : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] public bool isActivated;
    [SerializeField] public bool isInteracting;
    [SerializeField] public bool isPressed;
    [SerializeField] private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            if (other.CompareTag("Player"))
            {
                pv.RPC(nameof(RPC_SetDoorState), RpcTarget.AllBufferedViaServer);
            }
        }
    }

    [PunRPC]
    public void RPC_SetDoorState()
    {
        isActivated = true;
        isPressed = true;
        door.CheckButtonStates();
    }
}
