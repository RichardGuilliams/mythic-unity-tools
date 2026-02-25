using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using System.Globalization;

[AddComponentMenu("ComponentsMythic/Dialogue/Dialogue")]
public class Dialogue : AbstractObject
{
    public Dialogue nextDialogue;

    public bool cancel;

    public bool ok;

    public List<Choice> choices;

    public bool triggerLetterInput;
    [TextArea(3, 10)]
    public string text;
    public Sprite portrait; 

    public void start(DialogueManager dialogueManager)
    {
        dialogueManager.dialogueBox.SetActive(true);
    }
        
    public void update(DialogueManager dialogueManager)
    {
        if (dialogueManager.input.okPressed())
        {
            dialogueManager.changeDialogue(this.nextDialogue);
        }
        else if(dialogueManager.input.cancelPressed()) 
        {
            dialogueManager.endDialogue(this.nextDialogue);
        }
    }
}
