using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class EntityPlacer
{
    public static bool TryPickEntity(
        GridIndexes gridIndexes,
        Vector2Int position,
        out EntityDef entityDef
    )
    {
        var gameObject = gridIndexes.GlobalGrid.Get(position).FirstOrDefault();
        if (gameObject == null)
        {
            entityDef = null;
            return false;
        }

        entityDef = gameObject.GetComponent<EntityDefHolder>().EntityDef;
        return true;
    }

    public static bool TryPlaceEntity(
        EntityDef entityDef,
        GridIndexes gridIndexes,
        Vector2Int position,
        Transform parent,
        bool ignorePlacementRules = false
    )
    {
        if (
            !CanPlaceEntity(
                entityDef,
                gridIndexes,
                position,
                out var removedGameObjects,
                ignorePlacementRules
            )
        )
            return false;

        foreach (var gameObject in removedGameObjects.ToArray())
            Undo.DestroyObjectImmediate(gameObject);

        PlaceEntity(entityDef, gridIndexes.Grid, position, parent);

        if (!ignorePlacementRules)
            RemoveInconsistentEntities(gridIndexes, position);

        return true;
    }

    static bool CanPlaceEntity(
        EntityDef entityDef,
        GridIndexes gridIndexes,
        Vector2Int position,
        out IEnumerable<GameObject> removedGameObjects,
        bool ignorePlacementRules = false
    )
    {
        if (ignorePlacementRules)
        {
            removedGameObjects = gridIndexes.GlobalGrid.Get(position);
            return true;
        }

        return entityDef switch
        {
            ItemDef itemDef => CanPlaceItem(itemDef, gridIndexes, position, out removedGameObjects),
            StructureDef structureDef
                => CanPlaceStructure(structureDef, gridIndexes, position, out removedGameObjects),
            _ => throw new NotImplementedException()
        };
    }

    static bool CanPlaceItem(
        ItemDef itemDef,
        GridIndexes gridIndexes,
        Vector2Int position,
        out IEnumerable<GameObject> removedGameObjects
    )
    {
        if (gridIndexes.ItemGrid.TryGetItem(position, itemDef, out var item))
            removedGameObjects = new GameObject[] { item };
        else
            removedGameObjects = Array.Empty<GameObject>();

        return true;
    }

    static bool CanPlaceStructure(
        StructureDef structureDef,
        GridIndexes gridIndexes,
        Vector2Int position,
        out IEnumerable<GameObject> removedGameObjects
    )
    {
        var index = gridIndexes.GetStructureLayerIndex(structureDef.WorldLayer);
        if (index.TryGet(position, out var structure))
            removedGameObjects = new GameObject[] { structure };
        else
            removedGameObjects = Array.Empty<GameObject>();

        return structureDef.IsConstructibleAt(gridIndexes, position, ignoreExisting: true);
    }

    static void PlaceEntity(
        EntityDef entityDef,
        GridLayout grid,
        Vector2Int position,
        Transform parent
    )
    {
        var entity = (GameObject)PrefabUtility.InstantiatePrefab(entityDef.Prefab);
        entity.name = entityDef.name;
        entity.transform.parent = parent;
        entity.transform.localPosition =
            grid.CellToLocal((Vector3Int)position) + grid.GetLayoutCellCenter();
        entity.SetActive(true);

        Undo.RegisterCreatedObjectUndo(entity, "Place Entity");
    }

    static void RemoveInconsistentEntities(GridIndexes gridIndexes, Vector2Int position)
    {
        if (!gridIndexes.FurnitureGrid.TryGet(position, out var furniture))
            return;

        var furnitureDef = furniture.GetComponent<FurnitureDefHolder>().FurnitureDef;
        if (furnitureDef.IsConstructibleAt(gridIndexes, position, ignoreExisting: true))
            return;

        Undo.DestroyObjectImmediate(furniture);
    }

    public static void RemoveEntities(GridIndexes gridIndexes, Vector2Int position)
    {
        var gameObjects = gridIndexes.GlobalGrid.Get(position);
        foreach (var gameObject in gameObjects.ToArray())
            Undo.DestroyObjectImmediate(gameObject);
    }
}
