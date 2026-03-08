using UnityEditor;

public class FileWatcher : AssetPostprocessor
{
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