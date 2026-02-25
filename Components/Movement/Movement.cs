using System.Numerics;
using UnityEngine;

public abstract class Movement : AbstractObject
{
    public float spd;
    public bool moving;
    public virtual void Move(InputManager input){}
    public virtual void Update(){}
    public virtual void Start()
    {
        
    }

    public virtual bool Moving(InputManager input)
    {
        if(input.leftPressed() || input.rightPressed() || input.upPressed() || input.downPressed())
        {
            this.moving = true;
        }
        if(input.JoystickMoved())
        {
            this.moving = true;
        }
        else
        {
            this.moving = false;
        }
        return this.moving;
    }
}
