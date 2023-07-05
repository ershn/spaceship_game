using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ExecuteConditionally<TState> : Unit
{
    [Serialize, Inspectable, UnitHeaderInspectable("Variable")]
    public string Variable;

    [DoNotSerialize, PortLabelHidden]
    public ControlInput Enter;

    [DoNotSerialize, PortLabelHidden]
    public ControlOutput Exit;

    [DoNotSerialize, PortLabelHidden]
    public ValueInput Condition;

    [DoNotSerialize]
    public ValueOutput Completed;

    protected override void Definition()
    {
        Enter = ControlInput("Enter", Control);
        Exit = ControlOutput("Exit");
        Condition = ValueInput("Condition", true);
        Completed = ValueOutput<bool>("Completed");

        Succession(Enter, Exit);
        Requirement(Condition, Enter);
        Assignment(Enter, Completed);
    }

    ControlOutput Control(Flow flow)
    {
        var condition = flow.GetValue<bool>(Condition);
        var completed = condition && Execute(flow);
        flow.SetValue(Completed, completed);
        return Exit;
    }

    bool Execute(Flow flow)
    {
        var vars = Variables.Graph(flow.stack);
        var state = GetState(vars, () => Init(flow.stack.gameObject));
        var completed = Execute(ref state);
        SetState(vars, state);
        return completed;
    }

    TState GetState(VariableDeclarations variables, Func<TState> initializer)
    {
        if (variables.IsDefined(Variable))
            return variables.Get<TState>(Variable);
        else
            return initializer();
    }

    void SetState(VariableDeclarations variables, TState state)
    {
        variables.Set(Variable, state);
    }

    protected abstract TState Init(GameObject gameObject);

    protected abstract bool Execute(ref TState state);
}
