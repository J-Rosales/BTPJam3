using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Stamina : MonoBehaviour
{
    public Slider GUIDisplay;
    public bool running;
    public float max;
    public float current;
    [Tooltip("Time (s) to regain 1 lose")]
    public float decayTime;
    [Tooltip("Time (s) to regain 1 stamina")]
    public float regenTime;
    public float displayPixelsPerStamina;
    public KeyCode runInput;

    float counter;
    private void Update()
    {
        if(Input.GetKey(runInput))
        {
            running = true;
            if(current > 0)
            {
                counter += Time.deltaTime;
                if(counter > decayTime)
                {
                    counter = 0;
                    current--;
                    return;
                }
            }
            if(current == 0)
                running = false;
        } else
        {
            if(running)
            {
                running = false;
                counter = 0;
            }

            if(current < max)
            {
                counter += Time.deltaTime;
                if(counter > regenTime)
                {
                    counter = 0;
                    current++;
                }
            }
        }
        
        GUIDisplay.value = current / max;
    }
}
