using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

[AddComponentMenu("ComponentsMythic/Dialogue/Dialogue Manager")]
public class DialogueManager : Manager
{
    public bool active = false;
    public GameObject dialogueBox;
    public Dialogue currentDialogue;
    public InputManager input;
    public override void start()
    {
        this.active = true;
    }
    public bool okPressed()
    {
        return this.input.okPressed();
    }

    public bool cancelPressed()
    {
        return this.input.cancelPressed();
    }
    
    public override void Update()
    {
        if(!this.dialogueBox.activeSelf) this.dialogueBox.SetActive(true);
        if(this.active)
        {
            if(this.currentDialogue != null)
            {
                this.currentDialogue.update(this);
            }
        }
    }
    public override void update()
    {
        if(this.active)
        {
            if(this.currentDialogue != null)
            {
                this.currentDialogue.update(this);
            }
        }
    }

    public void changeDialogue(Dialogue dialogue)
    {
        this.currentDialogue = dialogue;
        this.currentDialogue.start(this);
    }

    public void endDialogue(Dialogue dialogue)
    {
        this.currentDialogue = dialogue;
        if(this.currentDialogue != null) this.currentDialogue.start(this);
        else this.dialogueBox.SetActive(false);
    }

}
