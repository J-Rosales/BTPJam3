using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    [SerializeField] private bool startWithTitleMenu;
    AsyncOperation pauseLoad;

    IntroControl intro;
    // Start is called before the first frame update
    void Start()
    {
        pauseLoad = SceneManager.LoadSceneAsync(
                "Pause", LoadSceneMode.Additive);

        if(!startWithTitleMenu)
        {
            FindObjectOfType<IntroControl>().EndTransition();
        }
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
