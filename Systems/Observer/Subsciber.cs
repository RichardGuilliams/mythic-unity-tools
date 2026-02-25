using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

[AddComponentMenu("ComponentsMythic/Observer/Subscriber")]
public class Subscriber : AbstractObject
{
    public bool cancellingSubscription;

    public List<Response> response;
    public void respond(Message message)
    {

    }

    public void listen(Message message)
    {
        for(int i = 0; i < response.Count; i++)
            {
                if(this.response[i].message == message){
                    this.response[i].receive(message);
                    break;
                }
            }
    }
}