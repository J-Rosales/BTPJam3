using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaDisplay : MonoBehaviour
{
    [SerializeField] private Slider slider;
    PlayerMovement player;
    
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();    
    }

    void Update()
    {
        slider.value = player.currentStamina / player.maxStamina;   
    }
}
