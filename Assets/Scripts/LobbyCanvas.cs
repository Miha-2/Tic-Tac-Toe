using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class LobbyCanvas : MonoBehaviourPunCallbacks
{
    private NetworkManager _netManager;
    public void SetReference(NetworkManager netManager)
    {
        _netManager = netManager;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
}