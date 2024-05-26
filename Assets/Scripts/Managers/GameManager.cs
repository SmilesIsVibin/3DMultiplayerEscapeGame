using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public GameObject loadingScreen;
    public GameObject winScreen;
    public GameObject winButton;
    public Transform[] playerPositions;
    public static PhotonView gmPV;
    public GameObject[] players;
    private void Awake()
    {
        Instance = this;
        loadingScreen.SetActive(true);
        winScreen.SetActive(false);
        gmPV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gmPV.RPC("RPC_SetUpPlayer", RpcTarget.AllBufferedViaServer);
        }
    }

    void Update()
    {

    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
        base.OnLeftRoom();
    }

    IEnumerator SetUpPlayer()
    {
        int attempts = 0;
        do
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            attempts++;
            yield return new WaitForSeconds(0.25f);
        } while ((players.Length < PhotonNetwork.PlayerList.Length) && (attempts < 4));
        Debug.Log(PhotonNetwork.PlayerList.Length);

        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = playerPositions[i].transform.position;
            players[i].GetComponent<PlayerController>().isActive = true;
        }
        loadingScreen.SetActive(false);
    }

    [PunRPC]
    public void RPC_SetUpPlayer()
    {
        StartCoroutine("SetUpPlayer");
    }

    public void WinLevel()
    {
        if (gmPV.IsMine)
        {
            gmPV.RPC(nameof(RPC_WinLevel), RpcTarget.AllBufferedViaServer);
        }
    }

    [PunRPC]
    public void RPC_WinLevel()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerController>().isActive = false;
        }
        SetupWinScreen();
    }

    public void LeaveGame()
    {
        gmPV.RPC(nameof(RPC_LeaveGame), RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void RPC_LeaveGame()
    {
        GameObject _go = GameObject.FindGameObjectWithTag("RoomManager");
        Destroy(_go);
        PhotonNetwork.LoadLevel(0);
        PhotonNetwork.LeaveRoom();
    }

    public void SetupWinScreen()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            winButton.SetActive(true);
        }
        else
        {
            winButton.SetActive(false);
        }

        winScreen.SetActive(true);
    }
}

