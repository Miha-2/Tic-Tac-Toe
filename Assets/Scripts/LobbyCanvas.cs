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
    private RoomOptions defaultRoom = new RoomOptions() {MaxPlayers = 2};
    [SerializeField] private TextMeshProUGUI _waitingText = null;
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
        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName, defaultRoom, TypedLobby.Default);
    }
    
    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    [SerializeField] private TextMeshProUGUI debugText = null;
    public override void OnCreatedRoom()
    {
        //debugText.color = Color.green;
        playerListingMenu.gameObject.SetActive(false);
        _waitingText.gameObject.SetActive(true);
        playerListingMenu.ResetListings();
    }

    public void LeftRoom()
    {
        playerListingMenu.gameObject.SetActive(true);
        _waitingText.gameObject.SetActive(false);
        print("Local player left the room! +++ in Lobby canvas");
        //debugText.color = Color.red;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //playerListingMenu.gameObject.SetActive(true);
        //debugText.color = Color.red;
    }

    public void JoinedRoom()
    {
        playerListingMenu.ResetListings();
    }
}