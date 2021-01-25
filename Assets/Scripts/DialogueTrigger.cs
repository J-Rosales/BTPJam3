using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    DialogueControl dialogue;
    [TextArea(2, 4)] public string text;
    public bool read;

    private void Start()
    {
        dialogue = FindObjectOfType<DialogueControl>();    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!read && other.GetComponent<Player>())
        {
            dialogue.Print(text);
            read = true;
        }
    }
}
