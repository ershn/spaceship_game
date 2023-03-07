using UnityEngine;

public class ConstructionRequestManager : MonoBehaviour
{
    public TaskRequestEvent OnTaskCreation;

    void OnEnable()
    {
        ConstructionRequester.OnConstructionRequest += RequestConstruction;
    }

    void OnDisable()
    {
        ConstructionRequester.OnConstructionRequest -= RequestConstruction;
    }

    // TODO: handle canceled tasks
    void RequestConstruction(IWork work)
    {
        var task = new CompositeTask(new ITask[]
        {
            new MoveTask(work.transform.position),
            new WorkTask(work)
        });
        OnTaskCreation.Invoke(task, success => {});
    }
}