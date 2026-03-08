#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.IO;

public static class AssetFactory
{
    public static ScriptableObject CreateAsset(Type soType, string folderPath, string assetName, bool selectAndPing = true)
    {
        validateAssetCreation(soType, assetName);
        ProcessPaths(ref folderPath, ref assetName);
        //folderPath = EnsureFolder(folderPath);
        //string safeName = MakeSafeFileName(assetName);
        string path = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{assetName}.asset");
        return CreateAssetInstance(soType, path, selectAndPing);
    }

    private static void ProcessPaths(ref string folderPath, ref string assetName)
    {
        folderPath = EnsureFolder(folderPath);
        assetName = MakeSafeFileName(assetName);
    }

    private static string EnsureFolder(string folderPath)
    {
        string fullPath = "Assets/Resources/" + folderPath;
        if (AssetDatabase.IsValidFolder(fullPath)) return fullPath;
        CreateFolders(fullPath);      
        AssetDatabase.Refresh();
        return fullPath;
    }

    private static void CreateFolders(string folderPath){
        string[] parts = folderPath.Split('/');
        string current = $"{parts[0]}/{parts[1]}"; // "Resources"

        for (int i = 2; i < parts.Length; i++)
        {
            string next = $"{current}/{parts[i]}";
            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(current, parts[i]);
            }
            current += $"/{parts[i]}";
        }
    }



    private static ScriptableObject CreateAssetInstance(Type soType, string path, bool selectAndPing)
    {
        var instance = ScriptableObject.CreateInstance(soType);
        if (instance == null) throw new InvalidOperationException($"CreateInstance failed for {soType.FullName}");
        FinalizeAssetInstance(ref instance, path);
        if(selectAndPing) PingAsset(instance);
        return instance;
    }

    private static void FinalizeAssetInstance(ref ScriptableObject instance, string path)
    {
        // Register Undo so user can Ctrl+Z the asset creation
        Undo.RegisterCreatedObjectUndo(instance, "Create ScriptableObject Asset");
        AssetDatabase.CreateAsset(instance, path);
        SaveAndRefreshNewAsset(instance);
    }

    private static void PingAsset(ScriptableObject instance){
            Selection.activeObject = instance;
            EditorGUIUtility.PingObject(instance);
    }

    private static void validateAssetCreation(Type soType, string assetName)
    {
        if (soType == null) throw new ArgumentNullException(nameof(soType));
        if (!typeof(ScriptableObject).IsAssignableFrom(soType)) throw new ArgumentException($"Type must derive from ScriptableObject: {soType.FullName}");
        if (soType.IsAbstract || soType.ContainsGenericParameters) throw new ArgumentException($"Type must be non-abstract and non-generic: {soType.FullName}");
        if (string.IsNullOrWhiteSpace(assetName)) throw new ArgumentException("assetName is null/empty.");
    }

    private static T CreateAsset<T>(string folderPath, string assetName, bool selectAndPing = true) where T : ScriptableObject
    {
        return (T)CreateAsset(typeof(T), folderPath, assetName, selectAndPing);
    }

    private static T CreateFromTemplate<T>(T template, string folderPath, string assetName, bool selectAndPing = true) where T : ScriptableObject
    {
        ValidateTemplate(template);
        T asset = CreateAsset<T>(folderPath, assetName, selectAndPing: false);
        EditorUtility.CopySerialized(template, asset);
        SaveAndRefreshNewAsset(asset);
        if(selectAndPing) PingAsset(asset);
        return asset;
    }

    private static void ValidateTemplate<T>(T template) where T : ScriptableObject
    {
        if (template == null) throw new ArgumentNullException(nameof(template));
    }

    private static void SaveAndRefreshNewAsset(ScriptableObject asset)
    {
        EditorUtility.SetDirty(asset);
        SaveAndRefresh();
    }

    private static void SaveAndRefresh(){
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static string GetFolderPath(DefaultAsset folderAsset)
    {
        if (folderAsset == null) return null;
        string path = AssetDatabase.GetAssetPath(folderAsset);
        return AssetDatabase.IsValidFolder(path) ? path : null;
    }

    private static string MakeSafeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name.Trim();
    }

    // Deletion Logic
    //TODO: Refactor and organize.
    private static void ValidateDeletionPath(ref string assetPath)
    {
        if (string.IsNullOrWhiteSpace(assetPath)) throw new ArgumentException("assetPath is null/empty.");
        assetPath = assetPath.Replace("\\", "/");
        if(assetPath == "Assets" || assetPath == "Resources" || assetPath == "Assets/Resources") throw new ArgumentException($"Cannot delete root folders: {assetPath}");
        if(!assetPath.StartsWith("Assets/Resources")) throw new ArgumentException($"Only files that exist in Assets/Resources can be deleted: {assetPath}");
    }

    private static bool DeleteAssetAtPath(string assetPath, bool moveToTrash = true)
    {
        ValidateDeletionPath(ref assetPath);

        // Ensure it exists
        if (!AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath))
        {
            // Could still be a folder, so check folder too
            if (!AssetDatabase.IsValidFolder(assetPath)) return false;
        }

        bool ok = moveToTrash ? AssetDatabase.MoveAssetToTrash(assetPath) : AssetDatabase.DeleteAsset(assetPath);
        SaveAndRefresh();
        return ok;
    }

    public static bool DeleteAsset(UnityEngine.Object asset, bool moveToTrash = true)
    {
        if (asset == null) return false;

        string path = AssetDatabase.GetAssetPath(asset);
        if (string.IsNullOrEmpty(path)) return false; // not an asset (maybe scene object)

        return DeleteAssetAtPath(path, moveToTrash);
    }

    /// <summary>
    /// Convenience: delete by your factory-style inputs.
    /// folderPath here is your Resources subpath (same format you pass into CreateAsset),
    /// and assetName is the file name without ".asset".
    /// </summary>
    private static bool DeleteCreatedAsset(string folderPath, string assetName, bool moveToTrash = true)
    {
        validateAssetName(assetName);

        ProcessPaths(ref folderPath, ref assetName);

        // Most of your factory creates .asset
        string assetPath = $"{folderPath}/{assetName}.asset";
        assetPath = assetPath.Replace("\\", "/");

        return DeleteAssetAtPath(assetPath, moveToTrash);
    }

    // Small helper to keep validation consistent
    private static void validateAssetName(string assetName)
    {
        if (string.IsNullOrWhiteSpace(assetName))
            throw new ArgumentException("assetName is null/empty.");
    }

    private static bool DeleteFolder(string resourcesSubPath, bool moveToTrash = true)
    {
        string fullPath = "Assets/Resources/" + resourcesSubPath;
        fullPath = fullPath.Replace("\\", "/").TrimEnd('/');

        if (!AssetDatabase.IsValidFolder(fullPath))
            return false;

        return DeleteAssetAtPath(fullPath, moveToTrash);
    }

    private static bool DeleteAssetOfType<T>(string assetPath, bool moveToTrash = true) where T : UnityEngine.Object
    {
        var obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        if (obj == null) return false;
        return DeleteAsset(obj, moveToTrash);
    }

}
#endif