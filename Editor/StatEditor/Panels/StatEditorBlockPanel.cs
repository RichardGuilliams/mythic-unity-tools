#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
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

    public override void OnEnable()
    {
        statBlocks = new List<StatBlock>();
        listPanel = CreateInstance<ListPanel>();
        // Load stat blocks from resources or create dummy data
    }

    public void SplitHorizontal(Rect area, float width)
    {
        left = new Rect(area.x, area.y, width, area.height - 160);
        right = new Rect(area.x + width + 6, area.y, area.width, area.height - 160);
    }

    public void SplitVertical(Rect area, float height)
    {
        top = new Rect(area.x, area.y, area.width, height);
        bottom = new Rect(area.x, area.y + height + 6, area.width, area.height - height - 6);
    }

    public override void draw(Rect area)
    {
        SplitHorizontal(area, 260);

        BeginArea(left);
        Label("Base Stat Blocks", EditorStyles.boldLabel);
        Space(6);
        AssetPath = (EditorGUILayout.TextField("Asset Folder", AssetPath));
        AssetName = (EditorGUILayout.TextField("Asset Name", AssetName));
        Button("Create Stat Block", () => onCreateNewAsset());    
        listPanel.drawList(left, statBlocks, (index, block) => Debug.Log("Clicked on: " + block.name),  (index, statBlocks) => DeleteListEntry(index, statBlocks), (block) => block.name);
        Space(6);
        EditorGUI.DrawRect(new Rect(left.width - 1, 0, 12, area.height), new Color(0,0,0,0.25f));
        EndArea();
 
        BeginArea(right);
        Label("Stat Editor", EditorStyles.boldLabel);
        Label("UI here...");
        EndArea();
    }

    private void DeleteListEntry(int index, List<StatBlock> list){
        AssetFactory.DeleteAsset(list[index]);
        list.RemoveAt(index);
    }

    private void onCreateNewAsset()
    {
        // Create a new stat block and add it to the list
        StatBlock newStatBlock = (StatBlock)AssetFactory.CreateAsset(typeof(StatBlock), AssetPath, AssetName, true);
        newStatBlock.name = AssetName;
        statBlocks.Add(newStatBlock);
    }
}

#endif