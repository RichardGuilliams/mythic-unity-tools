using System.Globalization;
using UnityEngine;

[AddComponentMenu("States/Move")]
public class StateMove : State
{
    public Movement movement;
    public InputManager input;
    public override void update()
    {
        if(movement.Moving(input)) movement.Move(input);
    }
}