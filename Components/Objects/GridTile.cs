using UnityEngine;

[DisallowMultipleComponent]
public class GridTile : MonoBehaviour
{
    // Grid coordinate anchored at world origin (0,0,0)
    // Coord meaning depends on your chosen up axis:
    // - Y-up: (x, yLayer, z)
    // - Z-up: (x, y, zLayer)
    public Vector3Int coord;
}
