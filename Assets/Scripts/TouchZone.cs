using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchZone : MonoBehaviour
{
    [SerializeField] private int id = 0;

    public int Id
    {
        get { return id; }
    }
}
