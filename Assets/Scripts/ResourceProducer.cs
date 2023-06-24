using System.Linq;
using UnityEngine;

public class ResourceProducer : MonoBehaviour
{
    class Executor
    {
        ResourceProducerDef _def;
        ResourceProducer _io;
        Timer _timer;

        public Executor(ResourceProducerDef def, ResourceProducer io)
        {
            _def = def;
            _io = io;
            _timer = Timer.Repeat(def.Interval, Produce);
        }

        public void Run() => _timer.Run();

        void Produce()
        {
            if (UnityEngine.Random.value > _def.Probability)
                return;
            _io.CreateItem(_def.ItemDef, _def.Amount);
        }
    }

    public ItemCreator ItemCreator;

    GridPosition _gridPosition;

    Executor[] _executors;

    void Awake()
    {
        _gridPosition = GetComponent<GridPosition>();

        var structureDef = GetComponent<StructureDefHolder>().StructureDef;
        var resourceProducerDefs = structureDef.ResourceProducerDefIndex.Values;
        _executors = resourceProducerDefs.Select(def => new Executor(def, this)).ToArray();
    }

    void Update()
    {
        foreach (var executor in _executors)
            executor.Run();
    }

    void CreateItem(ItemDef itemDef, ulong amount) =>
        ItemCreator.Upsert(_gridPosition.CellPosition, itemDef, amount);
}
