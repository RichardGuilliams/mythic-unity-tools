using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;

[AddComponentMenu("ComponentsMythic/Managers/SceneManager")]
public class SceneManager : Manager
{
    public StateManager stateManager;
    public AnimationManager animationManager;
    public InputManager inputManager;

    public override void Update()
    {
        this.stateManager?.update();
        this.animationManager?.update();
    }
}