using UnityEngine;
using System;
using System.Collections.Generic;
public abstract class StatNumeric<T> : Stat<T>, IBoundMinMax<T> where T : struct, System.IComparable<T>
{
    [HideInInspector]public T min;
        [HideInInspector]public T max;

    public T Min { get => min; set => min = value; }
    public T Max { get => max; set => max = value; }
}