
using System.Data;
using System.Runtime.Serialization;
using UnityEngine;

[AddComponentMenu("Movement/Grid Movement")]
public class GridMovement : Movement
{
    public MapGrid grid;
    public bool active = true;
    public InputManager input;
    public GameObject unit;
    public CollisionManager collisionManager;

    public Selector selector;

    public override void Start()
    {
        selector.showPanels();
    }

    public void FixedUpdate()
    {
        if(!this.active) return;
        if(this.input.JoystickMoved() && !this.collisionManager.colliding) this.MoveAxis(input.Joystick3D());
    }

    public float getSpeed()
    {
        return this.unit.GetComponent<Character>().stats.GetValue<float>("speed") * .1f;
    }

    public void MoveAxis(Vector3 axis)
    {
        float spd = getSpeed();
        Vector3 move = new Vector3(axis.x * spd, 0, axis.z * spd);
        transform.position = (transform.position + move * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        grid?.DrawGizmos();
        Gizmos.color = Color.gray;


    }
}
