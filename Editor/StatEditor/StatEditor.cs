#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class StatEditor : EditorWindow
{
    private Rect mainArea;

    private StatEditorBlockPanel statEditorBlockPanel;
    private StatEditorUnitPanel statEditorUnitPanel;
    private StatEditorSettingsPanel statEditorSettingsPanel;
    private ConsolePanel consolePanel;

    // Top-level tabs   
    private int topTab;
    private readonly string[] topTabs = { "Stat Block", "Units", "Settings" };

    [MenuItem("MythicTools/Stat Editor")]
    public static void Open() => GetWindow<StatEditor>("Stat Editor");

    private void OnEnable()
    {
        mainArea = new Rect(0, 20, position.width, position.height - 20);
        // Initialize any necessary data here
        statEditorBlockPanel = CreateInstance<StatEditorBlockPanel>();
        statEditorUnitPanel = CreateInstance<StatEditorUnitPanel>();
        statEditorSettingsPanel = CreateInstance<StatEditorSettingsPanel>();
        consolePanel = CreateInstance<ConsolePanel>();
    }

    private void OnGUI()
    {
        DrawTopToolbar();
        DrawMainArea();
        DrawBottomPanel();
    }

    private void DrawTopToolbar()
    {
        topTab = GUILayout.Toolbar(topTab, topTabs);
        GUILayout.Space(6);
    }

    private void DrawMainArea()
    {

        switch (topTab)
        {
            case 0:
                statEditorBlockPanel.draw(mainArea);
                break;

            case 1:
                statEditorUnitPanel.draw(mainArea);
                break;

            case 2:
                statEditorSettingsPanel.draw(mainArea);
                break;
        }
    }

    private void DrawBottomPanel()
    {
        consolePanel.draw(mainArea);
    }
}

#endif