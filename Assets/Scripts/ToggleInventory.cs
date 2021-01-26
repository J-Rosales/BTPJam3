using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInventory : MonoBehaviour
{
    public bool startToggled;
    public KeyCode primaryInput;
    public KeyCode secondaryInput;
    [SerializeField] private GameObject equipmentTooltip;
    CanvasGroup display;
    bool toggleValue;
    
    void Awake()
    {
        display = GetComponent<CanvasGroup>();    
    }

    void Start()
    {
        if(!startToggled)
            SetToggle(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(primaryInput) || Input.GetKeyDown(secondaryInput))
            Toggle();
    }

    private void Toggle()
    {
        SetToggle(!toggleValue);
    }

    private void SetToggle(bool value)
    {
        toggleValue = value;
        display.alpha = value ? 1 : 0;
        display.blocksRaycasts = value;
        display.interactable = value;
        equipmentTooltip.SetActive(value);
    }
}
