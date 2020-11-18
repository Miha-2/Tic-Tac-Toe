using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class VersionDisplay : MonoBehaviour
{
    private TextMeshProUGUI _versionText;

    private void Awake()
    {
        _versionText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _versionText.text = "v"+Application.version;
    }
}
