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

public class LayoutHandler
{
    private Rect area;
    private Rect originalArea;

    public Rect Area { get => area; set => area = value; }

    /*
    This class will be passed down the ui stack and will handle the layout of the panels.
    each panel will modify the remaining area for the panels below it.
    leaving only leftover space for the panels below it.
    */

    // X Cannont be greater then the width of the area unless the space is inside a scroll view
    public float X { get => area.x; set => area.x = value; }
    // Y cannot be greater then the height of the area unless the space is inside a scroll view
    public float Y { get => area.y; set => area.y = value; }
    // width and height will be the position.width and height of the parent window.
    public float Width { get => area.width; set => area.width = value; }
    public float Height { get => area.height; set => area.height = value; }

    public float TakeWidth(float width)
    {
        float assignedWidth = Mathf.Min(width, area.width);
        area.width -= assignedWidth;
        return assignedWidth;
    }

    public float GetWidthTaken(){
        return originalArea.width - area.width;
    }

    public float GetNextX()
    {
        return area.x + GetWidthTaken();
    }

    public float TakeHeight(float height)
    {
        float assignedHeight = Mathf.Min(height, area.height);
        area.height -= assignedHeight;
        return assignedHeight;
    }

    public float GetHeightTaken(){
        return originalArea.height - area.height;
    }

    public float GetNextY()
    {
        return area.y + GetHeightTaken();
    }

    public void reset()
    {
        area = originalArea;
    }
}
#endif