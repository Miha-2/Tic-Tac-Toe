using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rematch : MonoBehaviour
{
    private GamePlayer _gamePlayer;
    [SerializeField] private Image _buttonFill = null;
    [SerializeField] private TextMeshProUGUI _buttonText = null;
    private bool _rematchReady;
    private bool _opponentReady;
    private static readonly Color MainColor = new Color(0.17f, 0.17f, 0.17f);
    private static readonly Color SecondaryColor = Color.white;

    private void OnEnable()
    {
        _gamePlayer = FindObjectOfType<GamePlayer>();
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClientOnEventReceived;
    }

    private void NetworkingClientOnEventReceived(EventData obj)
    {
        if (obj.Code == (byte)EventType.RematchReady)
        {
            object[] data = (object[])obj.CustomData;
            _opponentReady = (bool)data[0];
            if (_rematchReady && _opponentReady) DoRematch();
        }
    }

    public void OnClick_Rematch()
    {
        _rematchReady = !_rematchReady;
        _buttonFill.color = _rematchReady ? SecondaryColor : MainColor;
        _buttonText.color = _rematchReady ? MainColor : SecondaryColor;

        object[] data = {_rematchReady};
        
        PhotonNetwork.RaiseEvent((byte)EventType.RematchReady, data, RaiseEventOptions.Default,
            SendOptions.SendUnreliable);
            
        if(_rematchReady && _opponentReady)
            DoRematch();
            
        
        //_gamePlayer.ResetBoard();
    }

    private void DoRematch()
    {
        _rematchReady = false;
        _opponentReady = false;
        _buttonFill.color = MainColor;
        _buttonText.color = SecondaryColor;
        _gamePlayer.ResetBoard(true);
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClientOnEventReceived;
    }
}
