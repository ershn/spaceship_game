public interface IStateMachine
{
    void ToState(State state);
    void ToEnded();
}