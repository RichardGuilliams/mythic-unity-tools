#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class StatEditorBlockPanel : BaseGUIPanel
{
    private string assetPath;
    private string assetName;

    public string AssetPath{get => assetPath; set => assetPath = value;}
    public string AssetName{get => assetName; set => assetName = value;}

    private Rect left;
    private Rect right;
    private Rect top;
    private Rect bottom;

    private ListPanel listPanel;
    private List<StatBlock> statBlocks;
    private StatBlock selectedStatBlock;

    public override void OnEnable()
    {
        statBlocks = new List<StatBlock>();
        listPanel = CreateInstance<ListPanel>();
        // Load stat blocks from resources or create dummy data
    }

    public void SplitHorizontal(Rect area, float width)
    {
        left = new Rect(area.x, area.y, width, area.height - 160);
        right = new Rect(area.x + width + 6, area.y, area.width - (width + 12), area.height - 160);
    }

    public void SplitVertical(Rect area, float height)
    {
        top = new Rect(area.x, area.y, area.width, height);
        bottom = new Rect(area.x, area.y + height + 6, area.width, area.height - height - 6);
    }

    public override void draw(Rect area)
    {
        SplitHorizontal(area, 260);
        DrawLeftPanel(area);
        DrawRightPanel(area);
    }

    private void onCreateNewAsset()
    {
        // Create a new stat block and add it to the list
        StatBlock newStatBlock = (StatBlock)AssetFactory.CreateAsset(typeof(StatBlock), AssetPath, AssetName, true);
        newStatBlock.name = AssetName;
        statBlocks.Add(newStatBlock);
    }

    private void DeleteListEntry(int index, List<StatBlock> list){
        if(selectedStatBlock == list[index]) selectedStatBlock = null;                          
        AssetFactory.DeleteAsset(list[index]);
        list.RemoveAt(index);
    }

    private void SelectListEntry(int index, StatBlock statBlock){
        selectedStatBlock = statBlock;
        // Load the selected stat block into the editor
        Debug.Log("Selected: " + statBlock.name);
    }

    private void DrawLeftPanel(Rect area)
    {
        BeginArea(left);
        Label("Base Stat Blocks", EditorStyles.boldLabel);
        Space(6);
        AssetPath = (EditorGUILayout.TextField("Asset Folder", AssetPath));
        AssetName = (EditorGUILayout.TextField("Asset Name", AssetName));
        Button("Create Stat Block", () => onCreateNewAsset());    
        listPanel.drawList(left, statBlocks, (index, block) => SelectListEntry(index, block),  (index, statBlocks) => DeleteListEntry(index, statBlocks), (block) => block.name);
        Space(6);
        EditorGUI.DrawRect(new Rect(left.width - 1, 0, 12, area.height), new Color(0,0,0,0.25f));
        EndArea();
    }

    private void DrawRightPanel(Rect area)
    {
        BeginArea(right);
        Label("Stat Editor", EditorStyles.boldLabel);
        Space(6);
        if(selectedStatBlock != null) SetupRightPanel(area); 
        else Label("No stat block selected.");
        EndArea();
    }

    private void SetupRightPanel(Rect area)
    {
        Label($"Selected: {selectedStatBlock.name}");
        Button("Add New Stat", () => AddNewStat());
        DrawStats(area);
    }

    private void DrawStats(Rect area)
    {
        if(selectedStatBlock.statsList == null) selectedStatBlock.Setup();
        foreach(var stat in selectedStatBlock.statsList)
        {
            switch(stat)
            {
                case StatInt s:
                     DrawStatField<int>(s);
                    break;
                case StatFloat s: 
                     DrawStatField<float>(s);
                    break;
                case StatString s: 
                     DrawStatField<string>(s);
                    break;
            }
        }
    }

    private void DrawStatField<T>(StatBase stat)
    {
        GUILayout.BeginHorizontal();
        Label($"{stat.name}: ");
        Label("Value");
        ProcessStatField(stat.BoxedValue);  
        Label("Min");
        ProcessStatField(stat.BoxedMin);  
        Label("Max");
        ProcessStatField(stat.BoxedMax);  
        GUILayout.EndHorizontal();
    }

    private void ProcessStatField(object property){
        switch(property){
            case int i:
                property = EditorGUILayout.IntField(i);
                break;
            case float f: 
                property = EditorGUILayout.FloatField(f);
                break;
            case string s: 
                property = EditorGUILayout.TextField(s);
                break;
        }
    }

    private void AddNewStat()
    {
        PopupWindow.Show(new Rect(0, -160, 200, 200), new StatSelector(selectedStatBlock));
    }
}

#endif