using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public KeyCode pauseInput;
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private bool startPaused;
    Canvas canvas;
    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = startPaused;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(pauseInput))
        {
            canvas.enabled = !canvas.enabled;
            Time.timeScale = canvas.enabled ? 0f : 1f;
            SwitchToPause();
        }
    }

    public void SwitchToSettings()
    {
        buttonsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void SwitchToPause()
    {
        buttonsPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        canvas.enabled = false;
    }
    
}
