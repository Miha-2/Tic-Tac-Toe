using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    
    [SerializeField] private GameObject _gameCanvas = null;
    [SerializeField] private StartCanvas _startCanvas = null;
    [SerializeField] private TextMeshProUGUI _infoText = null;
    private GamePlayer _gamePlayer = null;

    private void Awake()
    {
        _gamePlayer = FindObjectOfType<GamePlayer>();
        //Set reference
        _startCanvas.SetReference(this);
    }

    public void Connect(string playerName)
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.NickName = playerName;
        PhotonNetwork.ConnectUsingSettings();
        _infoText.text = "Connecting to server";
    }

    public override void OnConnectedToMaster()
    {
        _infoText.text = "Successfully connected to server";
        Debug.Log("Connected to master");
        PhotonNetwork.JoinOrCreateRoom("defaultRoom", new RoomOptions{MaxPlayers = 2}, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        _infoText.text = "Waiting for opponent";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _infoText.text = "Failed to create a room";
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        GameStarts();
    }

    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount != 2) return;
        GameStarts();
    }

    private void GameStarts()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        _gamePlayer.StartGame();
        _startCanvas.gameObject.SetActive(false);
        _gameCanvas.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        _infoText.text = "You left the room";
        _gameCanvas.SetActive(false);
        _startCanvas.gameObject.SetActive(true);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        _gamePlayer.ResetBoard(false);
        PhotonNetwork.LeaveRoom();
    }
}
