using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerListing : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI nameDisplay = null;
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            nameDisplay.text = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PhotonNetwork.JoinRoom(Name);
    }
}
