using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]public abstract class StatBase : AbstractScriptable
{

    // Holds the base types for the value, max and min properties in the child classes;
    public abstract System.Type TypeValue {get;}
    public abstract System.Type TypeMax {get;}
    public abstract System.Type TypeMin {get;}

    // Used to set the values of the value, max and min properties in the child classes
    public abstract object BoxedValue { get; set; }
    public abstract object BoxedMin { get; set; }
    public abstract object BoxedMax { get; set; }
}