using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class PrefabTemplater
{
    const string ItemDefsFolder = "Assets/Definitions/Items";
    const string ItemPrefabsFolder = "Assets/Prefabs/Items";

    const string StructureDefsFolder = "Assets/Definitions/Structures";
    const string StructurePrefabsFolder = "Assets/Prefabs/Structures";

    [MenuItem("Utils/Template prefabs")]
    static void TemplatePrefabs()
    {
        foreach (var asset in FindAssets<ItemDef>(ItemDefsFolder))
            TemplatePrefab(asset, ItemPrefabsFolder);
        foreach (var asset in FindAssets<StructureDef>(StructureDefsFolder))
            TemplatePrefab(asset, StructurePrefabsFolder);
    }

    static IEnumerable<T> FindAssets<T>(string folder)
        where T : Object
    {
        var assetGuids = AssetDatabase.FindAssets($"t:{typeof(T)}", new[] { folder });
        foreach (var assetGuid in assetGuids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset == null)
            {
                Debug.LogError($"Failed to load the asset at {assetPath}");
                continue;
            }
            yield return asset;
        }
    }

    static void TemplatePrefab<T>(T def, string folder)
        where T : ScriptableObject, ITemplatedPrefab
    {
        var prefab = (GameObject)PrefabUtility.InstantiatePrefab(def.Template);
        try
        {
            Templater.Template(prefab, def);
            var prefabPath = $"{folder}/{def.name}.prefab";
            def.Prefab = CreatePrefabAsset(prefabPath, prefab);
        }
        finally
        {
            Object.DestroyImmediate(prefab);
        }
    }

    static GameObject CreatePrefabAsset(string path, GameObject prefab)
    {
        var prefabAsset = PrefabUtility.SaveAsPrefabAsset(prefab, path, out var success);
        if (!success)
            Debug.LogError($"Failed to create a prefab at {path}");
        return prefabAsset;
    }
}
