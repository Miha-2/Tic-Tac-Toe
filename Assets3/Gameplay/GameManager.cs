// ReSharper disable PossibleLossOfFraction
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image circle = null;
    [SerializeField] private Image cross = null;
    [SerializeField] private Color unconfirmedColor = new Color(0.45f, 0.45f, 0.45f);
    [SerializeField] private TextMeshProUGUI winnerText = null;
    public string resetButton = "R";
    private Transform canvas;
    private bool isCross = true;
    private int zoneId = -1;
    private FieldType[,] zones = new FieldType[3,3];
    private Image lastImage;
    private bool isFinnished = false;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        isCross = Random.Range(0f, 1f) > .5f;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.R)) return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TouchZone(int _zoneId, Transform _zoneTransform)
    {
        if (isFinnished) return;
        _zoneId -= 1;
        if(zones[Mathf.FloorToInt(_zoneId/3),_zoneId % 3] != FieldType.Nobody) return;
        if (_zoneId != zoneId)
        {
            Image toInst = isCross ? cross : circle;
            if (lastImage != null) lastImage.transform.position = _zoneTransform.position;
            else lastImage = Instantiate(toInst, _zoneTransform.position, Quaternion.identity, canvas);
            lastImage.color = unconfirmedColor;
            zoneId = _zoneId;
        }
        else
        {
            FieldType t =isCross ? FieldType.Cross : FieldType.Circle;
            zones[Mathf.FloorToInt(_zoneId/3),_zoneId % 3] = t;
            isCross = !isCross;
            lastImage.color = Color.white;
            lastImage = null;
            CheckForWin(_zoneId, t);
        }
    }

    private void CheckForWin(int placeId, FieldType type)
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
                    return;
                }
                if (i1 - i >= 0 && i1 - i <= 2 && j1 - j >= 0 && j1 - j <= 2)
                {
                    if (zones[i1 - i, j1 - j] != type) continue;
                    Win(type);
                    return;
                }
            }
        }
        
        if(!isEmpty)Win(FieldType.Nobody);
    }

    private void Win(FieldType type)
    {
        isFinnished = true;
        winnerText.text = type + " won!";
    }
}



public enum FieldType
{
    Nobody = 0,
    Cross = 1,
    Circle = 2
}