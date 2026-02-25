using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;

[AddComponentMenu("ComponentsMythic/Managers/StateManager")]
public class StateManager : Manager
{
    public State state;
    [SerializeField]
    public List<State> possibleStates;


    public override void start()
    {

    }

    public override void update()
    {
        this.state.update();
    }

    public void noState()
    {
        Console.WriteLine("There is no state");
    }

    public void changeState(State state)
    {
        this.state = state;
    }
}