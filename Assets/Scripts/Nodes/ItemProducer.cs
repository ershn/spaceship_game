using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Producers")]
public class ItemProducer : ExecuteConditionally<ItemProducer.State>
{
    [Serialize, Inspectable, UnitHeaderInspectable("Probability")]
    public float Probability;

    [Serialize, Inspectable, UnitHeaderInspectable("ItemDef")]
    public ItemDef ItemDef;

    [Serialize, Inspectable, UnitHeaderInspectable("Amount")]
    public ulong Amount;

    public class State
    {
        public ItemCreator ItemCreator;
        public GridPosition GridPosition;
    }

    protected override State Init(GameObject gameObject)
    {
        return new()
        {
            ItemCreator = gameObject.transform.root.GetComponent<WorldInternalIO>().ItemCreator,
            GridPosition = gameObject.GetComponent<GridPosition>()
        };
    }

    protected override bool Execute(ref State state)
    {
        if (Random.value <= Probability)
        {
            state.ItemCreator.Upsert(state.GridPosition.CellPosition, ItemDef, Amount);
            return true;
        }
        else
            return false;
    }
}
