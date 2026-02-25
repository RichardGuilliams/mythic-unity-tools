using UnityEngine;
using System.Collections.Generic;

public abstract class State : AbstractObject
{
    public List<State> subStates;
    public bool changingState;
    public State nextState;

    public virtual StateManager getStateManager(){ return GetComponentInParent<StateManager>(); }
    public virtual void setNextState(State state)
    {
        this.nextState = state;
    }
    public virtual void changeState(){
        this.changingState = true;
    }
    public override void update()
    {
        if(this.changingState) this.getStateManager().changeState(this.nextState);
    }

    public virtual void Start()
    {
        
    }
}
