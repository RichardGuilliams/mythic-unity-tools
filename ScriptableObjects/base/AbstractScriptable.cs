using UnityEngine;
using System;

public abstract class AbstractScriptable : ScriptableObject
{
    [HideInInspector]public bool drawGizmos;     
    public virtual bool isType(Type type){ return type.IsAssignableFrom(GetType()); }

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
}
