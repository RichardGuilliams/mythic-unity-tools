#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class StatSelector : PopupWindowContent
{
    public StatBlock targetBlock;
    public List<StatBase> statsList;
    public override Vector2 GetWindowSize()
    {
        return new Vector2(200, 300);
    }

    public List<StatBase> LoadAllStats()
    {
        List<StatBase> stats = new List<StatBase>();

        string[] guids = AssetDatabase.FindAssets("t:StatBase");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            StatBase stat = AssetDatabase.LoadAssetAtPath<StatBase>(path);

            if (stat != null)
                stats.Add(stat);
        }

        return stats;
    }

    public override void OnGUI(Rect rect)
    {
        GUILayout.Label("Choose Stat", EditorStyles.boldLabel);
        foreach(var stat in LoadAllStats())
        {
            if (GUILayout.Button(stat.name))
            {
                Debug.Log($"{stat.name} selected");
                targetBlock.statsList.Add(stat);
                editorWindow.Close();
            }
        }
    }

    public StatSelector(StatBlock block)
    {
        targetBlock = block;
    }
}
#endif