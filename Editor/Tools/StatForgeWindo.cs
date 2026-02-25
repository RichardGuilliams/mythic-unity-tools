#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class StatForgeWindow : EditorWindow
{
    private int tabIndex;
    private readonly string[] tabs = { "Forge", "Library", "Batch", "Settings" };

    [MenuItem("MythicTools/StatForge")]
    public static void Open() => GetWindow<StatForgeWindow>("");

    private void OnGUI()
    {
        tabIndex = GUILayout.Toolbar(tabIndex, tabs);
        GUILayout.Space(8);

        switch (tabIndex)
        {
            case 0: DrawForge(); break;
            case 1: DrawLibrary(); break;
            case 2: DrawBatch(); break;
            case 3: DrawSettings(); break;
        }
    }

    private void DrawForge()
    {
        EditorGUILayout.HelpBox("Make assets, stamp statblocks, etc.", MessageType.Info);
        if (GUILayout.Button("Forge Something")) Debug.Log("CLANG!");
    }

    private void DrawLibrary()
    {
        GUILayout.Label("Library", EditorStyles.boldLabel);
        // Search list, previews, etc.
    }

    private void DrawBatch()
    {
        GUILayout.Label("Batch Tools", EditorStyles.boldLabel);
        // Multi-asset operations
    }

    private void DrawSettings()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        // Toggles, paths, defaults
    }
}
#endif