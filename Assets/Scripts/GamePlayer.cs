﻿// ReSharper disable PossibleLossOfFraction
using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
// ReSharper disable Unity.InefficientMultidimensionalArrayUsage
// ReSharper disable InconsistentNaming

public class GamePlayer : MonoBehaviour
{
    [SerializeField] private Image circle = null;
    [SerializeField] private Image cross = null;
    private static readonly Color unconfirmedColor = new Color(0.45f, 0.45f, 0.45f);
    [SerializeField] private TextMeshProUGUI winnerText = null;
    //public string resetButton = "R";
    [SerializeField] private Transform _gameCanvas = null;
    private FieldType myType;
    private int zoneId = -1;
    private FieldType[,] zones = new FieldType[3,3];
    [SerializeField] private Transform[] _zoneTransforms = null;
    private Image lastImage;
    private bool isFinnished = false;
    private bool _hasTurn = false;
    [SerializeField] private GameObject _rematchButton = null;
    [SerializeField] private GameObject _randomButton = null;
    private Image[] placedImages = new Image[9];
    public Photon.Realtime.Player opponent;
    
    private EventSystem _eventSystem;
    [SerializeField] private GraphicRaycaster _graphicRaycaster = null;
    [SerializeField] private PlayerInfoDisplay selfDisplay = null;
    [SerializeField] private PlayerInfoDisplay opponentDisplay = null;
    
    private bool HasTurn
    {
        get => _hasTurn;
        set
        {
            selfDisplay.HasTurn = value;
            opponentDisplay.HasTurn = !value;
            _hasTurn = value;
        }
    }

