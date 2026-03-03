#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ConsolePanel : BaseGUIPanel
{
    private Vector2 scroll;
    private float width;

    public override void OnEnable()
    {
        Logs = new List<string>();
    }

    public override void draw(Rect area)
    {
        // Top border
            
        BeginArea(new Rect(area.x, area.height - (140f), area.width, 140f));
        EditorGUI.DrawRect(new Rect(area.x, 0, area.width, 1), new Color(0,0,0,0.25f));
        Space(6);
        Label("Console / Diagnostics", EditorStyles.boldLabel);

        scroll = EditorGUILayout.BeginScrollView(scroll);
        if(Logs.Count != 0){
            Logs.ForEach(log => GUILayout.Label(log));  
        }
        EditorGUILayout.EndScrollView();

        EndArea();
    }

    private List<string> Logs;

    public void Log(string message)
    {
        // Implement logging to the bottom panel
        Logs.Add(message);
    }
}

#endif