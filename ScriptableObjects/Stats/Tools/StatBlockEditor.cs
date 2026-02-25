// StatBlockForgeWindow.cs
// Put this in an Editor folder: Assets/Editor/StatBlockForgeWindow.cs

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class StatBlockEditor : EditorWindow
{
    /*
    Asset creation: This will be the name of the newly created statBlock asset.
    this statblock will be a base template for a specific type of character/enemy/npc.
    Multiple statblocks can be created for different types of characters/enemies/npcs.
    becuase of this we will need the stat editor to be able to grab a collection of statblocks and cycle through them. 
    It will do this based on the folder that they are stored in
    */
    //Use base as the name for the specific statblock. This is because the statblock will be used as a template for all characters/enemies/npcs of that type.
    private string assetName = "NewStatBlock";

    private DefaultAsset targetFolder; // drag a project folder here

    // We will not load all Stats. we will instead creat a list of the stats manually for this specific statblock.
    // The reason is because these stats need to be able to be used as desired.
    private string resourcesFolder = "Stats"; // for Resources.LoadAll (NO leading "Resources/")

    // Populate options
    private StatBlock template; // optional template to copy from
    private bool populateFromTemplate = true;
    private bool populateFromResourcesFolder = false;

    // UX
    private bool pingAndSelect = true;

    [MenuItem("MythicTools/Stats/StatBlock Forge")]
    public static void Open()
    {
        GetWindow<StatForgeWindow>("StatBlock Forge");
    }

    private void OnGUI()
    {
        GUILayout.Label("Stat Forge", EditorStyles.boldLabel);
        EditorGUILayout.Space(6);

        assetName = EditorGUILayout.TextField("Asset Name", assetName);

        targetFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Target Folder",
            targetFolder,
            typeof(DefaultAsset),
            false
        );

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Populate", EditorStyles.boldLabel);

        template = (StatBlock)EditorGUILayout.ObjectField("Template (Optional)", template, typeof(StatBlock), false);

        populateFromTemplate = EditorGUILayout.ToggleLeft("Populate from Template", populateFromTemplate);
        populateFromResourcesFolder = EditorGUILayout.ToggleLeft("Populate from Resources Folder", populateFromResourcesFolder);

        using (new EditorGUI.DisabledScope(!populateFromResourcesFolder))
        {
            resourcesFolder = EditorGUILayout.TextField("Resources Subfolder", resourcesFolder);
            EditorGUILayout.HelpBox(
                "This uses Resources.LoadAll<StatBase>(resourcesFolder). " +
                "Example: if assets are in Assets/Resources/Stats, enter 'Stats'.",
                MessageType.Info
            );
        }

        EditorGUILayout.Space(6);
        pingAndSelect = EditorGUILayout.ToggleLeft("Select Created Asset", pingAndSelect);

        EditorGUILayout.Space(10);

        using (new EditorGUI.DisabledScope(string.IsNullOrWhiteSpace(assetName) || targetFolder == null))
        {
            if (GUILayout.Button("Forge StatBlock"))
            {
                Forge();
            }
        }
    }

    private void Forge()
    {
        string folderPath = AssetDatabase.GetAssetPath(targetFolder);
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogError("Target Folder must be a valid project folder asset.");
            return;
        }

        string safeName = MakeSafeFileName(assetName);
        string assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folderPath, safeName + ".asset"));

        // Create instance
        StatBlock created = ScriptableObject.CreateInstance<StatBlock>();

        // Populate from template (clone fields)
        if (populateFromTemplate && template != null)
        {
            EditorUtility.CopySerialized(template, created);
        }

        // Ensure folder string is set (if you rely on it at runtime/OnValidate)
        // We'll set it to whatever the user typed for Resources folder if that mode is enabled.
        if (populateFromResourcesFolder)
        {
            created.folder = resourcesFolder;
        }

        // Create asset on disk
        AssetDatabase.CreateAsset(created, assetPath);

        // Populate from resources folder into created.statData
        if (populateFromResourcesFolder)
        {
            PopulateFromResources(created, resourcesFolder);
            EditorUtility.SetDirty(created);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        if (pingAndSelect)
        {
            Selection.activeObject = created;
            EditorGUIUtility.PingObject(created);
        }

        Debug.Log($"Forged StatBlock at: {assetPath}");
    }

    private static void PopulateFromResources(StatBlock statBlock, string resFolder)
    {
        // Loads StatBase assets from Resources at edit-time too (works as long as assets are under Assets/Resources/)
        var stats = Resources.LoadAll<StatBase>(resFolder);

        if (stats == null || stats.Length == 0)
        {
            Debug.LogWarning($"No StatBase assets found in Resources/{resFolder}");
            return;
        }

        // If your StatBlock expects StatData entries, we create them here.
        // This assumes StatData contains a reference to StatBase or its data.
        // Adjust mapping below to match your StatData definition.

        if (statBlock.statData == null)
            statBlock.statData = new List<StatData>();

        statBlock.statData.Clear();

        foreach (var s in stats.OrderBy(x => x.name))
        {
            var sd = new StatData();

            // 🔧 IMPORTANT: You must adapt these lines to your real StatData fields.
            // Example possibilities:
            // sd.stat = s;
            // sd.name = s.statName;
            // sd.baseValue = s.defaultValue;

            // If StatData currently has no fields, you’ll add them and map here.

            statBlock.statData.Add(sd);
        }
    }

    private static string MakeSafeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name.Trim();
    }
}
#endif