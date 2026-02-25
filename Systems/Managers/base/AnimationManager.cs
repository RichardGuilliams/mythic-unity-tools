using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

[AddComponentMenu("ComponentsMythic/Managers/Animation")]
public class AnimationManager : Manager
{
    public int index = 0;

    public List<Animation> animations;
    public Animation currentAnimation;
    
    public override void start()
    {
        this.getAnimations();
        Console.WriteLine("hi");
    }

    public virtual void getAnimations()
    {
        this.animations = FindObjectsByType<Animation>(FindObjectsSortMode.None).ToList();
    }

    public virtual void updateIndex()
    {
        if(this.index == this.animations.Count - 1) {
            this.index = 0;
        } else {
            this.index++;
        }
    }
    public override void Update()
    {
        if(this.animations.Count > 0)
        {
            this.animations[this.index].update();
            this.updateIndex();
        }
    }

    public void clearAnimation()
    {
        this.currentAnimation = null;
    }
}