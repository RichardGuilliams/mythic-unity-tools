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
        folderPath = EnsureFolder(folderPath);
        string safeName = MakeSafeFileName(assetName);
        string path = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{safeName}.asset");
        return CreateAssetInstance(soType, path, selectAndPing);
    }

    public static string EnsureFolder(string folderPath)
    {
        string fullPath = "Assets/Resources/" + folderPath;
        if (AssetDatabase.IsValidFolder(fullPath)) return fullPath;
        CreateFolders(fullPath);      
        AssetDatabase.Refresh();
        return fullPath;
    }

    public static void CreateFolders(string folderPath){
        string[] parts = folderPath.Split('/');
        string current = parts[1]; // "Resources"

        for (int i = 2; i < parts.Length; i++)
        {
            string next = $"Assets/Resources/{parts[i]}";
            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder("Assets/Resources", parts[i]);
            }
            current = next;
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
        SaveAndRefreshAsset(instance);
    }

    private static void PingAsset(ScriptableObject instance){
            Selection.activeObject = instance;
            EditorGUIUtility.PingObject(instance);
    }

    private static void validateAssetCreation(Type soType, string assetName)
    {
        if (soType == null) throw new ArgumentNullException(nameof(soType));
        if (!typeof(ScriptableObject).IsAssignableFrom(soType))
            throw new ArgumentException($"Type must derive from ScriptableObject: {soType.FullName}");

        if (soType.IsAbstract || soType.ContainsGenericParameters)
            throw new ArgumentException($"Type must be non-abstract and non-generic: {soType.FullName}");

        if (string.IsNullOrWhiteSpace(assetName))
            throw new ArgumentException("assetName is null/empty.");
    }

    public static T CreateAsset<T>(string folderPath, string assetName, bool selectAndPing = true) where T : ScriptableObject
    {
        return (T)CreateAsset(typeof(T), folderPath, assetName, selectAndPing);
    }

    public static T CreateFromTemplate<T>(T template, string folderPath, string assetName, bool selectAndPing = true) where T : ScriptableObject
    {
        ValidateTemplate(template);
        T asset = CreateAsset<T>(folderPath, assetName, selectAndPing: false);
        EditorUtility.CopySerialized(template, asset);
        SaveAndRefreshAsset(asset);
        if(selectAndPing) PingAsset(asset);
        return asset;
    }

    public static void ValidateTemplate<T>(T template) where T : ScriptableObject
    {
        if (template == null) throw new ArgumentNullException(nameof(template));
    }

    private static void SaveAndRefreshAsset(ScriptableObject asset)
    {
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static string GetFolderPath(DefaultAsset folderAsset)
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
}
#endif