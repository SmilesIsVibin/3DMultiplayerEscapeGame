using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerDetails : MonoBehaviourPunCallbacks
{
    public TMP_Text playerName;
    public TMP_Text playerType;
    public TMP_Text playerScore;

    Player player;

    public void SetupPlayerDetails(Player player)
    {
        if (player == null)
        {
            return;
        }
        playerName.text = player.NickName;
        this.player = player;
        UpdateStats();
    }
    void UpdateStats()
    {
        if(player.CustomProperties.TryGetValue("PlayerScore", out object score))
        {
            playerScore.text = score.ToString();
        }
        if (player.CustomProperties.TryGetValue("PlayerType", out object type))
        {
            playerType.text = type.ToString();
            if(type.ToString() == "Runner")
            {
                playerType.color = Color.green;
                playerName.color = Color.green;
                //playerScore.color = Color.green;
            }else if (type.ToString() == "Seeker")
            {
                playerType.color = Color.red;
                playerName.color = Color.red;
                //playerScore.color = Color.red;
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(targetPlayer == player)
        {
            if (changedProps.ContainsKey("PlayerScore"))
            {
                UpdateStats();
            }
            if (changedProps.ContainsKey("PlayerType"))
            {
                UpdateStats();
            }
        }
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }
}
