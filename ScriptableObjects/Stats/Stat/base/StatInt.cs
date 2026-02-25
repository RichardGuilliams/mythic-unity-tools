using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Int", menuName = "Mythic/Stats/StatInt")]
[System.Serializable]public class StatInt : StatNumeric<int>, IBoundMinMax<int>
{

    public override System.Type TypeValue => typeof(int);
    public override System.Type TypeMax => typeof(int);
    public override System.Type TypeMin => typeof(int);

    public override object BoxedValue
    {
        get => value!;
        set => this.value = (int)value;
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