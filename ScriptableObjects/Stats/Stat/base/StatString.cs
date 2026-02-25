using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "String", menuName = "Mythic/Stats/StatString")]
[System.Serializable]public class StatString : Stat<string>, IBoundMinMax<int>
{

        [HideInInspector]public int min;
        [HideInInspector]public int max;
    public int Min { get => min; set => min = value; }
    public int Max { get => max; set => min = value; }

    public override System.Type TypeValue => typeof(string);
    public override System.Type TypeMax => typeof(int);
    public override System.Type TypeMin => typeof(int);

    public override object BoxedValue
    {
        get => value!;
        set => this.value = (string)value;
    }

    public override object BoxedMax
    {
        get => value!;
        set => this.max = (int)value;
    }

        public override object BoxedMin
    {
        get => value!;
        set => this.min = (int)value;
    }
}