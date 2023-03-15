public class TaskNode
{
    public ITask Task;

    public TaskNode SuccessNode;
    public TaskNode FailureNode;

    public uint LeadingSuccessArcs;
    public uint LeadingFailureArcs;

    public TaskNode(ITask task)
    {
        Task = task;
    }

    public TaskNode(ITask task, out TaskNode node)
    {
        Task = task;
        node = this;
    }

    public TaskNode To(TaskNode successNode, TaskNode failureNode = null)
    {
        if (successNode != null)
        {
            SuccessNode = successNode;
            SuccessNode.LeadingSuccessArcs++;
        }
        if (failureNode != null)
        {
            FailureNode = failureNode;
            FailureNode.LeadingFailureArcs++;
        }
        return successNode ?? failureNode;
    }
}