using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("ComponentsMythic/Dialogue/Choice")]
public class Choice : AbstractObject
{
    public string text;

    public Component response;
    public Dialogue nextDialogue;

    public void UseTarget(GameObject target)
    {
        
    }
}