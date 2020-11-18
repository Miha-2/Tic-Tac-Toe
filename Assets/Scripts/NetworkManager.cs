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
    [SerializeField] private LobbyCanvas _lobbyCanvas = null;
    [SerializeField] private TextMeshProUGUI _infoText = null;
    private GamePlayer _gamePlayer = null;
    private Photon.Realtime.Player opponent;

    private void Awake()
    {
        _gamePlayer = FindObjectOfType<GamePlayer>();
        //Set reference
        _lobbyCanvas.SetReference(this);
        _startCanvas.SetReference(this);
    }

    public void Connect(string playerName)
    {
        string[] split = Application.version.Split("."[0]);
        string gameVersion = "v"+ split[0] + "." + split[1];
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = playerName;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
        _infoText.text = "Connecting to server";
    }

    public override void OnConnectedToMaster()
    {
        _infoText.text = "Successfully connected to server";
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
        //PhotonNetwork.JoinOrCreateRoom("defaultRoom", new RoomOptions{MaxPlayers = 2}, TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        _startCanvas.gameObject.SetActive(false);
        _lobbyCanvas.gameObject.SetActive(true);
        Debug.Log("Successfully joined a lobby");
    }

    public override void OnCreatedRoom()
    {
        _infoText.text = "Waiting for opponent";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _infoText.text = "Failed to create a room";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        opponent = newPlayer;
        GameStarts();
    }

    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount != 2) return;
        Player[] playerList = PhotonNetwork.PlayerList;
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i] != PhotonNetwork.LocalPlayer)
            {
                opponent = playerList[i];
                break;
            }
        }
        GameStarts();
    }

    private void GameStarts()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        _gamePlayer.opponent = opponent;
        _lobbyCanvas.gameObject.SetActive(false);
        _gameCanvas.SetActive(true);
        _gamePlayer.StartGame();
    }

    public override void OnLeftRoom()
    {
        _infoText.text = "You left the room";
        _gameCanvas.SetActive(false);
        _lobbyCanvas.gameObject.SetActive(true);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        _gamePlayer.ResetBoard(false);
        PhotonNetwork.LeaveRoom();
    }
}

public enum EventType
{
    SetImage = (byte)0,
    RematchReady = (byte)1,
    SetStartingPlayer = (byte)2
}
