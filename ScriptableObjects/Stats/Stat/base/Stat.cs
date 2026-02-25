using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[System.Serializable]public abstract class Stat<T> : StatBase
{
    [HideInInspector]public T value;
}