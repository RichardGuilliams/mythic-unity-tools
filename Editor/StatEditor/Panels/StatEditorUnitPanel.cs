#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class StatEditorUnitPanel : BaseGUIPanel
{

    public override void draw()
    {
        GUILayout.Label("Unit Editor", EditorStyles.boldLabel);
        GUILayout.Label("UI here...");
    }
}
#endif