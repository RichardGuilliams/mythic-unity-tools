using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GridTilePlacerWindow : EditorWindow
{
    private enum UpAxis { Y, Z }

    [Header("Palette")]
    [SerializeField] private GameObject[] prefabTiles;
    [SerializeField] private int selectedIndex = 0;

    [Header("Grid")]
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private UpAxis upAxis = UpAxis.Y;
    [SerializeField] private int upLayer = 0; // layer along the chosen up axis (in cells)

    [Header("Brush")]
    [SerializeField] private bool paintEnabled = false;
    [SerializeField] private bool eraseMode = false;

    [Header("Placement")]
    [SerializeField] private Transform parentContainer;   // optional parent for spawned tiles

    [Header("Preview")]
    [SerializeField] private bool showGhostPreview = true;
    [SerializeField] private bool showCellWireCube = false; // optional helper

    // Cache prefab meshes so we don't re-scan constantly
    private GameObject cachedPrefab;
    private List<MeshPreviewItem> cachedMeshes = new List<MeshPreviewItem>();

    private struct MeshPreviewItem
    {
        public Mesh mesh;
        public Matrix4x4 localMatrix; // transform from prefab root to this mesh
    }

    // Prevent repeated placements in the same cell while dragging
    private static class DragCellGate
    {
        private static Vector3Int lastCoord;
        private static bool hasLast;

        public static bool Accept(Vector3Int coord, EventType type)
        {
            if (type == EventType.MouseDown)
            {
                lastCoord = coord;
                hasLast = true;
                return true;
            }

            if (!hasLast || coord != lastCoord)
            {
                lastCoord = coord;
                hasLast = true;
                return true;
            }

            return false;
        }
    }

    [MenuItem("MythicTools/Grid Tile Placer")]
    public static void Open()
    {
        GetWindow<GridTilePlacerWindow>("Grid Tile Placer");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += DuringSceneGUI;
        wantsMouseMove = true;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("3D Grid Paint Tool", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "Paints prefabs snapped to a grid anchored at world origin (0,0,0).\n" +
            "Left Click/Drag to paint. Hold Shift to erase.\n" +
            "One object per cell: painting over a cell replaces the existing tile.\n\n" +
            "Ctrl + Mouse Wheel: change layer along the chosen Up Axis.",
            MessageType.Info
        );

        EditorGUILayout.Space(6);

        // Palette list
        using (new EditorGUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Palette", EditorStyles.boldLabel);

            SerializedObject so = new SerializedObject(this);
            so.Update();
            EditorGUILayout.PropertyField(so.FindProperty("prefabTiles"), true);

            if (prefabTiles != null && prefabTiles.Length > 0)
            {
                selectedIndex = Mathf.Clamp(selectedIndex, 0, prefabTiles.Length - 1);
                string[] names = prefabTiles.Select(p => p ? p.name : "<null>").ToArray();
                int newIndex = EditorGUILayout.Popup("Selected Tile", selectedIndex, names);

                if (newIndex != selectedIndex)
                {
                    selectedIndex = newIndex;
                    InvalidatePreviewCache();
                    SceneView.RepaintAll();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Add at least one prefab tile to paint.", MessageType.Warning);
            }

            so.ApplyModifiedPropertiesWithoutUndo();
        }

        EditorGUILayout.Space(6);

        // Grid settings
        using (new EditorGUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Grid", EditorStyles.boldLabel);

            float newCell = Mathf.Max(0.01f, EditorGUILayout.FloatField("Cell Size", cellSize));
            if (!Mathf.Approximately(newCell, cellSize))
            {
                cellSize = newCell;
                SceneView.RepaintAll();
            }

            UpAxis newUp = (UpAxis)EditorGUILayout.EnumPopup("Up Axis", upAxis);
            if (newUp != upAxis)
            {
                upAxis = newUp;
                SceneView.RepaintAll();
            }

            int newLayer = EditorGUILayout.IntField($"{upAxis} Layer (cells)", upLayer);
            if (newLayer != upLayer)
            {
                upLayer = newLayer;
                SceneView.RepaintAll();
            }
        }

        EditorGUILayout.Space(6);

        // Preview settings
        using (new EditorGUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);
            showGhostPreview = EditorGUILayout.Toggle("Ghost Prefab Preview", showGhostPreview);
            showCellWireCube = EditorGUILayout.Toggle("Cell Wire Cube", showCellWireCube);
        }

        EditorGUILayout.Space(6);

        // Placement settings
        using (new EditorGUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Placement", EditorStyles.boldLabel);

            parentContainer = (Transform)EditorGUILayout.ObjectField(
                "Parent Container",
                parentContainer,
                typeof(Transform),
                true
            );

            eraseMode = EditorGUILayout.Toggle("Erase Mode", eraseMode);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(paintEnabled ? "Disable Paint" : "Enable Paint", GUILayout.Height(28)))
                {
                    paintEnabled = !paintEnabled;
                    SceneView.RepaintAll();
                }

                if (GUILayout.Button("Create Parent @ Origin", GUILayout.Height(28)))
                {
                    var go = new GameObject("Grid_Tiles");
                    Undo.RegisterCreatedObjectUndo(go, "Create Grid Parent");
                    go.transform.position = Vector3.zero;
                    parentContainer = go.transform;
                }
            }
        }
    }

    private void DuringSceneGUI(SceneView view)
    {
        if (!paintEnabled) return;

        Event e = Event.current;

        // Ctrl + mouse wheel adjusts layer
        if (e.type == EventType.ScrollWheel && e.control)
        {
            // e.delta.y is typically + when scrolling down, - when scrolling up
            int dir = (e.delta.y > 0f) ? -1 : 1;
            upLayer += dir;

            e.Use();
            Repaint();            // repaint window
            view.Repaint();       // repaint scene view
            SceneView.RepaintAll();
            return;
        }

        // Prevent scene selection while painting
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Plane plane = GetPaintPlane();

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        if (!plane.Raycast(ray, out float enter))
            return;

        Vector3 hit = ray.GetPoint(enter);

        Vector3Int coord = WorldToCoord(hit);
        Vector3 snappedWorld = CoordToWorld(coord);

        if (showCellWireCube)
            DrawCellCube(snappedWorld, cellSize, upAxis);

        if (showGhostPreview)
            DrawGhostPrefab(snappedWorld);

        bool isErase = eraseMode || e.shift;

        bool wantsPaint = (e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0;
        if (wantsPaint && DragCellGate.Accept(coord, e.type))
        {
            if (isErase)
            {
                EraseAt(coord);
            }
            else
            {
                ReplaceAt(coord); // one object per cell
            }

            e.Use();
        }

        if (e.type == EventType.MouseMove)
            view.Repaint();
    }

    private Plane GetPaintPlane()
    {
        float upWorld = upLayer * cellSize;

        if (upAxis == UpAxis.Y)
        {
            // Y-up: paint on XZ plane at y = upWorld
            return new Plane(Vector3.up, new Vector3(0f, upWorld, 0f));
        }
        else
        {
            // Z-up: paint on XY plane at z = upWorld
            return new Plane(Vector3.forward, new Vector3(0f, 0f, upWorld));
        }
    }

    private Vector3Int WorldToCoord(Vector3 world)
    {
        // Anchored at origin (0,0,0): coord = round(world / cellSize)
        if (upAxis == UpAxis.Y)
        {
            int x = Mathf.RoundToInt(world.x / cellSize);
            int z = Mathf.RoundToInt(world.z / cellSize);
            return new Vector3Int(x, upLayer, z);
        }
        else
        {
            int x = Mathf.RoundToInt(world.x / cellSize);
            int y = Mathf.RoundToInt(world.y / cellSize);
            return new Vector3Int(x, y, upLayer);
        }
    }

    private Vector3 CoordToWorld(Vector3Int coord)
    {
        return new Vector3(coord.x * cellSize, coord.y * cellSize, coord.z * cellSize);
    }

    // --- Replacement rules: one tile per cell ---
    private void ReplaceAt(Vector3Int coord)
    {
        EraseAt(coord);
        PlaceAt(coord);
    }

    private void PlaceAt(Vector3Int coord)
    {
        if (prefabTiles == null || prefabTiles.Length == 0) return;

        selectedIndex = Mathf.Clamp(selectedIndex, 0, prefabTiles.Length - 1);
        GameObject prefab = prefabTiles[selectedIndex];
        if (!prefab) return;

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        Undo.RegisterCreatedObjectUndo(instance, "Paint Tile");

        instance.transform.position = CoordToWorld(coord);
        instance.transform.rotation = Quaternion.identity;

        if (parentContainer != null)
            instance.transform.SetParent(parentContainer);

        GridTile marker = instance.GetComponent<GridTile>();
        if (!marker) marker = Undo.AddComponent<GridTile>(instance);
        marker.coord = coord;

        EditorUtility.SetDirty(instance);
    }

    private void EraseAt(Vector3Int coord)
    {
        GridTile existing = FindExistingAt(coord);
        if (existing != null)
            Undo.DestroyObjectImmediate(existing.gameObject);
    }

    private GridTile FindExistingAt(Vector3Int coord)
    {
        GridTile[] tiles = parentContainer
            ? parentContainer.GetComponentsInChildren<GridTile>(true)
            : Object.FindObjectsByType<GridTile>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null && tiles[i].coord == coord)
                return tiles[i];
        }

        return null;
    }

    // --- Ghost preview (wireframe meshes) ---
