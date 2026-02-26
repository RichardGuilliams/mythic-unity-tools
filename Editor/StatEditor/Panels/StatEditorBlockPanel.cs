#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class StatEditorBlockPanel : BaseGUIPanel
{
    private string AssetPath;
    private Rect left = new Rect(0, 0, 260, 450);
    private Rect right = new Rect(266, 0, 400, 450);
    private ListPanel listPanel;

    private List<StatBlock> statBlocks;
    public override void OnEnable()
    {
        statBlocks = new List<StatBlock>();
        listPanel = CreateInstance<ListPanel>();
        // Load stat blocks from resources or create dummy data
    }

    public override void draw()
    {
        BeginArea(left);
        //GUILayout.BeginArea(left);
        GUILayout.Label("Base Stat Blocks", EditorStyles.boldLabel);
        GUILayout.Space(6);
        GUILayout.Button("Create New Stat Block");
        GUILayout.Space(6);
        listPanel.draw();
        GUILayout.Space(6);
        GUILayout.EndArea();
        GUILayout.BeginArea(right);
        GUILayout.Label("Stat Editor", EditorStyles.boldLabel);
        GUILayout.Label("UI here...");
        GUILayout.EndArea();
    }
}

#endif