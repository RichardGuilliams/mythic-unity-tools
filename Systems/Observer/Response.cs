using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

[AddComponentMenu("ComponentsMythic/Observer/Response")]
public abstract class Response : AbstractObject
{
    public int RespondCounter = 0;
    public int MaxResponses = 1;
    public Message nextMessage;
    public Message message;

    public virtual void sendNextMessage()
    {
        GetComponent<Observer>().useResponse(this);
    }

    public virtual void receive(Message message)
    {
        
    }
}