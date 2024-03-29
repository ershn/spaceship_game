using System;
using UnityEngine;

public class UIActionReducer : MonoBehaviour
{
    [SerializeField]
    WorldCamera _worldCamera;

    [SerializeField]
    World _initialWorld;

    Grid _worldGrid;
    WorldExternalIO _worldExternalIO;

    Action<Vector2> _onWorldClick;

    void Start()
    {
        SelectWorld(_initialWorld);
    }

    #region worlds

    public void SelectWorld(World world)
    {
        _worldGrid = world.GetComponent<Grid>();
        _worldExternalIO = world.GetComponent<WorldExternalIO>();

        _worldCamera.SelectWorld(world);
    }

    public void WorldClick(Vector2 position)
    {
        _onWorldClick?.Invoke(position);
    }

    #endregion
    #region blueprints

    public void SelectBlueprint(StructureDef structureDef)
    {
        _onWorldClick = position => PlaceBlueprint(position, structureDef);
    }

    void PlaceBlueprint(Vector2 position, StructureDef structureDef)
    {
        var cellPosition = (Vector2Int)_worldGrid.WorldToCell(position);
        _worldExternalIO.StructureManipulator.Construct(cellPosition, structureDef);
    }

    #endregion
    #region tasks

    readonly WorldLayer _structureLayers = WorldLayer.Floor;

    public void SelectCancelTask()
    {
        _onWorldClick = CancelTask;
    }

    void CancelTask(Vector2 position)
    {
        var cellPosition = (Vector2Int)_worldGrid.WorldToCell(position);
        _worldExternalIO.StructureManipulator.Cancel(cellPosition, _structureLayers);
    }

    public void SelectDeconstructTask()
    {
        _onWorldClick = DeconstructTask;
    }

    void DeconstructTask(Vector2 position)
    {
        var cellPosition = (Vector2Int)_worldGrid.WorldToCell(position);
        _worldExternalIO.StructureManipulator.Deconstruct(cellPosition, _structureLayers);
    }

    #endregion
}
