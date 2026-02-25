#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

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

        // Main content area (everything except bottom panel)
        Rect full = position;
        full.x = 0; full.y = 0; // GUI coords
        full.height -= bottomHeight;

        Rect bottom = new Rect(0, position.height - bottomHeight, position.width, bottomHeight);

        DrawMainArea(full);
        DrawBottomPanel(bottom);
    }

    private void DrawTopToolbar()
    {
        topTab = GUILayout.Toolbar(topTab, topTabs);
        GUILayout.Space(6);
    }

    private void DrawMainArea(Rect area)
    {
        // Split into left/right
        Rect left = new Rect(0, 30, leftWidth, area.height - 30);
        Rect right = new Rect(leftWidth + 6, 30, area.width - leftWidth - 6, area.height - 30);

        // Divider line
        EditorGUI.DrawRect(new Rect(leftWidth, 30, 1, area.height - 30), new Color(0,0,0,0.25f));

        GUILayout.BeginArea(left);
        DrawLeftPanel();
        GUILayout.EndArea();

        GUILayout.BeginArea(right);
        DrawRightPanel();
        GUILayout.EndArea();
    }

    private void DrawLeftPanel()
    {
        GUILayout.Label("Left Panel", EditorStyles.boldLabel);

        leftScroll = EditorGUILayout.BeginScrollView(leftScroll);
        GUILayout.Label("List goes here...");
        for (int i = 0; i < 30; i++)
            GUILayout.Button("Item " + i);
        EditorGUILayout.EndScrollView();
    }

    private void DrawRightPanel()
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

    public void drawStatEditorBlockPanel(){
        statEditorBlockPanel.draw();
                GUILayout.Label("Stat Editor", EditorStyles.boldLabel);
                GUILayout.Label("Stat Editor UI here...");
    }

    public void drawUnitEditorPanel(){
                GUILayout.Label("Unit Editor", EditorStyles.boldLabel);
                GUILayout.Label("Unit Editor UI here...");
    }

    public void drawSettingsPanel(){
                GUILayout.Label("Settings Editor", EditorStyles.boldLabel);
                GUILayout.Label("Settings UI here...");
    }
}

public class StatEditorBlockPanel : EditorWindow
{

    public void draw()
    {
        GUILayout.Label("Stat Editor", EditorStyles.boldLabel);
        GUILayout.Label("UI here...");
    }
}

public class StatEditorUnitPanel : EditorWindow
{

    public void draw()
    {
        GUILayout.Label("Unit Editor", EditorStyles.boldLabel);
        GUILayout.Label("UI here...");
    }
}

public class StatEditorSettingsPanel : EditorWindow
{

    public void draw()
    {
        GUILayout.Label("Unit Editor", EditorStyles.boldLabel);
        GUILayout.Label("UI here...");
    }
}



#endif