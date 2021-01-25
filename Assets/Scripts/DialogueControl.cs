using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private float sleepTime;
    IEnumerator printRoutine;
    Canvas canvas;
    
    private void Awake()
    {
        canvas = GetComponent<Canvas>();    
    }

    public void Print(string dialogue)
    {
        canvas.enabled = true;
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
            textComponent.maxVisibleCharacters = visibleCount;
            visibleCount += 1;

            yield return new WaitForSeconds(sleepTime);
        }
    }
}
