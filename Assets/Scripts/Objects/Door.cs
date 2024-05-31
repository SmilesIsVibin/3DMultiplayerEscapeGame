using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Door : MonoBehaviour
{
    [SerializeField] public List<Button> buttonList = new List<Button>();
    [SerializeField] public GameObject doorObject;
    [SerializeField] public PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        foreach(Button b in buttonList)
        {
            b.isActivated = false;
            b.isInteracting = false;
        }
        doorObject.SetActive(true);
    }

    public void CheckButtonStates()
    {
        pv.RPC(nameof(RPC_CheckButtonStates), RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void RPC_CheckButtonStates()
    {
        int count = 0;
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i].isActivated)
            {
                count++;
            }
        }

        if (count >= buttonList.Count)
        {
            pv.RPC(nameof(DoorOpen), RpcTarget.AllBufferedViaServer);
        }
        else
        {
            Debug.Log("Some Buttons Still Need to be activated");
        }
    }

    [PunRPC]
    public void DoorOpen()
    {
        doorObject.SetActive(false);
    }
}
