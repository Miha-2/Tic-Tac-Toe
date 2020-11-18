using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
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
    
    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    [SerializeField] private TextMeshProUGUI debugText = null;
    public override void OnCreatedRoom()
    {
        debugText.color = Color.green;
    }

    public void LeftRoom()
    {
        print("Local player left the room! +++ in Lobby canvas");
        debugText.color = Color.red;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        debugText.color = Color.red;
    }
}