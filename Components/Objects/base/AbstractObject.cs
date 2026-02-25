using   UnityEngine;
using System;
using System.Collections.Generic;

public abstract class AbstractObject : MonoBehaviour
{
    [HideInInspector]public bool gizmosDisabled;     
    [HideInInspector] public string ObjectName;

    public List<Action> events;

    public virtual object getProperty(string statName)
{
    var field = GetType().GetField(statName);
    if (field != null)
        return field.GetValue(this);

    var prop = GetType().GetProperty(statName);
    if (prop != null)
        return prop.GetValue(this);

    return null;
}


    public GameObject assignedParent;
    public virtual GameObject getControllerHolder(Type type){ return GetComponentInParent<Controller>().transform.parent.gameObject; }
    public virtual GameObject getParent(){ return transform.parent.gameObject; }
    public virtual GameObject getObjectByName(string name){ return GameObject.Find(name); }
    public bool isType(Type type){ return type.IsAssignableFrom(GetType()); }
    public virtual void update(){}
    public virtual void start(){}
}