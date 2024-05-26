using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class ServerManager : MonoBehaviourPunCallbacks
{
    public static ServerManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("You are currently connected to Master Server");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        base.OnConnectedToMaster();
    }

    public void CreateRoom(string roomName)
    {
        if (string.IsNullOrEmpty(roomName))
        {
            return;
        }
        else
        {
            PhotonNetwork.CreateRoom(roomName);
            MenuManager.Instance.OpenMenu("LOADING");
        }
    }

    public void JoinRoom(RoomInfo roomName)
    {
        MenuManager.Instance.OpenMenu("LOADING");
        PhotonNetwork.JoinRoom(roomName.Name);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        MenuManager.Instance.OpenMenu("LOGIN");
        base.OnJoinedLobby();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room has been created: " + PhotonNetwork.CurrentRoom.Name.ToString());
        base.OnCreatedRoom();
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.SetUpRoom();
        MenuManager.Instance.OpenMenu("ROOM");
        base.OnJoinedRoom();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        MenuManager.Instance.MasterClientTransfer();
        base.OnMasterClientSwitched(newMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        MenuManager.Instance.RoomCreateFailed(message);
        MenuManager.Instance.OpenMenu("ERROR");
        base.OnCreateRoomFailed(returnCode, message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room, Room cannot be found");
        MenuManager.Instance.OpenMenu("ERROR");
        base.OnJoinRoomFailed(returnCode, message);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("LOADING");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("TITLE");
        base.OnLeftRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        MenuManager.Instance.AddPlayerData(newPlayer);
        MenuManager.Instance.SetUpRoom();
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        MenuManager.Instance.SetUpRoom();
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public void StartGame()
    {
        MenuManager.Instance.OpenMenu("LOADING");
        PhotonNetwork.LoadLevel(1);
    }
}