private void DrawGhostPrefab(Vector3 snappedWorld)
{
    GameObject prefab = GetSelectedPrefab();
    if (!prefab) return;

    EnsurePreviewCache(prefab);

    Matrix4x4 root = Matrix4x4.TRS(snappedWorld, Quaternion.identity, Vector3.one);

    // Use a simple colored material pass
    Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

    using (new Handles.DrawingScope(Color.cyan))
    {
        for (int i = 0; i < cachedMeshes.Count; i++)
        {
            var item = cachedMeshes[i];
            if (!item.mesh) continue;

            Matrix4x4 matrix = root * item.localMatrix;

            Graphics.DrawMeshNow(item.mesh, matrix);
        }
    }

    Vector3 up = (upAxis == UpAxis.Y) ? Vector3.up : Vector3.forward;
    Handles.DrawLine(snappedWorld, snappedWorld + up * (cellSize * 0.5f));
}


    private GameObject GetSelectedPrefab()
    {
        if (prefabTiles == null || prefabTiles.Length == 0) return null;
        selectedIndex = Mathf.Clamp(selectedIndex, 0, prefabTiles.Length - 1);
        return prefabTiles[selectedIndex];
    }

    private void InvalidatePreviewCache()
    {
        cachedPrefab = null;
        cachedMeshes.Clear();
    }

    private void EnsurePreviewCache(GameObject prefab)
    {
        if (cachedPrefab == prefab && cachedMeshes.Count > 0) return;

        cachedPrefab = prefab;
        cachedMeshes.Clear();

        // Grab MeshFilters
        var meshFilters = prefab.GetComponentsInChildren<MeshFilter>(true);
        foreach (var mf in meshFilters)
        {
            if (!mf || !mf.sharedMesh) continue;
            cachedMeshes.Add(new MeshPreviewItem
            {
                mesh = mf.sharedMesh,
                localMatrix = mf.transform.localToWorldMatrix // relative to prefab root *when prefab is root*
            });
        }

        // Grab SkinnedMeshRenderers too
        var skinned = prefab.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (var smr in skinned)
        {
            if (!smr || !smr.sharedMesh) continue;
            cachedMeshes.Add(new MeshPreviewItem
            {
                mesh = smr.sharedMesh,
                localMatrix = smr.transform.localToWorldMatrix
            });
        }

        // localToWorldMatrix above assumes prefab root at identity.
        // We need matrices relative to the prefab root, not world.
        // Convert by multiplying with inverse of prefab root localToWorld.
        Matrix4x4 rootInv = prefab.transform.localToWorldMatrix.inverse;
        for (int i = 0; i < cachedMeshes.Count; i++)
        {
            var item = cachedMeshes[i];
            item.localMatrix = rootInv * item.localMatrix;
            cachedMeshes[i] = item;
        }
    }

    private static void DecomposeTRS(Matrix4x4 m, out Vector3 pos, out Quaternion rot, out Vector3 scale)
    {
        pos = m.GetColumn(3);

        Vector3 x = m.GetColumn(0);
        Vector3 y = m.GetColumn(1);
        Vector3 z = m.GetColumn(2);

        scale = new Vector3(x.magnitude, y.magnitude, z.magnitude);

        // Avoid divide-by-zero
        if (scale.x != 0) x /= scale.x;
        if (scale.y != 0) y /= scale.y;
        if (scale.z != 0) z /= scale.z;

        rot = Quaternion.LookRotation(z, y);
    }

    // Optional helper: cell wire cube
    private static void DrawCellCube(Vector3 center, float cellSize, UpAxis upAxis)
    {
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
        Handles.DrawWireCube(center, Vector3.one * cellSize);

        Vector3 up = (upAxis == UpAxis.Y) ? Vector3.up : Vector3.forward;
        Handles.DrawLine(center, center + up * (cellSize * 0.5f));
    }
}
