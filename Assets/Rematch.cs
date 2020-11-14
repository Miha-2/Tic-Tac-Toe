using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Rematch : MonoBehaviour
{
    private GamePlayer _gamePlayer;
    [SerializeField] private Image _buttonFill = null;
    private bool _rematchReady;
    private bool _opponentReady;
    private static readonly Color defaultColor = new Color(0.17f, 0.17f, 0.17f);
    private static readonly Color rematchColor = new Color(0.24f, 0.45f, 0.24f);
    private const int REMATCH_READY = 1;

    private void OnEnable()
    {
        _gamePlayer = FindObjectOfType<GamePlayer>();
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClientOnEventReceived;
    }

    private void NetworkingClientOnEventReceived(EventData obj)
    {
        object data = obj.CustomData;
        if (obj.Code == REMATCH_READY)
        {
            _opponentReady = (bool)data;
            if (_rematchReady && _opponentReady) DoRematch();
        }
    }

    public void OnClick_Rematch()
    {
        _rematchReady = !_rematchReady;
        _buttonFill.color = _rematchReady ? rematchColor : defaultColor;
        
        PhotonNetwork.RaiseEvent(REMATCH_READY, _rematchReady, RaiseEventOptions.Default,
            SendOptions.SendUnreliable);
            
        if(_rematchReady && _opponentReady)
            DoRematch();
            
        
        //_gamePlayer.ResetBoard();
    }

    private void DoRematch()
    {
        _rematchReady = false;
        _opponentReady = false;
        _buttonFill.color = defaultColor;
        _gamePlayer.ResetBoard(true);
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClientOnEventReceived;
    }
}
