using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    [Header("Room List")]
    [SerializeField] Menu[] menuList;

    [Header("UI Text")]
    [SerializeField] public TMP_Text errorText;
    [SerializeField] public TMP_Text roomNameText;
    [SerializeField] public TMP_Text roomHostName;
    [SerializeField] public TMP_Text roomPlayerCount;

    [Header("UI Input Fields")]
    [SerializeField] public TMP_InputField createRoomInput;
    [SerializeField] public TMP_InputField joinRoomInput;

    [Header("Misc")]
    [SerializeField] public Transform playerListContent;
    [SerializeField] public GameObject playerListItemPrefab;
    [SerializeField] public GameObject startGameButton;

    private void Awake()
    {
        Instance = this;
    }

    public void SetUpPlayerName(string name)
    {
        PhotonNetwork.NickName = name;
        OpenMenu("TITLE");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createRoomInput.text))
        {
            return;
        }
        else
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(createRoomInput.text, options);
            OpenMenu("LOADING");
        }
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinRoomInput.text))
        {
            return;
        }
        else
        {
            PhotonNetwork.JoinRoom(joinRoomInput.text);
            OpenMenu("LOADING");
        }
    }

    public void SetUpRoom()
    {
        roomNameText.text = "Room: " + PhotonNetwork.CurrentRoom.Name.ToString();
        roomHostName.text = "Hosted by: " + PhotonNetwork.MasterClient.NickName.ToString();

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        roomPlayerCount.text = PhotonNetwork.PlayerList.Length.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void MasterClientTransfer()
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void RoomCreateFailed(string message)
    {
        errorText.text = "Room Creation Has Failed: " + message;
    }

    public void AddPlayerData(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menuList.Length; i++)
        {
            if (menuList[i].menuName == menuName)
            {
                menuList[i].Open();
            }
            else if (menuList[i])
            {
                CloseMenu(menuList[i]);
            }
        }
    }
    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menuList.Length; i++)
        {
            if (menuList[i])
            {
                CloseMenu(menuList[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}