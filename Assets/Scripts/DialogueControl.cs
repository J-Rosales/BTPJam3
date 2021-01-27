using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueControl : MonoBehaviour
{
    public KeyCode continueInput;
    public KeyCode instantInput;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private float sleepTime;
    public bool printing;
    public bool ready;
    public GameObject dialogueBoxObject;

    IEnumerator printRoutine;
    Canvas canvas;
    PlayerMovement player;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();    
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        SetBoxState(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(continueInput) && ready)
        {
            SetBoxState(false);
            ready = false;
        }
        if(Input.GetKeyDown(instantInput))
            ready = true;
    }

    public void Print(string dialogue)
    {
        SetBoxState(true);
        canvas.enabled = true;
        printing = true;
        printRoutine = PrintRoutine(dialogue);
        StartCoroutine(printRoutine);
    }

    public IEnumerator PrintRoutine(string dialogue)
    {   
        textComponent.text = dialogue;
        textComponent.ForceMeshUpdate();
        textComponent.maxVisibleCharacters = 0;
        TMP_TextInfo textInfo = textComponent.textInfo;

        int totalVisibleCharacters = textInfo.characterCount;
        int visibleCount = 0;
        
        while (visibleCount < totalVisibleCharacters)
        {
            visibleCount += 1;
            if(ready)
                visibleCount = totalVisibleCharacters;
            
            textComponent.maxVisibleCharacters = visibleCount;
            yield return new WaitForSeconds(sleepTime);
        }
        printing = false;
        ready = true;
    }

    public void SetBoxState(bool value)
    {
        dialogueBoxObject.SetActive(value);
        player.canMove = !value;
    }
}