    private void OnEnable()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClientOnEventReceived;
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            HasTurn = Random.Range(0f, 1f) < .5f;
            object[] data = {!HasTurn};
            PhotonNetwork.RaiseEvent((byte) EventType.SetStartingPlayer, data, RaiseEventOptions.Default,
                SendOptions.SendReliable);
        }

        myType = PhotonNetwork.IsMasterClient ? FieldType.Cross : FieldType.Circle;

        selfDisplay.Name = PhotonNetwork.LocalPlayer.NickName;
        selfDisplay.Type = myType == FieldType.Circle ? circle : cross;
        opponentDisplay.Name = opponent.NickName;
        opponentDisplay.Type = myType == FieldType.Circle ? cross : circle;
    }
    
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.R));
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if(Input.GetMouseButtonDown(0) && HasTurn)
        {
            ChooseZone();
        }
    }
    
    private void ChooseZone()
    {
        PointerEventData _eventData = new PointerEventData(_eventSystem);
        _eventData.position = Input.mousePosition;
        
        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(_eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent(out TouchZone t))
            {
                TouchZone(t.Id);
            }
        }
    }

    [ContextMenu("Random Zone")]
    public void OnClick_SelectRandom()
    {
        if(!HasTurn) return;
        int freeZones = 9;
        foreach (FieldType type in zones)
        {
            if(type != FieldType.Nobody)
                freeZones--;
        }

        int randomZone = Random.Range(1, freeZones + 1);
        int randomId = 0;
        foreach (FieldType type in zones)
        {
            if (randomZone == 0) break;
            if (type != FieldType.Nobody)
                randomId++;
            else
            {
                randomZone--;
                randomId++;
            }
        }
        
        TouchZone(randomId);
        TouchZone(randomId);
    }

    private void TouchZone(int _zoneId)
    {
        if (isFinnished) return;
        _zoneId -= 1;
        if(zones[Mathf.FloorToInt(_zoneId/3),_zoneId % 3] != FieldType.Nobody) return;
        if (_zoneId != zoneId)
        {
            Image toInst = myType == FieldType.Cross ? cross : circle;
            if (lastImage != null) lastImage.transform.position = _zoneTransforms[_zoneId].position;
            else lastImage = Instantiate(toInst, _zoneTransforms[_zoneId].position, Quaternion.identity, _gameCanvas);
            lastImage.color = unconfirmedColor;
            placedImages[_zoneId] = lastImage;
            zoneId = _zoneId;
        }
        else
        {
            zones[Mathf.FloorToInt(_zoneId/3),_zoneId % 3] = myType;
            lastImage.color = Color.white;
            HasTurn = false;

            byte _win = CheckForWin(_zoneId, myType);
            
            object[] l = {_zoneId, myType, _win};
            PhotonNetwork.RaiseEvent((byte) EventType.SetImage, l, RaiseEventOptions.Default, SendOptions.SendReliable);
            lastImage = null;
        }
    }

    private void NetworkingClientOnEventReceived(EventData obj)
    {
        switch (obj.Code)
        {
            case (byte)EventType.SetImage:
            {
                object[] data = (object[]) obj.CustomData;
                HasTurn = true;
            
                Image toInst = (FieldType)data[1] == FieldType.Cross ? cross : circle;
                Image i = Instantiate(toInst, _zoneTransforms[(int)data[0]].position, quaternion.identity, _gameCanvas);
                zones[Mathf.FloorToInt((int)data[0]/3),(int)data[0] % 3] = (FieldType)data[1];
            
                placedImages[(int)data[0]] = i;
            
                if((byte)data[2] != 0)
                    Win((byte)data[2] == 1 ? (FieldType)data[1] : FieldType.Nobody);
                break;
            }
            case (byte)EventType.SetStartingPlayer:
            {
                object[] data = (object[]) obj.CustomData;
                //print("Received: "+(bool)data[0]);
                HasTurn = (bool)data[0];
                break;
            }
        }
    }
    //0 => no win
    //1 => placer wins
    //2 => draw
    private byte CheckForWin(int placeId, FieldType type)
    {
        bool isEmpty = false;
        int i1 = Mathf.FloorToInt(placeId / 3);
        int j1 = placeId % 3;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (zones[i + 1, j + 1] == FieldType.Nobody) isEmpty = true;
                
                if(i == 0 && j == 0) continue;
                if (i1 + i < 0 || i1 + i > 2 || j1 + j < 0 || j1 + j > 2) continue; //out of bounds
                if (zones[i1 + i, j1 + j] != type) continue; //not a same type neighbour
                if (i1 + 2 * i >= 0 && i1 + 2 * i <= 2 && j1 + 2 * j >= 0 && j1 + 2 * j <= 2)
                {
                    if (zones[i1 + 2 * i, j1 + 2 * j] != type) continue;
                    Win(type);
                    return 1;
                }
                if (i1 - i >= 0 && i1 - i <= 2 && j1 - j >= 0 && j1 - j <= 2)
                {
                    if (zones[i1 - i, j1 - j] != type) continue;
                    Win(type);
                    return 1;
                }
            }
        }

        if (!isEmpty)
        {
            Win(FieldType.Nobody);
            return 2;
        }

        return 0;
    }

    private void Win(FieldType type)
    {
        isFinnished = true;
        _rematchButton.SetActive(true);
        _randomButton.SetActive(false);

        selfDisplay.HasTurn = false;
        opponentDisplay.HasTurn = false;
        
        string t;
        if (type == myType)
            t = "You won!";
        else if (type == FieldType.Nobody)
            t = "Draw!";
        else
            t = "You lost!";

        winnerText.text = t;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClientOnEventReceived;
    }

    public void ResetBoard(bool rematch)
    {
        for (int index = 0; index < placedImages.Length; index++)
        {
            Image placement = placedImages[index];
            placedImages[index] = null;
            if (placement == null)
            {
                 continue;
            }
            Destroy(placement.gameObject);

            zones[Mathf.FloorToInt(index / 3), index % 3] = FieldType.Nobody;
        }

        _rematchButton.SetActive(false);
        _randomButton.SetActive(true);
        winnerText.text = String.Empty;
        isFinnished = false;
        if(rematch)
            StartGame();
    }
}



public enum FieldType
{
    Nobody = 0,
    Cross = 1,
    Circle = 2
}