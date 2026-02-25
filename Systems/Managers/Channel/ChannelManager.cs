using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

[AddComponentMenu("ComponentsMythic/Data/Data Manager")]
public abstract class Channel<TType>{
    public string name;
    public TType subject;


    public virtual void OnChange()
    {}

    public void Start()
    {
        
    }

    public void Update()
    {
        
    }

    

    public delegate void ChannelAction(object data);
    public event ChannelAction OnChannelEvent;
    public List<string> channels;
}