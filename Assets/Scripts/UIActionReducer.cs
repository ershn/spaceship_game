using System;
using UnityEngine;

public class UIActionReducer : MonoBehaviour
{
    public GameObject World;

    Grid2D _worldGrid2D;
    WorldExternalIO _worldExternalIO;

    Action<Vector2> _onWorldClick;

    void Awake()
    {
        ConnectToWorld(World);
    }

    void ConnectToWorld(GameObject world)
    {
        _worldGrid2D = world.GetComponent<Grid2D>();
        _worldExternalIO = world.GetComponent<WorldExternalIO>();
    }

    public void WorldClick(Vector2 position)
    {
        _onWorldClick?.Invoke(position);
    }

    #region blueprints

    public void SelectBlueprint(StructureDef structureDef)
    {
        _onWorldClick = position => PlaceBlueprint(position, structureDef);
    }

    void PlaceBlueprint(Vector2 position, StructureDef structureDef)
    {
        var cellPosition = _worldGrid2D.WorldToCell(position);
        _worldExternalIO.StructureManipulator.Construct(cellPosition, structureDef);
    }

    #endregion
    #region tasks

    WorldLayer _structureLayers = WorldLayer.Floor;

    public void SelectCancelTask()
    {
        _onWorldClick = CancelTask;
    }

    void CancelTask(Vector2 position)
    {
        var cellPosition = _worldGrid2D.WorldToCell(position);
        _worldExternalIO.StructureManipulator.Cancel(cellPosition, _structureLayers);
    }

    public void SelectDeconstructTask()
    {
        _onWorldClick = DeconstructTask;
    }

    void DeconstructTask(Vector2 position)
    {
        var cellPosition = _worldGrid2D.WorldToCell(position);
        _worldExternalIO.StructureManipulator.Deconstruct(cellPosition, _structureLayers);
    }

    #endregion
}
