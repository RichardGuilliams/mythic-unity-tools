
using UnityEngine;

[CreateAssetMenu(fileName = "MapGrid", menuName = "ComponentsMythic/Map/Grid")]
public class MapGrid : AbstractScriptable
{
    public Selector selector;
    public Color drawColor;
    public Vector3 origin;
    public int width;
    public int length;
    public int height;
    public float cellSize;

    public void mouseToGrid(Vector3 mousePos, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt((mousePos.x - this.origin.x) / this.cellSize);
        y = Mathf.FloorToInt((mousePos.y - this.origin.y) / this.cellSize);
        z = Mathf.FloorToInt((mousePos.z - this.origin.z) / this.cellSize);
    }

    public Vector3 gridToWorld(int x, int y, int z)
    {
        return new Vector3(x * this.cellSize + this.origin.x, y * this.cellSize + this.origin.y, z * this.cellSize + this.origin.z);
    }

    public void DrawGizmos()
    {
        Gizmos.color = this.drawColor;

        for(int y = 0; y <= this.height; y++)
        {
            float length = this.length * this.cellSize;
            float height = this.height * this.cellSize;
            float width = this.width * this.cellSize;
                float yOffset = this.cellSize * y;
            for (int x = 0; x <= this.width; x++)
            {
                float px = x * this.cellSize + this.origin.x;
                float py = this.origin.y;
                float pz = this.origin.z;
                Gizmos.DrawLine(new Vector3(px, py + yOffset, pz), new Vector3(px, py + yOffset, length + pz));
                Gizmos.DrawLine(new Vector3(px, py, pz), new Vector3(px, py, length + pz));
            }
            for (int z = 0; z <= this.length; z++)
            {
                float px = this.origin.x;
                float py = this.origin.y;
                float pz = z * this.cellSize + this.origin.z;
                Gizmos.DrawLine(new Vector3(px, py + yOffset, pz), new Vector3(width + px, py + yOffset, pz));
                Gizmos.DrawLine(new Vector3(px, py, pz), new Vector3(width + px, py, pz));
            }
        }


        for(int x = 0; x <= this.width; x++)
        {
            for (int z = 0; z <= this.length; z++)
            {
                            Gizmos.DrawLine(new Vector3(x + this.origin.x, this.origin.y ,z + this.origin.z), new Vector3(x + this.origin.x, this.cellSize * this.height + this.origin.y , z + this.origin.z));

            }
        }
    }
}