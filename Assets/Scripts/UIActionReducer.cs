using System;
using UnityEngine;

public class UIActionReducer : MonoBehaviour
{
    public GameObject World;

    Grid2D _worldGrid2D;
    WorldIO _worldIO;

    Action<Vector2> _onWorldClick;

    void Awake()
    {
        ConnectToWorld(World);
    }

    void ConnectToWorld(GameObject world)
    {
        _worldGrid2D = world.GetComponent<Grid2D>();
        _worldIO = world.GetComponent<WorldIO>();
    }

    public void WorldClick(Vector2 position)
    {
        _onWorldClick?.Invoke(position);
    }

#region blueprints

    public void SelectBlueprint(BuildingDef buildingDef)
    {
        _onWorldClick = position => PlaceBlueprint(position, buildingDef);
    }

    void PlaceBlueprint(Vector2 position, BuildingDef buildingDef)
    {
        var cellPosition = _worldGrid2D.WorldToCell(position);
        _worldIO.BuildingManipulator.Construct(cellPosition, buildingDef);
    }
 
#endregion
#region tasks

    public void SelectCancelTask()
    {
        _onWorldClick = CancelTask;
    }

    void CancelTask(Vector2 position)
    {
        var cellPosition = _worldGrid2D.WorldToCell(position);
        _worldIO.BuildingManipulator.Cancel(cellPosition);
    }

    public void SelectDeconstructTask()
    {
        _onWorldClick = DeconstructTask;
    }

    void DeconstructTask(Vector2 position)
    {
        var cellPosition = _worldGrid2D.WorldToCell(position);
        _worldIO.BuildingManipulator.Deconstruct(cellPosition);
    }
    
#endregion
}