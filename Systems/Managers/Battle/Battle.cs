using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[AddComponentMenu("ComponentsMythic/Managers/Battle/Battle")]
public class Battle : Manager
{
    public Unit unit;
    public int round;
    public int turn;

    public List<GameObject> units;

    public void getUnits()
    {
        foreach (GameObject el in GameObject.FindGameObjectsWithTag("Unit")){
            this.units.Add(el);
        }
    }

    public override void Update()
    {

    }

    public override void start()
    {
        this.getUnits();
        this.round = 1;
        this.turn = 1;
        
    }
}
