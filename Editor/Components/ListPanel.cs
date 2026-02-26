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

    public override void draw(){
        Label("Left Panel", EditorStyles.boldLabel);

    }

    public virtual void drawList<T>(IList<T> items, System.Action<int, T> onClicked, System.Func<T, string> getLabel = null){
        draw();
        
        getLabel ??= (item) => item?.ToString() ?? "(null)";
        scroll = EditorGUILayout.BeginScrollView(scroll);
        Label("List goes here...");
        for (int i = 0; i < items.Count; i++)
        {
            int index = i;              // closure fix
            T item = items[index];

            Button(getLabel(item), () => onClicked?.Invoke(index, item));
        }
            EditorGUILayout.EndScrollView();
    }

    public virtual void OnItemClicked(int index)
    {
        Debug.Log("Clicked index: " + index);
    }
}

#endif