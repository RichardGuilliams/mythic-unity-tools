using UnityEditor;

public class FileWatcher : AssetPostprocessor
{
    // We will refactor this to fire events for specific asset types when they are deleted.
    static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (var path in deletedAssets)
        {
            UnityEngine.Debug.Log($"Asset deleted: {path}");
        }
    }
}