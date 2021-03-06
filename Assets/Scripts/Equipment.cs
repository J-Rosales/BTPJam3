﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public string displayName;
    public string statRow1;
    public string statRow2;
    public string effectText;

    EquipmentTooltipControl tooltipControl;
    Camera gameCamera;
    // Start is called before the first frame update
    void Start()
    {
        tooltipControl = FindObjectOfType<EquipmentTooltipControl>();
        gameCamera = FindObjectOfType<Camera>();
    }

    public void ShowDisplay()
    {
        RectTransform tooltipTransform = tooltipControl.transform as RectTransform;
        tooltipTransform.position = new Vector3(
                tooltipTransform.position.x,
                Input.mousePosition.y, 0);
        tooltipControl.SetLabels(displayName, statRow1, statRow2, effectText);
        tooltipControl.SetVisibility(true);
    }

    public void HideDisplay()
    {
        tooltipControl.SetVisibility(false);
    }
}
