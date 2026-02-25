using UnityEngine;
using System;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "Diamond", menuName = "ComponentsMythic/Selector/Shapes/Diamond")]
public class ShapeDiamond: SelectionShape
{

    public override void showPanels(GameObject obj)
    {
        for(int i = -range; i <= range; i++)
        {
            for(int j = -Math.Abs(i) + range; j >= -(-Math.Abs(i) + range); j--)
            {
                if(Math.Abs(i) + Math.Abs(j) != 0) this.spawnPanel(i, j, obj);
            }
        }
    }
}