using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.AccessControl;
//TODO: build out Diamond, Square, Line, DiagonalLine, Cone, Triangle, and Circle selection shapes. Also build out a way to create custom shapes.
public abstract class SelectionShape: ScriptableObject
{
    public Selector selector;
    public GameObject origin;
    public GameObject panel;
    public int range;

    public void setRange(int newRange)
    {
        this.range = newRange;
    }

    public void setup(Selector selector, GameObject origin, GameObject panel)
    {
        this.selector = selector;
        this.origin = origin;
        this.panel = panel;
    }
    public virtual void spawnPanel(int x, int y, GameObject obj)
    {
        Vector3 pos = origin.transform.position + new Vector3(x * selector.tileSize, selector.offset.y, (y) * selector.tileSize);
        Instantiate(panel, pos, Quaternion.identity, obj.transform);
    }
    public virtual void showPanels(GameObject obj)
    {
        
    }
}