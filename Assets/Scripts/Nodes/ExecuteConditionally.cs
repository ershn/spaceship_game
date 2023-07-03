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
    public ValueOutput Executed;

    protected override void Definition()
    {
        Enter = ControlInput("Enter", Control);
        Exit = ControlOutput("Exit");
        Condition = ValueInput("Condition", true);
        Executed = ValueOutput<bool>("Executed");

        Succession(Enter, Exit);
        Requirement(Condition, Enter);
        Assignment(Enter, Executed);
    }

    ControlOutput Control(Flow flow)
    {
        var condition = flow.GetValue<bool>(Condition);
        var executed = condition ? Execute(flow) : false;
        flow.SetValue(Executed, executed);
        return Exit;
    }

    bool Execute(Flow flow)
    {
        var vars = Variables.Graph(flow.stack);
        var state = GetState(vars, () => Init(flow.stack.gameObject));
        var executed = Execute(ref state);
        SetState(vars, state);
        return executed;
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