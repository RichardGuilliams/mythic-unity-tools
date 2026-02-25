using System.ComponentModel.Design;
using UnityEngine;

[AddComponentMenu("ComponentsMythic/Collision/Collision Manager")]
public class CollisionManager : Manager
{
    public LayerMask layerMask;
    public Collider col;
    public int collisionCount = 0;
    public bool colliding;

    public bool triggered;
    private bool IsColliding => collisionCount > 0;

    public Collider[] collisions;

    public void checkCollider()
    {
        this.resetState();
        this.collisions = Physics.OverlapBox(col.bounds.center, col.bounds.extents, Quaternion.identity, this.layerMask);
        
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i] == col) continue; // skip self

            if (collisions[i].isTrigger)
                triggered = true;
            else
                colliding = true;
        }
    }

    public void resetState()
    {
        this.colliding = false;
        this.triggered = false;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        col = GetComponent<Collider>();
    }

    public override void Update()
    {
        this.checkCollider();
    }   
}