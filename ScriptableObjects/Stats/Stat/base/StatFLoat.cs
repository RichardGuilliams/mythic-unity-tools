using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Float", menuName = "Mythic/Stats/StatFloat")]
[System.Serializable]public class StatFloat : StatNumeric<float>, IBoundMinMax<float>
{

    public override System.Type TypeValue => typeof(float);
    public override System.Type TypeMax => typeof(float);
    public override System.Type TypeMin => typeof(float);

    public override object BoxedValue
    {
        get => value!;
        set => this.value = (float)value;
    }

    public override object BoxedMax
    {
        get => value!;
        set => this.max = (float)value;
    }

        public override object BoxedMin
    {
        get => value!;
        set => this.min = (float)value;
    }
}