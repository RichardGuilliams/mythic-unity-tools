using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using System.Net;

[AddComponentMenu("ComponentsMythic/Observer/Observer")]
public class Observer : AbstractObject
{
    public List<Subscriber> subscribers;
    public Message message;

    public override void start()
    {
        this.subscribers = new List<Subscriber>();
    }

    public void messageOne()
    {
        
    }

    public void messageAll()
    {
        
    }

    public void messageSelected(List<Subscriber> subscribers)
    {
        
    }

    public void sendMessage(Subscriber subscriber)
    {
        subscriber.listen(this.message);
    }

    public void unsubscribe(Subscriber subscriber)
    {
        this.subscribers.Remove(subscriber);
    }

    public void unsubscribeAll()
    {
        this.subscribers.Clear();
    }

    public void unsubscribeSelected(List<Subscriber> subscribers)
    {
        for(var i = 0; i < subscribers.Count; i++)
        {
            this.subscribers.Remove(subscribers[i]);
        }
    }

    public void subscribe(Subscriber subscriber)
    {
        this.subscribers.Add(subscriber);
    }

    public void useResponse(Response response)
    {
        
    }
}
