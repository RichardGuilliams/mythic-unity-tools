using UnityEngine;

[AddComponentMenu("ComponentsMythic/Mode/ExploreMode")]
public class StateExplore : State
{
    public GameObject player;
    public Movement movement;
    public InputManager input;
    public override void update()
    {
        if(movement.Moving(input)) movement.Move(input);
    }
}