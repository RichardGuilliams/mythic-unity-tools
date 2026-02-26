#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class StatEditor : EditorWindow
{
    private StatEditorBlockPanel statEditorBlockPanel;
    private StatEditorUnitPanel statEditorUnitPanel;
    private StatEditorSettingsPanel statEditorSettingsPanel;

    // Top-level tabs   
    private int topTab;
    private readonly string[] topTabs = { "Stat Block", "Units", "Settings" };


    // Layout state
    private Vector2 leftScroll;
    private Vector2 rightScroll;
    private Vector2 bottomScroll;

    private float leftWidth = 260f;
    private float bottomHeight = 140f;

    [MenuItem("MythicTools/Stat Editor")]
    public static void Open() => GetWindow<StatEditor>("Stat Editor");

    private void OnEnable()
    {
        // Initialize any necessary data here
        statEditorBlockPanel = CreateInstance<StatEditorBlockPanel>();
        statEditorUnitPanel = CreateInstance<StatEditorUnitPanel>();
        statEditorSettingsPanel = CreateInstance<StatEditorSettingsPanel>();

    }

    private void OnGUI()
    {
        DrawTopToolbar();
        DrawMainArea(getMainArea());
        DrawBottomPanel(getBottomArea());
    }

    private Rect getMainArea() => new Rect(0, 30, position.width, position.height - 30 - bottomHeight);
    private Rect getBottomArea() => new Rect(0, position.height - bottomHeight, position.width, bottomHeight);

    private void DrawTopToolbar()
    {
        topTab = GUILayout.Toolbar(topTab, topTabs);
        GUILayout.Space(6);
    }

    private void DrawMainArea(Rect area)
    {
        // Split into left/right
        //Rect(x, y, width, height)
        Rect main = new Rect(0, 30, area.width, area.height);
        Rect left = new Rect(0, 30, leftWidth, area.height - 30);
        Rect right = new Rect(leftWidth + 6, 30, area.width - leftWidth - 6, area.height - 30);

        // Divider line with DrawRect(new Rect(x, y width, height), new Color(r,g,b,a))


        DrawArea();

    }



    private void DrawArea()
    {
        rightScroll = EditorGUILayout.BeginScrollView(rightScroll);

        switch (topTab)
        {
            case 0:
                statEditorBlockPanel.draw();
                break;

            case 1:
                statEditorUnitPanel.draw();
                break;

            case 2:
                statEditorSettingsPanel.draw();
                break;
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawBottomPanel(Rect area)
    {
        // Top border
        EditorGUI.DrawRect(new Rect(0, area.y, area.width, 1), new Color(0,0,0,0.25f));

        GUILayout.BeginArea(new Rect(area.x, area.y + 6, area.width, area.height - 6));
        GUILayout.Label("Console / Diagnostics", EditorStyles.boldLabel);

        bottomScroll = EditorGUILayout.BeginScrollView(bottomScroll);
        GUILayout.Label("Logs, validation results, warnings...");
        EditorGUILayout.EndScrollView();

        GUILayout.EndArea();
    }
}
#endif