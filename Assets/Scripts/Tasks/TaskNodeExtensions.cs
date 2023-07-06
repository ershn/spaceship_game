// Simplify TaskNode manipulation by defining the next alias in every file that uses them
using TaskNode = Vertex<SuccessState, Task>;

public static class TaskNodeExtensions
{
    public static Task Task(this TaskNode node) => node.Value;

    public static TaskNode SuccessNode(this TaskNode node) => node[SuccessState.Success];

    public static TaskNode FailureNode(this TaskNode node) => node[SuccessState.Failure];
}
