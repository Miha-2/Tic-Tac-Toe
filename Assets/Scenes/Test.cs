using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat;

public class Test : MonoBehaviour
{
    private LocalizedString _myString;
    [SerializeField] private LocalizeStringEvent _event;
    [SerializeField] private string newText;

    [HideInInspector] public string myText = "ab1";
    public string MyText
    {
        get => myText;
        set
        {
            myText = value;
            _myString.RefreshString();
        }
    }
    public float TimeNow => Time.time;
    private void Start()
    {
        _myString = _event.StringReference;
        //_myString.StringChanged += UpdateString;
    }
    void OnGUI()
    {
        //_myString.RefreshString();
    }

    [ContextMenu("UpdateString")]
    private void UpdateString()
    {
        MyText = newText;
    }
}
