#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ListPanel : BaseGUIPanel
{

    private UnityAction boundAction;
    private Vector2 scroll;
    public override void OnEnable()
    {
        boundAction = () => Debug.Log("Item clicked");
    }   

    public override void draw(Rect area){
        Label("Left Panel", EditorStyles.boldLabel);
    }

    public virtual void drawList<T>(Rect area, List<T> items, System.Action<int, T> onClicked, System.Action<int, List<T>> onDelete, System.Func<T, string> getLabel = null){
        draw(area);
        
        getLabel ??= (item) => item?.ToString() ?? "(null)";
        scroll = EditorGUILayout.BeginScrollView(scroll);
        Label("List goes here...");
        for (int i = 0; i < items.Count; i++)
        {
            int index = i;              // closure fix
            T item = items[index];

            ButtonNestedRow.Draw(area, index, getLabel(item), item, items, onClicked, onDelete);

        }
            EditorGUILayout.EndScrollView();
    }
}

public static class ButtonNestedRow
{
    public static void Draw<TItem, TItems>(Rect area, int index, string label, TItem item, List<TItems> items, System.Action<int, TItem> onClicked, System.Action<int, List<TItems>> onDelete = null)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(label, GUILayout.ExpandWidth(true))) onClicked?.Invoke(index, item);
        if (GUILayout.Button("Delete", GUILayout.Width(60))) onDelete?.Invoke(index, items);
        GUILayout.EndHorizontal();
    }
}

#endif