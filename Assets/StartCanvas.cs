using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] private Button _goOnlineBtn = null;
    [SerializeField] private TMP_InputField _inputText = null;
    [SerializeField] private GameObject _infoText = null;
    private NetworkManager _netManager;
    private const string Nickname = "Nickname";

    public void SetReference(NetworkManager netManager)
    {
        _netManager = netManager;
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey(Nickname))
        {
            _inputText.text = PlayerPrefs.GetString(Nickname);
            _goOnlineBtn.interactable = true;
        }
    }
    
    public void OnUpdatedName()
    {
        _goOnlineBtn.interactable = !string.IsNullOrEmpty(_inputText.text);
    }

    public void OnClick_GoOnline()
    {
        _goOnlineBtn.gameObject.SetActive(false);
        _inputText.gameObject.SetActive(false);
        _infoText.SetActive(true);
        PlayerPrefs.SetString(Nickname, _inputText.text);
        _netManager.Connect(_inputText.text);
    }
}
