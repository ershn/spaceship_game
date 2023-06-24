using UnityEngine;

public class FloorConstructor : MonoBehaviour
{
    public static bool IsConstructibleAt(Vector2Int cellPosition, GridIndexes gridIndexes) =>
        !gridIndexes.FloorGrid.Has(cellPosition);

    void Start()
    {
        GetComponent<StructureConstructor>().Construct();
    }
}
