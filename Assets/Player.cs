using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private EventSystem _eventSystem = null;
    [SerializeField] private GraphicRaycaster _graphicRaycaster = null;

    private void Start()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int z = ChooseZone();
            if (z == -1) return;
            //Continue with field logic
        }
    }

    private int ChooseZone()
    {
        PointerEventData _eventData = new PointerEventData(_eventSystem);
        _eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(_eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent(out TouchZone t))
            {
                return t.Id;
            }
        }

        return -1;
    }
}
