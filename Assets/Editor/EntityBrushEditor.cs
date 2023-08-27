using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Tilemaps;
using UnityEngine;

[CustomEditor(typeof(EntityBrush))]
public class EntityBrushEditor : GridBrushEditorBase
{
    public override GameObject[] validTargets
    {
        get
        {
            var stageHandle = StageUtility.GetCurrentStageHandle();
            var worlds = stageHandle
                .FindComponentsOfType<World>()
                .OrderBy(world => world.Index)
                .Select(world => world.gameObject)
                .Where(gameObject => gameObject.scene.isLoaded && gameObject.activeInHierarchy);
            return worlds.ToArray();
        }
    }
}
