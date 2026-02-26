#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class StatEditorSettingsPanel : BaseGUIPanel
{

    public override void draw()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        GUILayout.Label("UI here...");
    }
}

#endif