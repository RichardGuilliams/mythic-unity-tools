using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public abstract class Controller: AbstractObject
{
    public Movement movement;
    public Input input;

    public virtual void Start(){}
    public virtual void Update(){}
}