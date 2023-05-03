using System.Collections.Generic;
using System.Linq;

public static class TaskCreator
{
    public static ITask WorkOn(IWork work)
    {
        return new SequenceTask(new ITask[]
        {
            new MoveTask(work.transform.position),
            new WorkTask(work)
        });
    }

    public static ITask DeliverItem(ItemMass item, ulong mass, IInventoryAdd inventory)
    {
        new TaskNode(new MoveTask(item.transform.position), out var startNode)
        .To(new(new ItemFromWorldToBackpackTask(item, mass)))
        .To(new(new MoveTask(inventory.transform.position)),
            new(new DumpBackpackTask(), out var failureNode))
        .To(new(new ItemFromBackpackToInventoryTask(inventory, item.Def, mass)), failureNode)
        .To(null, failureNode);

        return new GraphTask(startNode);
    }

    public static ITask EatFood(
        IEnumerable<(ItemMass itemMass, ulong markedMass)> foodItems,
        IWork foodConsumption)
    {
        var failureNode = new TaskNode(new DumpBackpackTask());

        var item = foodItems.First();

        var startNode = new TaskNode(new MoveTask(item.itemMass.transform.position));
        var lastNode = startNode
        .To(new(new ItemFromWorldToBackpackTask(item.itemMass, item.markedMass)));

        foreach (var (itemMass, markedMass) in foodItems.Skip(1))
        {
            lastNode = lastNode
            .To(new(new MoveTask(itemMass.transform.position)), failureNode)
            .To(new(new ItemFromWorldToBackpackTask(itemMass, markedMass)), failureNode);
        }

        lastNode
        .To(new(new WorkTask(foodConsumption)), failureNode)
        .To(null, failureNode);

        return new GraphTask(startNode);
    }
}