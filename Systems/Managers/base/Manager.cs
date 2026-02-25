using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Manager : AbstractObject
{
    
    public override void start(){}
    public virtual void Update(){}


    public GameObject getObjectsParent(GameObject obj)
    {

        return obj.transform.gameObject;
    }

}
