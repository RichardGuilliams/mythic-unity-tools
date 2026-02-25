
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selector : AbstractObject
{ 
    
    public int range;
    public Material valid;
    public Material invalid;
    public Vector3 offset;
    public GameObject selector;
    public GameObject panel;
    public float tileSize = 1f;

    protected List<GameObject> spawnedPanels = new List<GameObject>();

    public virtual void showPanels()
    {

    }
    public void showSquare()
    {
        for(int i = -range; i <= range; i++)
        {
            for(int j = -range; j <= range; j++)
            {

            }
        }
    }

    bool inRange(int x, int y) => Mathf.Abs(x) + Mathf.Abs(y) <= range;
}