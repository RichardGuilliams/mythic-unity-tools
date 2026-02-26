#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public abstract class BaseGUIPanel : EditorWindow
{

    public void BeginHorizontal()
    {
        GUILayout.BeginHorizontal();
    }

    public void EndHorizontal()
    {
        GUILayout.EndHorizontal();
    }

    public void BeginVertical()
    {
        GUILayout.BeginVertical();
    }
    public void EndVertical()
    {
        GUILayout.EndVertical();
    }

    public void FlexibleSpace()
    {
        GUILayout.FlexibleSpace();
    }

    public void Box(string text, GUIStyle style = null)
    {
        if (style == null) style = EditorStyles.helpBox;
        GUILayout.Box(text, style);
    }

    public void EndArea()
    {
        GUILayout.EndArea();
    }

    public void Space(float pixels)
    {
        GUILayout.Space(pixels);
    }

    public void Button(string text, UnityAction onClick, GUIStyle style = null)
    {
        if (style == null) style = EditorStyles.miniButton;
        if (GUILayout.Button(text, style))
        {
            onClick.Invoke();
        }
    }

    public void Label(string text, GUIStyle style = null)
    {
        if (style == null) style = EditorStyles.label;
        GUILayout.Label(text, style);
    }

    public void BeginArea(Rect area)
    {
        GUILayout.BeginArea(area);
    }
    public virtual void OnEnable(){}
    public virtual void draw(){}
}

#endif