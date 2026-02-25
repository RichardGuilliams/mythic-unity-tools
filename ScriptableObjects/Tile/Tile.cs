using System.Linq.Expressions;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTile", menuName = "Game/Tile")]
public class Tile : ScriptableObject
{
    public TileType tileType;
    public int MovementCost;
    public ElementType element;
    public bool contactDamage;
}
