using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText = null;
    [SerializeField] private Image _logoImage = null;
    [SerializeField] private Animator _pointerAnimator = null;
    private static readonly int Turn = Animator.StringToHash("HasTurn");

    public string Name
    {
        set => _nameText.text = value;
    }
    public Image Type
    {
        set => _logoImage.sprite = value.sprite;
    }
    public bool HasTurn
    {
        set {
            if (_pointerAnimator.isActiveAndEnabled == false)
            {
                print("WHAT! Animator is not ENABLED!!");
                return;
            }

            _pointerAnimator.SetBool(Turn, value); }
    }
}
