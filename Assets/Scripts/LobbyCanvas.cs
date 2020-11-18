using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class LobbyCanvas : MonoBehaviourPunCallbacks
{
    private NetworkManager _netManager;
    [SerializeField] private PlayerListingMenu playerListingMenu = null;
    public void SetReference(NetworkManager netManager)
    {
        _netManager = netManager;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        playerListingMenu.UpdateListings(roomList);
    }

    public void OnClick_CreateNewRoom()
    {
        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName);
    }
}