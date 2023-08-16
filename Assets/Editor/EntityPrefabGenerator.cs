using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EntityPrefabGenerator
{
    const string ItemDefsFolder = "Assets/Definitions/Items";
    const string ItemPrefabsFolder = "Assets/Prefabs/Items";

    static readonly ulong s_defaultMass = 100.KiloGrams();
    static readonly ulong s_defaultCount = 1;

    const string StructureDefsFolder = "Assets/Definitions/Structures";
    const string StructurePrefabsFolder = "Assets/Prefabs/Structures";

    [MenuItem("Utils/Generate entity prefabs")]
    static void GenerateEntityPrefabs()
    {
        foreach (var asset in FindAssets<ItemDef>(ItemDefsFolder))
            CreateItemPrefab(asset);
        foreach (var asset in FindAssets<StructureDef>(StructureDefsFolder))
            CreateStructurePrefab(asset);
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

    static void CreatePrefabAsset(string path, GameObject prefab)
    {
        PrefabUtility.SaveAsPrefabAsset(prefab, path, out var success);
        if (!success)
            Debug.LogError($"Failed to create a prefab at {path}");
    }

    static void CreateItemPrefab(ItemDef itemDef)
    {
        var amount = itemDef.AmountMode is MassMode ? s_defaultMass : s_defaultCount;

        var prefab = (GameObject)PrefabUtility.InstantiatePrefab(itemDef.Prefab);
        ItemInstantiator.Setup(prefab, itemDef, amount);

        var prefabPath = $"{ItemPrefabsFolder}/{itemDef.name}.prefab";
        CreatePrefabAsset(prefabPath, prefab);

        Object.DestroyImmediate(prefab);
    }

    static void CreateStructurePrefab(StructureDef structureDef)
    {
        var prefab = (GameObject)PrefabUtility.InstantiatePrefab(structureDef.Prefab);
        StructureInstantiator.Setup(prefab, structureDef);

        var prefabPath = $"{StructurePrefabsFolder}/{structureDef.name}.prefab";
        CreatePrefabAsset(prefabPath, prefab);

        Object.DestroyImmediate(prefab);
    }
}
